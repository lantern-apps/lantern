using Lantern.Platform;
using Lantern.Windows;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Extensions.FileProviders;

namespace Lantern.Blazor;


public class BlazorWebViewWindow : WebViewWindow
{
    private readonly IServiceProvider _serviceProvider;
    private LanternWebViewManager? _webViewManager;

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
            null,
            null,
            "wwwroot/index.html");
        //_webViewManager.Navigate(_windowOptions.Url);
        base.OnWebViewInitialized();
    }
}

public class LanternWebViewManager : WebViewManager
{
    private static readonly Uri AppBaseUri = new("https://0.0.0.0/");

    private readonly WebViewWindow _window;
    public LanternWebViewManager(
                                 WebViewWindow webviewWindow,
                                 IServiceProvider provider,
                                 Dispatcher dispatcher,
                                 IFileProvider fileProvider,
                                 JSComponentConfigurationStore jsComponents,
                                 string hostPageRelativePath) : base(provider, dispatcher, AppBaseUri, fileProvider, jsComponents, hostPageRelativePath)
    {
        _window = webviewWindow;
        _window.WebViewMessageReceived += OnWebViewMessageReceived;
    }

    protected override void SendMessage(string message)
    {

        _window.PostMessage(message);
    }

    private async void OnWebViewMessageReceived(string message)
    {
        this.MessageReceived();
        throw new NotImplementedException();
    }

    protected override async void NavigateCore(Uri absoluteUri)
    {
        await _window.SetUrlAsync(absoluteUri.ToString());
    }
}