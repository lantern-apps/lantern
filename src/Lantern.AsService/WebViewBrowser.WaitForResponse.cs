using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Lantern.AsService;

public partial class WebViewBrowser
{
    public Task<WebViewHttpResponse> WaitForResponseAsync(string urlOrPredicate, WaitForResponseOptions? options = null)
    {
        var regex = urlOrPredicate.GlobToRegex();
        if (regex == null)
            throw new ArgumentException("Argument invalid", nameof(urlOrPredicate));

        return WaitForResponseAsync(regex, options);
    }

    public async Task<WebViewHttpResponse> WaitForResponseAsync(Regex urlOrPredicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;

        TaskCompletionSource<WebViewHttpResponse> tcs = new();

        await InvokeAsync(() => _webview.WebResourceResponseReceived += handler);
        try
        {
            return await tcs.Task.WithCancellation(options.Timeout, options.CancellationToken);
        }
        finally
        {
            await InvokeAsync(() => _webview.WebResourceResponseReceived -= handler);
        }

        async void handler(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            if (options.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            if (urlOrPredicate.IsMatch(e.Request.Uri))
            {
                Stream? content = null;
                try
                {
                    content = options.LoadContent ? await e.Response.GetContentAsync() : null;
                    //content = options.LoadContent ? await InvokeAsync(e.Response.GetContentAsync) : null;
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.Message);
                }

                _webview.WebResourceResponseReceived -= handler;
                tcs.TrySetResult(new WebViewHttpResponse(e, content));
            }
        };

    }

