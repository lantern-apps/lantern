namespace Lantern.AsService;

public static class WebViewHttpRequestExtensions
{
    public static string? GetUserAgent(this WebViewHttpRequest request)
    {
        request.Headers.TryGetValue("user-agent", out string? userAgent);
        return userAgent;
    }

    public static string? GetCookie(this WebViewHttpRequest request)
    {
        request.Headers.TryGetValue("cookie", out string? cookie);
        return cookie;
    }

}