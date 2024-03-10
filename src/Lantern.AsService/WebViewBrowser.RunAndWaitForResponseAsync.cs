using System.Text.RegularExpressions;

namespace Lantern.AsService;

public partial class WebViewBrowser
{
    public Task<WebViewHttpResponse> RunAndWaitForResponseAsync(Action action, string urlOrPredicate, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        var waiter = WaitForResponseAsync(urlOrPredicate, options);
        action();
        return waiter.WithCancellation(options.Timeout, options.CancellationToken);
    }

    public Task<WebViewHttpResponse> RunAndWaitForResponseAsync(Action action, string urlOrPredicate, string? httpMethod, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;
        var waiter = WaitForResponseAsync(urlOrPredicate, httpMethod, options);
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
