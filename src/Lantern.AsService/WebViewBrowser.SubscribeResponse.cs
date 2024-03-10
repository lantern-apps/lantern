using Microsoft.Web.WebView2.Core;
using System.Diagnostics;

namespace Lantern.AsService;

public partial class WebViewBrowser
{

    public void SubscribeResponse(string urlOrPredicate, Func<WebViewHttpResponse, Task<bool>> next, WaitForResponseOptions? options = null)
    {
        SubscribeResponse(urlOrPredicate, null, next, options);
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
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.Message);
                }
                var response = new WebViewHttpResponse(e, content);

                try
                {
                    if (!await next(response))
                    {
                        _webview.WebResourceResponseReceived -= handler;
                    }
                }
                catch
                {
                    _webview.WebResourceResponseReceived -= handler;
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
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
            var response = new WebViewHttpResponse(e, content);

            try
            {
                if (!await next(response))
                {
                    _webview.WebResourceResponseReceived -= handler;
                }
            }
            catch
            {
                _webview.WebResourceResponseReceived -= handler;
            }
        };
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
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.Message);
                }

                var response = new WebViewHttpResponse(e, content);
                try
                {
                    if (!await next(response))
                    {
                        _webview.WebResourceResponseReceived -= handler;
                        tcs.SetResult();
                    }
                }
                catch (Exception ex)
                {
                    _webview.WebResourceResponseReceived -= handler;
                    tcs.SetException(ex);
                }
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
                _webview.WebResourceResponseReceived -= handler;
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
                if (!await next(response))
                {
                    _webview.WebResourceResponseReceived -= handler;
                    tcs.SetResult();
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
