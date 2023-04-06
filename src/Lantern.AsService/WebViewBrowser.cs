using Lantern.Threading;
using Lantern.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;

namespace Lantern.AsService;

public sealed partial class WebViewBrowser
{
    private readonly WebViewWindow _window;
    private readonly CoreWebView2 _webview;
    private readonly NavigationStateMachine _navigationStateMachine;
    private readonly ILogger _logger;

    internal WebViewBrowser(WebViewWindow window, CoreWebView2 webview, ILogger logger)
    {
        _window = window;
        _webview = webview;
        _logger = logger;
        _navigationStateMachine = new NavigationStateMachine(_webview, _logger, window.WindowClosed);
        _webview.WebResourceRequested += OnWebResourceRequested;
    }

    public IWebViewWindow Window => _window;

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