using Microsoft.Web.WebView2.Core;

namespace Lantern.AsService;

public class WebViewHttpRequest
{
    private readonly CoreWebView2WebResourceRequest _request;
    private readonly Dictionary<string, string> _headers;
    private string _method;
    private Stream? _content;
    private string _url;

    public WebViewHttpRequest(CoreWebView2WebResourceRequest request)
    {
        _request = request;
        _url = _request.Uri;
        _method = _request.Method;
        _content = _request.Content;
        _headers = _request.Headers.ToDictionary(x => x.Key, x => x.Value);
    }

    public string Method
    {
        get => _method;
        set => _method = value;
    }

    public Stream? Content
    {
        get => _content;
        set => _content = value;
    }

    public string Url
    {
        get => _url;
        set => _url = value;
    }

    public IReadOnlyDictionary<string, string> Headers => _headers;

}
