using Lantern.AsService;
using System.Text.RegularExpressions;

namespace Lantern;

public sealed class WebViewRequestFilterWarpper
{
    private readonly Regex _regex;
    private readonly IWebViewRequestFilter _handler;

    public WebViewRequestFilterWarpper(IWebViewRequestFilter handler)
    {
        _regex = handler.UrlOrPredicate.GlobToRegex();
        _handler = handler;
    }

    public string UrlOrPredicate => _handler.UrlOrPredicate;
    public WebViewResourceType ResourceType => _handler.ResourceType;

    public bool CanHandle(string url, WebViewResourceType resourceType)
    {
        if (resourceType != ResourceType)
        {
            return false;
        }

        return _regex.IsMatch(url);
    }

    public Task HandleAsync(WebViewRequestContext context) => _handler.HandleAsync(context);
}
