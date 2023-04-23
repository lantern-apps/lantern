namespace Lantern;

public interface IWebViewRequestFilter
{
    string UrlOrPredicate { get; }
    WebViewResourceType ResourceType { get; }

    Task HandleAsync(WebViewRequestContext context);
}