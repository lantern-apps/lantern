using Lantern.Platform;
using Lantern.Windows;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.FileProviders;

namespace Lantern.Blazor;

public class BlazorWebViewWindow : WebViewWindow
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IFileProvider _fileProvider;
    private LanternWebViewManager _webViewManager;

    public BlazorWebViewWindow(WebViewEnvironmentOptions environmentOptions,
                               WebViewWindowOptions windowOptions,
                               IWindowManager? windowManager,
                               IWindowImpl windowImpl,
                               IServiceProvider serviceProvider) : base(environmentOptions, windowOptions, windowManager, windowImpl)
    {
        _serviceProvider = serviceProvider;
    }

    protected override void OnWebViewInitialized()
    {
        _webViewManager = new LanternWebViewManager(
            this,
            _serviceProvider,
            new LanternDispatcher(),
            _fileProvider,
            new JSComponentConfigurationStore(),
            "wwwroot/index.html");
        //_webViewManager.Navigate(_windowOptions.Url);
        base.OnWebViewInitialized();
    }
}
