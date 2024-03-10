﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Web.WebView2.Core;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Lantern.AsService;

public partial class WebViewBrowser
{
    /// <summary>
    /// 确保当前页面处于指定Url
    /// </summary>
    /// <param name="url"></param>
    /// <param name="options"></param>
    /// <returns>如果浏览器页面已是指定Url，则返回true，反之返回false</returns>
    public async Task<bool> EnsureUrlAsync(string url, LoadOptions? options = null)
    {
        var current = await UrlAsync();
        current = UrlUtility.GetUrlWithoutQueryString(current);
        url = UrlUtility.GetUrlWithoutQueryString(url);
        if (string.Equals(current, url, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        await GotoAsync(url, options);
        return false;
    }

    public async Task NavigateToAsync(string url)
    {
        var current = await UrlAsync();
        current = UrlUtility.GetUrlWithoutQueryString(current);
        url = UrlUtility.GetUrlWithoutQueryString(url);
        if (string.Equals(current, url, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        await InvokeAsync(() => _webview.Navigate(url));
    }

    public Task NavigateAsync(string url)
    {
        return InvokeAsync(() => _webview.Navigate(url));
    }

    public async Task<LoadResponse?> GotoAsync(string url, LoadOptions? options = null)
    {
        options ??= LoadOptions.Default;

        await InvokeAsync(() => _webview.Navigate(url));

        return await _navigationStateMachine.SubscribeAsync(
            u => u.Equals(url),
            options.WaitUntil,
            options.Timeout,
            options.CancellationToken);
    }

    public async Task<LoadResponse?> ReloadAsync(LoadOptions? options = null)
    {
        options ??= LoadOptions.Default;
        await InvokeAsync(() => _webview.Reload());

        return await _navigationStateMachine.SubscribeAsync(
            options.WaitUntil,
            options.Timeout,
            options.CancellationToken);
    }

    public Task<LoadResponse?> WaitForLoadStateAsync(LoadOptions? options = null)
    {
        options ??= LoadOptions.Default;

        return _navigationStateMachine.SubscribeAsync(
            options.WaitUntil,
            options.Timeout,
            options.CancellationToken);
    }

    public Task<LoadResponse?> WaitForUrlAsync(string urlOrPredicate, LoadOptions? options = null)
    {
        options ??= LoadOptions.Default;

        if (Uri.IsWellFormedUriString(urlOrPredicate, UriKind.Absolute))
        {
            var uri = new Uri(urlOrPredicate);
            return WaitForUrlAsync((Uri u) => uri.Equals(u), options);
        }

        var regex = urlOrPredicate.GlobToRegex();
        if (regex == null)
        {
            ThrowHelper.ThrowInvaildUrlOrPredicate();
            return null!;
        }

        return WaitForUrlAsync(regex, options);
    }

    public Task<LoadResponse?> WaitForUrlAsync(Regex urlOrPredicate, LoadOptions? options = null)
    {
        options ??= LoadOptions.Default;
        return _navigationStateMachine.SubscribeAsync(
            url => urlOrPredicate.IsMatch(url.AbsoluteUri),
            options.WaitUntil,
            options.Timeout,
            options.CancellationToken);
    }

    public Task<LoadResponse?> WaitForUrlAsync(Func<string, bool> predicate, LoadOptions? options = null)
    {
        options ??= LoadOptions.Default;
        return _navigationStateMachine.SubscribeAsync(
            url => predicate(url.AbsoluteUri),
            options.WaitUntil,
            options.Timeout,
            options.CancellationToken);
    }

    public Task<LoadResponse?> WaitForUrlAsync(Func<Uri, bool> predicate, LoadOptions? options = null)
    {
        options ??= LoadOptions.Default;
        return _navigationStateMachine.SubscribeAsync(
            url => predicate(url),
            options.WaitUntil,
            options.Timeout,
            options.CancellationToken);
    }


    private sealed class NavigationStateMachine
    {
        private sealed class WaitState(
            WaitUntilState waitUntil,
            TaskCompletionSource<LoadResponse?> completion,
            Func<Uri, bool>? predicate)
        {
            public readonly WaitUntilState WaitUntil = waitUntil;
            public readonly TaskCompletionSource<LoadResponse?> Completion = completion;
            public readonly Func<Uri, bool>? Predicate = predicate;
            public ulong NavigationId;

            public bool IsCompleted =>
                Completion.Task.IsCompleted ||
                Completion.Task.IsCanceled ||
                Completion.Task.IsFaulted;

        }

        private struct NavigationState
        {
            public bool IsLoading;
            public ulong NavigationId;
            public Uri? Url;

            public LoadState State;
            public CoreWebView2WebErrorStatus WebErrorStatus;
            public int HttpStatusCode;
            public bool IsSuccess;
        }

        internal enum LoadState
        {
            WaitForRuning,
            Starting,
            ContentLoading,
            DOMContentLoaded,
            Completed,
            Canceled,
        }

        private NavigationState _state = new();
        private ImmutableArray<WaitState> _waiters = ImmutableArray.Create<WaitState>();
        private readonly ILogger _logger;

        public NavigationStateMachine(CoreWebView2 webview, ILogger logger, CancellationToken cancellationToken)
        {
            _logger = logger;
            webview.NavigationCompleted += OnNavigationCompleted;
            webview.NavigationStarting += OnNavigationStarting;
            webview.DOMContentLoaded += OnDOMContentLoaded;
            webview.ContentLoading += OnContentLoading;

            cancellationToken.Register(() =>
            {
                ImmutableArray<WaitState> waiters = _waiters;
                foreach (WaitState waiter in waiters)
                {
                    if (waiter.IsCompleted)
                        continue;

                    waiter.Completion.SetCanceled(cancellationToken);
                }
                _logger.LogTrace("Navigation Canceled.");
                _waiters = ImmutableArray<WaitState>.Empty;

            });
        }

        public Task<LoadResponse?> SubscribeAsync(WaitUntilState waitUntil, TimeSpan timeout, CancellationToken cancellationToken) => SubscribeAsync(null, waitUntil, timeout, cancellationToken);

        public async Task<LoadResponse?> SubscribeAsync(Func<Uri, bool>? predicate, WaitUntilState waitUntil, TimeSpan timeout, CancellationToken cancellationToken)
        {
            if (_state.IsLoading && IsSatisfy(_state.State, waitUntil))
            {
                Debug.Assert(_state.Url != null);
                if (predicate == null || predicate(_state.Url))
                    return CreateCurrentResponse();
            }

            if (!_state.IsLoading && _state.Url != null)
            {
                if (predicate == null || predicate(_state.Url))
                    return CreateCurrentResponse();
            }

            TaskCompletionSource<LoadResponse?> completion = new();
            var waitter = new WaitState(waitUntil, completion, predicate);

            _waiters = _waiters.Add(waitter);

            CancellationTokenSource? cancellationTimoutSource = null;
            if (timeout > TimeSpan.Zero)
            {
                cancellationTimoutSource = new CancellationTokenSource(timeout);
                cancellationTimoutSource.Token.Register(() =>
                {
                    if (!waitter.IsCompleted)
                    {
                        waitter.Completion.SetCanceled(cancellationTimoutSource.Token);
                        _waiters = _waiters.Remove(waitter);
                    }
                });
            }

            cancellationToken.Register(() =>
            {
                if (!waitter.IsCompleted)
                {
                    waitter.Completion.SetCanceled(cancellationToken);
                    _waiters = _waiters.Remove(waitter);
                }
            });

            try
            {
                return await completion.Task;
            }
            catch (OperationCanceledException ex)
            {
                if (cancellationTimoutSource != null && ex.CancellationToken == cancellationTimoutSource.Token)
                    throw new TimeoutException();

                throw;
            }
            finally
            {
                cancellationTimoutSource?.Dispose();
            }
        }

        private void OnNavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
            _logger.LogTrace($"Navigation Starting {e.NavigationId} {e.Uri}");

            if (e.IsRedirected)
            {
                _state.Url = new Uri(e.Uri);
            }
            else
            {
                _state.IsLoading = true;
                _state.NavigationId = e.NavigationId;
                _state.State = LoadState.Starting;
                _state.Url = new Uri(e.Uri);
            }

            ImmutableArray<WaitState> waiters = _waiters;
            foreach (var waiter in waiters)
            {
                if (waiter.IsCompleted)
                    continue;

                if (waiter.NavigationId == 0 && (waiter.Predicate == null || waiter.Predicate(_state.Url)))
                {
                    waiter.NavigationId = _state.NavigationId;
                }
            }
        }

        private void OnContentLoading(object? sender, CoreWebView2ContentLoadingEventArgs e)
        {
            _logger.LogTrace($"Navigation ContentLoading {e.NavigationId}");
            _state.State = LoadState.ContentLoading;
            TrySetResult(WaitUntilState.ContentLoading);
        }

        private void OnDOMContentLoaded(object? sender, CoreWebView2DOMContentLoadedEventArgs e)
        {
            _logger.LogTrace($"Navigation DOMContentLoaded {e.NavigationId}");
            _state.State = LoadState.DOMContentLoaded;
            TrySetResult(WaitUntilState.DOMContentLoaded);
        }

        private void OnNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            _logger.LogTrace($"Navigation Completed {e.NavigationId}");

            _state.IsLoading = false;
            _state.State = LoadState.Completed;
            _state.IsSuccess = e.IsSuccess;
            _state.HttpStatusCode = e.HttpStatusCode;
            _state.WebErrorStatus = e.WebErrorStatus;
            TrySetResult(WaitUntilState.Completed);
        }

        private void TrySetResult(WaitUntilState waitUntil)
        {
            if (_waiters.Length == 0)
                return;

            ImmutableArray<WaitState> waiters = _waiters;
            for (int i = 0; i < waiters.Length; i++)
            {
                WaitState waiter = waiters[i];
                Debug.Assert(_state.Url != null);

                if (waiter.IsCompleted)
                    continue;

                if ((waiter.NavigationId == 0 || waiter.NavigationId == _state.NavigationId) && waiter.WaitUntil <= waitUntil)
                {
                    var response = CreateCurrentResponse();
                    waiter.Completion.SetResult(response);
                }
            }

            _waiters = waiters.RemoveAll(state => state.IsCompleted);
        }

        private LoadResponse? CreateCurrentResponse()
        {
            if (_state.Url == null || _state.State != LoadState.Completed)
                return null;

            if (_state.State != LoadState.Completed)
                return null;

            return new(
                _state.IsSuccess,
                _state.Url.AbsoluteUri,
                _state.HttpStatusCode,
                (WebViewErrorStatus)_state.WebErrorStatus);

        }

        private static bool IsSatisfy(LoadState navigationState, WaitUntilState waitState) => waitState switch
        {
            WaitUntilState.ContentLoading => navigationState >= LoadState.ContentLoading,
            WaitUntilState.DOMContentLoaded => navigationState >= LoadState.DOMContentLoaded,
            WaitUntilState.Completed => navigationState >= LoadState.Completed,
            _ => false,
        };
    }
}

public class LoadOptions
{
    public static readonly LoadOptions Default = new();
    public WaitUntilState WaitUntil { get; set; } = WaitUntilState.Completed;
    public CancellationToken CancellationToken { get; set; }
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
}

public class LoadResponse(bool success, string url, int httpStatusCode, WebViewErrorStatus? errorStatus)
{
    public WebViewErrorStatus? ErrorStatus { get; } = errorStatus;
    public int HttpStatusCode { get; } = httpStatusCode;
    public bool Success { get; } = success;
    public string Url { get; } = url;

}