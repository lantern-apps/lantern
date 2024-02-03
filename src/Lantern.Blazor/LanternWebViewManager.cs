using Lantern.Windows;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Extensions.FileProviders;

namespace Lantern.Blazor;

public class LanternWebViewManager : WebViewManager
{
    private static readonly string AppBaseUri = "http://0.0.0.0/";

    private readonly WebViewWindow _window;
    public LanternWebViewManager(
                                 WebViewWindow webviewWindow,
                                 IServiceProvider provider,
                                 Dispatcher dispatcher,
                                 IFileProvider fileProvider,
                                 JSComponentConfigurationStore jsComponents,
                                 string hostPageRelativePath) : base(provider, dispatcher, new Uri(AppBaseUri), fileProvider, jsComponents, hostPageRelativePath)
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
        MessageReceived(new Uri(AppBaseUri), message);
    }

    protected override async void NavigateCore(Uri absoluteUri)
    {
        await _window.SetUrlAsync(absoluteUri.ToString());
    }

    public Stream HandleWebRequest(object sender, string schema, string url, out string contentType)
    {
        // It would be better if we were told whether or not this is a navigation request, but
        // since we're not, guess.
        var localPath = (new Uri(url)).LocalPath;
        var hasFileExtension = localPath.LastIndexOf('.') > localPath.LastIndexOf('/');

        //Remove parameters before attempting to retrieve the file. For example: http://localhost/_content/Blazorise/button.js?v=1.0.7.0
        if (url.Contains('?')) url = url.Substring(0, url.IndexOf('?'));

        if (url.StartsWith(AppBaseUri, StringComparison.Ordinal)
            && TryGetResponseContent(url, !hasFileExtension, out var statusCode, out var statusMessage,
                out var content, out var headers))
        {
            headers.TryGetValue("Content-Type", out contentType);
            return content;
        }
        else
        {
            contentType = default;
            return null;
        }
    }

}