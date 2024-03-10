using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Lantern.AsService;

public partial class WebViewBrowser
{
    public Task<WebViewHttpResponse> WaitForResponseAsync(string urlOrPredicate, WaitForResponseOptions? options = null)
    {
        return WaitForResponseAsync(urlOrPredicate, null, options);
    }

    public Task<WebViewHttpResponse> WaitForResponseAsync(string urlOrPredicate, string? httpMethod, WaitForResponseOptions? options = null)
    {
        var regex = urlOrPredicate.GlobToRegex() ?? throw new ArgumentException("Argument invalid", nameof(urlOrPredicate));
        return WaitForResponseAsync(regex, httpMethod, options);
    }

    public Task<WebViewHttpResponse> WaitForResponseAsync(Regex urlOrPredicate, WaitForResponseOptions? options = null)
    {
        return WaitForResponseAsync(urlOrPredicate, null, options);
    }

    public async Task<WebViewHttpResponse> WaitForResponseAsync(Regex urlOrPredicate, string? httpMethod, WaitForResponseOptions? options = null)
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

            if ((httpMethod == null || string.Equals(e.Request.Method, httpMethod, StringComparison.OrdinalIgnoreCase)) && urlOrPredicate.IsMatch(e.Request.Uri))
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
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
            var response = new WebViewHttpResponse(e, content);
            try
            {
                if (predicate(response))
                {
                    _webview.WebResourceResponseReceived -= handler;
                    tcs.SetResult(response);
                }
            }
            catch (Exception ex)
            {
                _webview.WebResourceResponseReceived -= handler;
                tcs.SetException(ex);
            }
        };
    }

}

public class WaitForResponseOptions
{
    internal static readonly WaitForResponseOptions Default = new();
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
    public CancellationToken CancellationToken { get; set; }
    public bool LoadContent { get; set; }
}
