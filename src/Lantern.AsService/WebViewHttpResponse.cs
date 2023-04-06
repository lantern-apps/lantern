using Microsoft.Web.WebView2.Core;

namespace Lantern.AsService;

public class WebViewHttpResponse
{
    private readonly CoreWebView2WebResourceResponseReceivedEventArgs _response;
    private readonly string _reasonPhrase;
    private readonly int _statusCode;
    private readonly Dictionary<string, string> _headers = new();
    private readonly WebViewHttpRequest _request;

    public WebViewHttpResponse(CoreWebView2WebResourceResponseReceivedEventArgs response)
    {
        _response = response;
        _reasonPhrase = _response.Response.ReasonPhrase;
        _statusCode = _response.Response.StatusCode;

        foreach(var header in _response.Response.Headers)
        {
            if (_headers.ContainsKey(header.Key))
            {
                var value = _headers[header.Key];
                _headers[header.Key] = value + ';' + header.Value;
            }
            else
            {
                _headers.Add(header.Key, header.Value);
            }
        }
        _request = new WebViewHttpRequest(_response.Request);
    }

    public string ReasonPhrase => _reasonPhrase;
    public int StatusCode => _statusCode;
    public IDictionary<string, string> Headers => _headers;
    public WebViewHttpRequest Request => _request;

    public Task<Stream> BodyAsync()
    {
        return _response.Response.GetContentAsync();
    }
}
