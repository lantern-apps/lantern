using Lantern.Threading;
using Lantern.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Core.DevToolsProtocolExtension;

namespace Lantern.AsService;

public sealed partial class WebViewBrowser
{
    private readonly WebViewWindow _window;
    private readonly CoreWebView2Controller _controller;
    private readonly CoreWebView2 _webview;
    private readonly NavigationStateMachine _navigationStateMachine;
    private readonly DevToolsProtocolHelper _helper;
    private readonly ILogger _logger;

    internal WebViewBrowser(WebViewWindow window, CoreWebView2Controller controller, ILogger logger)
    {
        _window = window;
        _controller = controller;
        _webview = controller.CoreWebView2;
        _logger = logger;
        _navigationStateMachine = new NavigationStateMachine(_webview, _logger, window.WindowClosed);
        _helper = _webview.GetDevToolsProtocolHelper();
        _webview.WebResourceRequested += OnWebResourceRequested;
        _webview.ProcessFailed += OnWebViewProcessFailed;
        _webview.SourceChanged += OnWebViewSourceChanged;
        _webview.NavigationCompleted += OnWebViewNavigationCompleted;
    }

    public event Action<string>? OnProcessFailed;
    public event Action<string>? OnSourceChanged;
    public event Action<string>? OnNavigationCompleted;

    private void OnWebViewNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        OnNavigationCompleted?.Invoke(_webview.Source);
    }

    private void OnWebViewSourceChanged(object? sender, CoreWebView2SourceChangedEventArgs e)
    {
        OnSourceChanged?.Invoke(_webview.Source);
    }

    private void OnWebViewProcessFailed(object? sender, CoreWebView2ProcessFailedEventArgs e)
    {
        OnProcessFailed?.Invoke(e.ProcessDescription);
    }

    public IWebViewWindow Window => _window;

    public DevToolsProtocolHelper Cdp => _helper;
    public CoreWebView2Controller Controller => _controller;

    public Task<MemoryStream> ScreenshotAsync()
    {
        return InvokeAsync(async () =>
        {
            MemoryStream ms = new();
            await _webview.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Png, ms);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        });
    }

    public Task<string> GetProfilePathAsync() => InvokeAsync(() => _webview.Profile.ProfilePath);

    public Task ClearCacheAsync(CoreWebView2BrowsingDataKinds kinds) => InvokeAsync(() => _webview.Profile.ClearBrowsingDataAsync(kinds));

    internal static Task InvokeAsync(Action action)
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            action();
            return Task.CompletedTask;
        }
        return Dispatcher.UIThread.InvokeAsync(action);
    }

    internal static Task<T> InvokeAsync<T>(Func<T> action)
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            return Task.FromResult(action());
        }
        return Dispatcher.UIThread.InvokeAsync(action);
    }

    internal static Task InvokeAsync(Func<Task> func)
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            return func();
        }
        return Dispatcher.UIThread.InvokeAsync(func);
    }

    internal static Task<T> InvokeAsync<T>(Func<Task<T>> func)
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            return func();
        }
        return Dispatcher.UIThread.InvokeAsync(func);
    }
}