using Lantern.Platform;
using Lantern.Windows;
using Microsoft.Extensions.Logging;

namespace Lantern.AsService;

public class WebBrowserWindow : WebViewWindow
{
    private readonly ILogger _logger;
    public WebViewBrowser Browser { get; private set; } = null!;

    public WebBrowserWindow(
        WebViewEnvironmentOptions environmentOptions,
        WebViewWindowOptions windowOptions,
        IWindowImpl windowImpl,
        ILogger<WebBrowserWindow> logger)
        : base(environmentOptions, windowOptions, null, windowImpl)
    {
        _logger = logger;
    }

    protected override void OnWebViewInitialized()
    {
        Browser = new WebViewBrowser(this, _controller!, _logger);
        base.OnWebViewInitialized();
    }
}