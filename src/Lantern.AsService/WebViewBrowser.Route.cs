using Microsoft.Web.WebView2.Core;

namespace Lantern.AsService;

public partial class WebViewBrowser
{
    private readonly List<RouteHandler> _routes = new();

    public Task RouteAbortAsync(string urlOrPredicate) => RouteAsync(urlOrPredicate, WebViewResourceType.All, route => route.AbortAsync());
    public Task RouteAbortAsync(string urlOrPredicate, WebViewResourceType resourceType) => RouteAsync(urlOrPredicate, resourceType, route => route.AbortAsync());
    public Task RouteAbortAsync(WebViewResourceType resourceType) => RouteAsync("**", resourceType, route => route.AbortAsync());
    public Task RouteAsync(string urlOrPredicate, Action<WebViewRoute> action) => RouteAsync(urlOrPredicate, WebViewResourceType.All, action);

    public Task RouteAsync(string urlOrPredicate, WebViewResourceType resourceType, Action<WebViewRoute> action)
    {
        var pattern = urlOrPredicate.Trim();
        Func<Uri, bool> predicate;
        if (Uri.IsWellFormedUriString(pattern, UriKind.Absolute))
        {
            var uri = new Uri(pattern);
            predicate = uri.Equals;
        }
        else
        {
            var regex = pattern.GlobToRegex();
            if (regex == null)
            {
                ThrowHelper.ThrowInvaildUrlOrPredicate();
                return null!;
            }
            predicate = u => regex.IsMatch(u.AbsoluteUri);
        }

        var subscribed = _routes.Any(x => x.UrlOrPredicate == pattern && x.ResourceType == resourceType);
        _routes.Add(new RouteHandler(pattern, resourceType, predicate, action));

        if (subscribed)
            return Task.CompletedTask;

        return InvokeAsync(() =>
        {
            _webview.AddWebResourceRequestedFilter(pattern, (CoreWebView2WebResourceContext)resourceType);
        });
    }

    public Task UnrouteAsync(string urlOrPredicate)
    {
        var pattern = urlOrPredicate.Trim();

        for (int i = _routes.Count - 1; i >= 0; i--)
        {
            var route = _routes[i];
            if (route.UrlOrPredicate == urlOrPredicate)
                _routes.RemoveAt(i);
        }

        var subscribed = _routes.Any(x => x.UrlOrPredicate == pattern);
        if (subscribed)
            return Task.CompletedTask;

        return InvokeAsync(() =>
        {
            _webview.RemoveWebResourceRequestedFilter(pattern, CoreWebView2WebResourceContext.All);
        });
    }

    private void OnWebResourceRequested(object? o, CoreWebView2WebResourceRequestedEventArgs e)
    {
        if (_routes.Count == 0)
            return;

        if (_window.WindowClosed.IsCancellationRequested)
            return;

        var uri = new Uri(e.Request.Uri);
        var route = new WebViewRoute(_webview, e);
        var resourceType = (WebViewResourceType)e.ResourceContext;
        for (int i = 0; i < _routes.Count; i++)
        {
            var handler = _routes[i];
            if ((handler.ResourceType == WebViewResourceType.All || resourceType == handler.ResourceType) && 
                handler.Predicate(uri))
                handler.Action(route);
        }
    }

    private sealed class RouteHandler
    {
        public RouteHandler(
            string urlOrPredicate,
            WebViewResourceType resourceType,
            Func<Uri, bool> predicate,
            Action<WebViewRoute> action)
        {
            UrlOrPredicate = urlOrPredicate;
            ResourceType = resourceType;
            Predicate = predicate;
            Action = action;
        }

        public string UrlOrPredicate;
        public WebViewResourceType ResourceType;
        public readonly Func<Uri, bool> Predicate;
        public readonly Action<WebViewRoute> Action;

    }
}