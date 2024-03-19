using Lantern.Platform;
using Lantern.Threading;
using Lantern.Windows;
using Microsoft.Extensions.Logging;

namespace Lantern.AsService;

public sealed class WebBrowserManager(
    WebViewEnvironmentOptions environmentOptions,
    ILoggerFactory loggerFactory,
    IWindowingPlatform windowingPlatform) : IWebBrowserManager, IDisposable
{
    private readonly List<WebBrowserWindow> _windows = [];

    public WebViewBrowser? Get(string name)
    {
        return _windows.FirstOrDefault(x => x.Name == name)?.Browser;
    }

    public IReadOnlyList<WebViewBrowser> GetAll() => _windows.Select(x => x.Browser).ToList().AsReadOnly();

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

    private WebBrowserWindow CreateWindowCore(WebViewWindowOptions windowOptions)
    {
        windowOptions.Name ??= $"Lantern-Window-{Guid.NewGuid():N}";

        var logger = loggerFactory.CreateLogger<WebBrowserWindow>();
        var window = new WebBrowserWindow(
            environmentOptions,
            windowOptions,
            windowingPlatform.CreateWindow(),
            logger);

        window.Closed += () => _windows.Remove(window);
        _windows.Add(window);
        return window;
    }

    public void Dispose()
    {
        foreach (var window in _windows)
        {
            window.Close();
        }
        _windows.Clear();
    }
}