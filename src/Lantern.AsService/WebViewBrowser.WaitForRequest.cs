using Lantern.AsService;
using Microsoft.Web.WebView2.Core;
using System.Text.RegularExpressions;

namespace Lantern.AsService;

public partial class WebViewBrowser
{
    public Task RunAndWaitForRequestAsync(Action action, string urlOrPredicate, WaitForRequestOptions? options = null)
    {
        options ??= WaitForRequestOptions.Default;
        var task = WaitForRequestAsync(urlOrPredicate, options);
        action();
        return task.WithCancellation(options.Timeout, options.CancellationToken); ;
    }

    public Task RunAndWaitForRequestAsync(Action action, Regex urlOrPredicate, WaitForRequestOptions? options = null)
    {
        options ??= WaitForRequestOptions.Default;
        var task = WaitForRequestAsync(urlOrPredicate, options);
        action();
        return task.WithCancellation(options.Timeout, options.CancellationToken); ;
    }

    public Task RunWatiForRequestAsync(Action action, Func<WebViewHttpRequest, bool> predicate, WaitForRequestOptions? options = null)
    {
        options ??= WaitForRequestOptions.Default;
        var task = WaitForRequestAsync(predicate, options);
        action();
        return task.WithCancellation(options.Timeout, options.CancellationToken); ;
    }


    public Task RunAndWaitForRequestAsync(Func<Task> action, string urlOrPredicate, WaitForRequestOptions? options = null)
    {
        options ??= WaitForRequestOptions.Default;
        var waiter = WaitForRequestAsync(urlOrPredicate, options);
        var task = action();
        return Task.WhenAll(waiter, task).WithCancellation(options.Timeout, options.CancellationToken);
    }

    public Task RunAndWaitForRequestAsync(Func<Task> action, Regex urlOrPredicate, WaitForRequestOptions? options = null)
    {
        options ??= WaitForRequestOptions.Default;
        var waiter = WaitForRequestAsync(urlOrPredicate, options);
        var task = action();
        return Task.WhenAll(waiter, task).WithCancellation(options.Timeout, options.CancellationToken);
    }

    public Task RunWatiForRequestAsync(Func<Task> action, Func<WebViewHttpRequest, bool> predicate, WaitForRequestOptions? options = null)
    {
        options ??= WaitForRequestOptions.Default;
        var waiter = WaitForRequestAsync(predicate, options);
        var task = action();
        return Task.WhenAll(waiter, task).WithCancellation(options.Timeout, options.CancellationToken);
    }


    public Task WaitForRequestAsync(string urlOrPredicate, WaitForRequestOptions? options = null)
    {
        var regex = urlOrPredicate.GlobToRegex();
        if (regex == null)
            return Task.CompletedTask;

        return WaitForRequestAsync(regex, options);
    }

    public Task WaitForRequestAsync(Regex urlOrPredicate, WaitForRequestOptions? options = null)
    {
        options ??= WaitForRequestOptions.Default;

        TaskCompletionSource tcs = new();
        void handler(object? sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            if (urlOrPredicate.IsMatch(e.Request.Uri))
            {
                _webview.WebResourceRequested -= handler;
                tcs.SetResult();
            }
        };

        InvokeAsync(() => _webview.WebResourceRequested += handler);
        return tcs.Task.WithCancellation(options.Timeout, options.CancellationToken);
    }

    public Task WaitForRequestAsync(Func<WebViewHttpRequest, bool> predicate, WaitForRequestOptions? options = null)
    {
        options ??= WaitForRequestOptions.Default;

        TaskCompletionSource tcs = new();
        void handler(object? sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            if (predicate(new WebViewHttpRequest(e.Request)))
            {
                _webview.WebResourceRequested -= handler;
                _webview.RemoveWebResourceRequestedFilter("**", CoreWebView2WebResourceContext.All);
                tcs.SetResult();
            }
        };

        InvokeAsync(() =>
        {
            _webview.WebResourceRequested += handler;
            _webview.AddWebResourceRequestedFilter("**", CoreWebView2WebResourceContext.All);
        });
        return tcs.Task.WithCancellation(options.Timeout, options.CancellationToken);
    }
}

public class WaitForRequestOptions
{
    internal static readonly WaitForRequestOptions Default = new();
    public TimeSpan Timeout { get; set; }
    public CancellationToken CancellationToken { get; set; }
}
