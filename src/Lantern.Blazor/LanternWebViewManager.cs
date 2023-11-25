using Lantern.Windows;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Extensions.FileProviders;

namespace Lantern.Blazor;

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

    private void OnWebViewMessageReceived(string source, string message)
    {
        MessageReceived(new Uri(source), message);
    }

    protected override async void NavigateCore(Uri absoluteUri)
    {
        await _window.SetUrlAsync(absoluteUri.ToString());
    }
}