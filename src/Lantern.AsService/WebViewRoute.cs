using Microsoft.Web.WebView2.Core;

namespace Lantern.AsService;

public class WebViewRoute
{
    private readonly CoreWebView2 _webview;
    private readonly CoreWebView2WebResourceRequestedEventArgs _eventArgs;
    private readonly WebViewHttpRequest _request;
    private readonly WebViewResourceType _resourceType;

    public WebViewRoute(CoreWebView2 webview, CoreWebView2WebResourceRequestedEventArgs eventArgs)
    {
        _webview = webview;
        _eventArgs = eventArgs;
        _request = new WebViewHttpRequest(_eventArgs.Request);
        _resourceType = (WebViewResourceType)eventArgs.ResourceContext;
    }

    public WebViewResourceType ResourceType => _resourceType;
    public WebViewHttpRequest Request => _request;

    public Task AbortAsync(string? errorCode = default)
    {
        return WebViewBrowser.InvokeAsync(() =>
        {
            _eventArgs.Response = _webview.Environment.CreateWebResourceResponse(null, 451, errorCode, null);
        });
    }

    public Task ContinueAsync(RouteContinueOptions? options = default)
    {
        if (options == null)
            return Task.CompletedTask;

        return WebViewBrowser.InvokeAsync(() =>
        {
            if (options.Method != null)
                _eventArgs.Request.Method = options.Method;

            if (options.Url != null)
                _eventArgs.Request.Uri = options.Url;

            if (options.Headers != null)
            {
                var keys = _eventArgs.Request.Headers.Select(x => x.Key).ToArray();
                foreach (var key in keys)
                {
                    _eventArgs.Request.Headers.SetHeader(key, null);
                }
                foreach (var header in options.Headers)
                {
                    _eventArgs.Request.Headers.SetHeader(header.Key, header.Value);
                }
            }

            if (options.PostData != null)
                _eventArgs.Request.Content = new MemoryStream(options.PostData, false);
        });
    }

}

public class RouteContinueOptions
{
    public RouteContinueOptions() { }

    public RouteContinueOptions(RouteContinueOptions clone)
    {
        if (clone == null)
        {
            return;
        }

        Headers = clone.Headers;
        Method = clone.Method;
        PostData = clone.PostData;
        Url = clone.Url;
    }

    /// <summary><para>If set changes the request HTTP headers. Header values will be converted to a string.</para></summary>
    public IEnumerable<KeyValuePair<string, string>>? Headers { get; set; }

    /// <summary><para>If set changes the request method (e.g. GET or POST).</para></summary>
    public string? Method { get; set; }

    /// <summary><para>If set changes the post data of request.</para></summary>
    public byte[]? PostData { get; set; }

    /// <summary><para>If set changes the request URL. New URL must have same protocol as original one.</para></summary>
    public string? Url { get; set; }
}
