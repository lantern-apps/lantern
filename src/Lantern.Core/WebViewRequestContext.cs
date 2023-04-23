using Lantern.Threading;
using Microsoft.Web.WebView2.Core;
using System.Text;

namespace Lantern;

public sealed class WebViewRequestContext
{
    private readonly CoreWebView2Environment _environment;
    private readonly CoreWebView2WebResourceRequestedEventArgs _event;

    internal WebViewRequestContext(CoreWebView2Environment environment, CoreWebView2WebResourceRequestedEventArgs @event)
    {
        _environment = environment;
        _event = @event;

        Headers = @event.Request.Headers;
    }

    public string Uri => _event.Request.Uri;
    public string Method => _event.Request.Method;
    public IEnumerable<KeyValuePair<string, string>> Headers { get; }

    public void Response(int statusCode, IEnumerable<KeyValuePair<string, string>>? headers = null, Stream? content = null)
    {
        var headerString = GetHeaderString(headers);
        _event.Response = _environment.CreateWebResourceResponse(content, statusCode, statusCode == 200 ? "OK" : null, headerString);
    }

    public void Response(int statusCode, IEnumerable<KeyValuePair<string, string>>? headers, byte[]? content)
    {
        Response(statusCode, headers, content == null ? null : new MemoryStream(content));
    }

    private static string? GetHeaderString(IEnumerable<KeyValuePair<string, string>>? headers)
    {
        if (headers == null)
            return null;

        StringBuilder sb = new();
        foreach (var header in headers)
        {
            sb.Append(header.Key);
            sb.Append(": ");
            sb.Append(header.Value);
            sb.AppendLine();
        }

        return sb.ToString();
    }
}