    public void SubscribeResponse(string urlOrPredicate, string? httpMethod, Func<WebViewHttpResponse, Task<bool>> next, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        var regex = urlOrPredicate.GlobToRegex() ?? throw new ArgumentException("Argument invalid", nameof(urlOrPredicate));

        InvokeAsync(() => _webview.WebResourceResponseReceived += handler);

        async void handler(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            if (options.CancellationToken.IsCancellationRequested)
            {
                _webview.WebResourceResponseReceived -= handler;
            }
            else if ((httpMethod == null || string.Equals(e.Request.Method, httpMethod, StringComparison.OrdinalIgnoreCase)) && regex.IsMatch(e.Request.Uri))
            {
                Stream? content = null;
                try
                {
                    content = options.LoadContent ? await e.Response.GetContentAsync() : null;
                    //content = options.LoadContent ? await InvokeAsync(e.Response.GetContentAsync) : null;
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.Message);
                }
                var response = new WebViewHttpResponse(e, content);

                if (!await next(response))
                {
                    _webview.WebResourceResponseReceived -= handler;
                }
            }
        };

    }

    public void SubscribeResponse(string urlOrPredicate, Func<WebViewHttpResponse, Task<bool>> next, WaitForResponseOptions? options = null)
    {
        SubscribeResponse(urlOrPredicate, null, next, options);
    }

    public async Task SubscribeResponseAsync(string urlOrPredicate, Func<WebViewHttpResponse, Task<bool>> next, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        var regex = urlOrPredicate.GlobToRegex() ?? throw new ArgumentException("Argument invalid", nameof(urlOrPredicate));

        TaskCompletionSource tcs = new();
        await InvokeAsync(() => _webview.WebResourceResponseReceived += handler);

        try
        {
            await tcs.Task.WithCancellation(options.Timeout, options.CancellationToken);
        }
        finally
        {
            await InvokeAsync(() => _webview.WebResourceResponseReceived -= handler);
        }

        async void handler(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            if (options.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            if (regex.IsMatch(e.Request.Uri))
            {
                Stream? content = null;
                try
                {
                    content = options.LoadContent ? await e.Response.GetContentAsync() : null;
                    //content = options.LoadContent ? await InvokeAsync(e.Response.GetContentAsync) : null;
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.Message);
                }

                var response = new WebViewHttpResponse(e, content);
                if (!await next(response))
                {
                    _webview.WebResourceResponseReceived -= handler;
                    tcs.SetResult();
                }
            }
        };
    }

    public void SubscribeResponse(Func<WebViewHttpResponse, Task<bool>> next, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;

        InvokeAsync(() => _webview.WebResourceResponseReceived += handler);

        async void handler(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            if (options.CancellationToken.IsCancellationRequested)
            {
                _webview.WebResourceResponseReceived -= handler;
                return;
            }

            Stream? content = null;
            try
            {
                content = options.LoadContent ? await e.Response.GetContentAsync() : null;
                //content = options.LoadContent ? await InvokeAsync(e.Response.GetContentAsync) : null;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
            var response = new WebViewHttpResponse(e, content);

            if (!await next(response))
            {
                _webview.WebResourceResponseReceived -= handler;
            }
        };
    }

    public async Task SubscribeResponseAsync(Func<WebViewHttpResponse, Task<bool>> next, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        TaskCompletionSource tcs = new();

        await InvokeAsync(() => _webview.WebResourceResponseReceived += handler);

        try
        {
            await tcs.Task.WithCancellation(options.Timeout, options.CancellationToken);
        }
        finally
        {
            await InvokeAsync(() => _webview.WebResourceResponseReceived -= handler);
        }

        async void handler(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            if (options.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            Stream? content = null;
            try
            {
                content = options.LoadContent ? await e.Response.GetContentAsync() : null;
                //content = options.LoadContent ? await InvokeAsync(e.Response.GetContentAsync) : null;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
            var response = new WebViewHttpResponse(e, content);

            if (!await next(response))
            {
                _webview.WebResourceResponseReceived -= handler;
                tcs.SetResult();
            }
        };
    }

    public async Task<WebViewHttpResponse> WaitForResponseAsync(Func<WebViewHttpResponse, bool> predicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;

        TaskCompletionSource<WebViewHttpResponse> tcs = new();

        await InvokeAsync(() => _webview.WebResourceResponseReceived += handler);

        try
        {
            return await tcs.Task.WithCancellation(options.Timeout, options.CancellationToken);
        }
        finally
        {
            await InvokeAsync(() => _webview.WebResourceResponseReceived -= handler);
        }

        async void handler(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            if (options.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            Stream? content = null;
            try
            {
                content = options.LoadContent ? await e.Response.GetContentAsync() : null;
                //content = options.LoadContent ? await InvokeAsync(e.Response.GetContentAsync) : null;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
            var response = new WebViewHttpResponse(e, content);
            if (predicate(response))
            {
                _webview.WebResourceResponseReceived -= handler;
                tcs.SetResult(response);
            }
        };
    }


    public Task<WebViewHttpResponse> RunAndWaitForResponseAsync(Action action, string urlOrPredicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        var waiter = WaitForResponseAsync(urlOrPredicate, options);
        action();
        return waiter.WithCancellation(options.Timeout, options.CancellationToken);
    }

    public Task<WebViewHttpResponse> RunAndWaitForResponseAsync(Action action, Regex urlOrPredicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        var waiter = WaitForResponseAsync(urlOrPredicate, options);
        action();
        return waiter.WithCancellation(options.Timeout, options.CancellationToken);
    }

    public Task<WebViewHttpResponse> RunAndWaitForResponseAsync(Action action, Func<WebViewHttpResponse, bool> predicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        var waiter = WaitForResponseAsync(predicate, options);
        action();
        return waiter.WithCancellation(options.Timeout, options.CancellationToken);
    }


    public async Task<WebViewHttpResponse> RunAndWaitForResponseAsync(Func<Task> action, string urlOrPredicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        var waiter = WaitForResponseAsync(urlOrPredicate, options);
        var task = action();
        await Task.WhenAll(waiter, task).WithCancellation(options.Timeout, options.CancellationToken);
        return waiter.Result;
    }

    public async Task<WebViewHttpResponse> RunAndWaitForResponseAsync(Func<Task> action, Regex urlOrPredicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        var waiter = WaitForResponseAsync(urlOrPredicate, options);
        var task = action();
        await Task.WhenAll(waiter, task).WithCancellation(options.Timeout, options.CancellationToken);
        return waiter.Result;
    }

    public async Task<WebViewHttpResponse> RunAndWaitForResponseAsync(Func<Task> action, Func<WebViewHttpResponse, bool> predicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        var waiter = WaitForResponseAsync(predicate, options);
        var task = action();
        await Task.WhenAll(waiter, task).WithCancellation(options.Timeout, options.CancellationToken);
        return waiter.Result;
    }
}

public class WaitForResponseOptions
{
    internal static readonly WaitForResponseOptions Default = new();
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
    public CancellationToken CancellationToken { get; set; }
    public bool LoadContent { get; set; }
}
