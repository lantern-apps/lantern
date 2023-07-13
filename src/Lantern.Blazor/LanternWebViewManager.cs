using Lantern.Windows;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Extensions.FileProviders;

namespace Lantern.Blazor;

public class LanternWebViewManager : WebViewManager
{
    private readonly WebViewWindow _window;
    public LanternWebViewManager(
                                 WebViewWindow webviewWindow,
                                 IServiceProvider provider,
                                 Dispatcher dispatcher,
                                 Uri appBaseUri,
                                 IFileProvider fileProvider,
                                 JSComponentConfigurationStore jsComponents,
                                 string hostPageRelativePath) : base(provider, dispatcher, appBaseUri, fileProvider, jsComponents, hostPageRelativePath)
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
        throw new NotImplementedException();
    }

    protected override async void NavigateCore(Uri absoluteUri)
    {
        await _window.SetUrlAsync(absoluteUri.ToString());
    }
}