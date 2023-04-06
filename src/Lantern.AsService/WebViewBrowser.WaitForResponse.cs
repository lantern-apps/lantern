using Lantern.AsService;
using Microsoft.Web.WebView2.Core;
using System.Text.RegularExpressions;

namespace Lantern.AsService;

public partial class WebViewBrowser
{
    public Task WaitForResponseAsync(string urlOrPredicate, WaitForResponseOptions? options = null)
    {
        var regex = urlOrPredicate.GlobToRegex();
        if (regex == null)
            return Task.CompletedTask;

        return WaitForResponseAsync(regex, options);
    }

    public async Task WaitForResponseAsync(Regex urlOrPredicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;

        TaskCompletionSource tcs = new();
        void handler(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            if (urlOrPredicate.IsMatch(e.Request.Uri))
            {
                _webview.WebResourceResponseReceived -= handler;
                tcs.SetResult();
            }
        };

        await InvokeAsync(() => _webview.WebResourceResponseReceived += handler);
        try
        {
            await tcs.Task.WithCancellation(options.Timeout, options.CancellationToken);
        }
        finally
        {
            await InvokeAsync(() => _webview.WebResourceResponseReceived -= handler);
        }
    }

    public async Task WaitForResponseAsync(Func<WebViewHttpResponse, bool> predicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;

        TaskCompletionSource tcs = new();

        void handler(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            if (predicate(new WebViewHttpResponse(e)))
            {
                _webview.WebResourceResponseReceived -= handler;
                tcs.SetResult();
            }
        };

        await InvokeAsync(() => _webview.WebResourceResponseReceived += handler);
        try
        {
            await tcs.Task.WithCancellation(options.Timeout, options.CancellationToken);
        }
        finally
        {
            await InvokeAsync(() => _webview.WebResourceResponseReceived -= handler);
        }
    }


    public Task RunAndWaitForResponseAsync(Action action, string urlOrPredicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        var waiter = WaitForResponseAsync(urlOrPredicate, options);
        action();
        return waiter.WithCancellation(options.Timeout, options.CancellationToken);
    }

    public Task RunAndWaitForResponseAsync(Action action, Regex urlOrPredicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        var waiter = WaitForResponseAsync(urlOrPredicate, options);
        action();
        return waiter.WithCancellation(options.Timeout, options.CancellationToken);
    }

    public Task RunAndWaitForResponseAsync(Action action, Func<WebViewHttpResponse, bool> predicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        var waiter = WaitForResponseAsync(predicate, options);
        action();
        return waiter.WithCancellation(options.Timeout, options.CancellationToken);
    }


    public Task RunAndWaitForResponseAsync(Func<Task> action, string urlOrPredicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        var waiter = WaitForResponseAsync(urlOrPredicate, options);
        var task = action();
        return Task.WhenAll(waiter, task).WithCancellation(options.Timeout, options.CancellationToken);
    }

    public Task RunAndWaitForResponseAsync(Func<Task> action, Regex urlOrPredicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        var waiter = WaitForResponseAsync(urlOrPredicate, options);
        var task = action();
        return Task.WhenAll(waiter, task).WithCancellation(options.Timeout, options.CancellationToken);
    }

    public Task RunAndWaitForResponseAsync(Func<Task> action, Func<WebViewHttpResponse, bool> predicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        var waiter = WaitForResponseAsync(predicate, options);
        var task = action();
        return Task.WhenAll(waiter, task).WithCancellation(options.Timeout, options.CancellationToken);
    }
}

public class WaitForResponseOptions
{
    internal static readonly WaitForResponseOptions Default = new();
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
    public CancellationToken CancellationToken { get; set; }
}
