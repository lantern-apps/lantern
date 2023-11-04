using Lantern.AsService;
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
        async void handler(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            if (urlOrPredicate.IsMatch(e.Request.Uri))
            {
                Stream? content = null;
                try
                {
                    content = options.LoadContent ? await e.Response.GetContentAsync() : null;
                }
                catch(Exception ex)
                {
                    Debug.Fail(ex.Message);
                }
                _webview.WebResourceResponseReceived -= handler;
                tcs.SetResult(new WebViewHttpResponse(e, content));
            }
        };

        await InvokeAsync(() => _webview.WebResourceResponseReceived += handler);
        try
        {
            return await tcs.Task.WithCancellation(options.Timeout, options.CancellationToken);
        }
        finally
        {
            await InvokeAsync(() => _webview.WebResourceResponseReceived -= handler);
        }
    }

    public async Task<WebViewHttpResponse> WaitForResponseAsync(Func<WebViewHttpResponse, bool> predicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;

        TaskCompletionSource<WebViewHttpResponse> tcs = new();

        async void handler(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            Stream? content = null;
            try
            {
                content = options.LoadContent ? await e.Response.GetContentAsync() : null;
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

        await InvokeAsync(() => _webview.WebResourceResponseReceived += handler);
        try
        {
            return await tcs.Task.WithCancellation(options.Timeout, options.CancellationToken);
        }
        finally
        {
            await InvokeAsync(() => _webview.WebResourceResponseReceived -= handler);
        }
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
