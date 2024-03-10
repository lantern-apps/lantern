using Microsoft.Web.WebView2.Core;
using System.Diagnostics;

namespace Lantern.AsService;

public sealed class FetchOptions
{
    public static readonly FetchOptions Default = new();
    public CancellationToken CancellationToken { get; set; }
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);
}

public partial class WebViewBrowser
{
    public async Task<WebViewHttpResponse> FetchAsync(string url, FetchOptions? options = null)
    {
        options ??= FetchOptions.Default;

        TaskCompletionSource<WebViewHttpResponse> tcs = new();

        await InvokeAsync(() =>
        {
            _webview.WebResourceResponseReceived += handler;
            _webview.Navigate(url);
        });

        try
        {
            return await tcs.Task.WithCancellation(options.Timeout, options.CancellationToken);
        }
        finally
        {
            await InvokeAsync(() =>
            {
                _webview.WebResourceResponseReceived -= handler;
                _webview.Stop();
            });
        }

        async void handler(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            if (options.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            if (e.Request.Uri.StartsWith(url, StringComparison.OrdinalIgnoreCase))
            {
                Stream? content = null;
                try
                {
                    content = await e.Response.GetContentAsync();
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.Message);
                }
                tcs.SetResult(new WebViewHttpResponse(e, content));
            }
        };
    }
}
