using Lantern.Platform;
using Lantern.Threading;
using Lantern.Windows;
using Microsoft.Extensions.Logging;

namespace Lantern.AsService;

public class WebBrowserManager : IWebBrowserManager
{
    private readonly List<WebBrowserWindow> _windows = new();
    private readonly ILoggerFactory _loggerFactory;
    private readonly IWindowingPlatform _windowingPlatform;
    private readonly WebViewEnvironmentOptions _environmentOptions;
    private readonly IServiceProvider _serviceProvider;

    public WebBrowserManager(
        WebViewEnvironmentOptions environmentOptions,
        ILoggerFactory loggerFactory,
        IWindowingPlatform windowingPlatform,
        IServiceProvider serviceProvider)
    {
        _loggerFactory = loggerFactory;
        _windowingPlatform = windowingPlatform;
        _environmentOptions = environmentOptions;
        _serviceProvider = serviceProvider;
    }

    public async Task<WebViewBrowser> CreateAsync(WebViewWindowOptions windowOptions)
    {
        WebBrowserWindow window;
        if (Dispatcher.UIThread.CheckAccess())
        {
            window = CreateWindowCore(windowOptions);
        }
        else
        {
            window = await Dispatcher.UIThread.InvokeAsync(() => CreateWindowCore(windowOptions));
        }

        await window.EnsureWebViewInitializedAsync();
        return window.Browser!;
    }

    protected virtual WebBrowserWindow CreateWindowCore(WebViewWindowOptions windowOptions)
    {
        windowOptions.Name ??= $"Lantern-Window-{Guid.NewGuid():N}";

        var logger = _loggerFactory.CreateLogger<WebBrowserWindow>();
        var window = new WebBrowserWindow(
            _environmentOptions,
            windowOptions,
            _serviceProvider,
            _windowingPlatform.CreateWindow(),
            logger);

        window.Closed += () => _windows.Remove(window);
        _windows.Add(window);
        return window;
    }
}