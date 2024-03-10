using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Lantern.AsService;

public partial class WebViewBrowser
{
    public Task<WebViewHttpResponse> NavigateAndWaitForResponseAsync(string navigateUri, string waitUriOrPredicate, WaitForResponseOptions? options = null)
    {
        return NavigateAndWaitForResponseAsync(navigateUri, waitUriOrPredicate, null, options);
    }

    public Task<WebViewHttpResponse> NavigateAndWaitForResponseAsync(string navigateUri, string waitUriOrPredicate, string? waitHttpMethod, WaitForResponseOptions? options = null)
    {
        var regex = waitUriOrPredicate.GlobToRegex() ?? throw new ArgumentException("Argument invalid", nameof(waitUriOrPredicate));
        return NavigateAndWaitForResponseAsync(navigateUri, regex, waitHttpMethod, options);
    }

    public async Task<WebViewHttpResponse> NavigateAndWaitForResponseAsync(string navigateUri, Regex waitUriOrPredicate, string? waitHttpMethod, WaitForResponseOptions? options = null)
    {
        options ??= WaitForResponseOptions.Default;

        TaskCompletionSource<WebViewHttpResponse> tcs = new();

        await InvokeAsync(() =>
        {
            _webview.WebResourceResponseReceived += handler;
            _webview.Navigate(navigateUri);
        });

        try
        {
            return await tcs.Task.WithCancellation(options.Timeout, options.CancellationToken);
        }
        finally
        {
            await InvokeAsync(() =>
            {
                _webview.Stop();
                _webview.WebResourceResponseReceived -= handler;
            });
        }

        async void handler(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            if (options.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            if ((waitHttpMethod == null || string.Equals(e.Request.Method, waitHttpMethod, StringComparison.OrdinalIgnoreCase)) && waitUriOrPredicate.IsMatch(e.Request.Uri))
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

                _webview.WebResourceResponseReceived -= handler;
                tcs.TrySetResult(new WebViewHttpResponse(e, content));
            }
        };
    }
}
