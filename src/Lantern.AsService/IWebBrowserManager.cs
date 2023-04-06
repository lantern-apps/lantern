using Lantern.Windows;

namespace Lantern.AsService;

public interface IWebBrowserManager
{
    Task<WebViewBrowser> CreateAsync(WebViewWindowOptions windowOptions);
}
