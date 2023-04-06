using Lantern.Platform;
using Lantern.Threading;
using System.ComponentModel.DataAnnotations;

namespace Lantern.Windows;

public class WindowManager : IWindowManager
{
    protected readonly List<WebViewWindow> _windows = new();

    private readonly WebViewEnvironmentOptions _envOptions;
    private readonly IServiceProvider _services;
    protected readonly IWindowingPlatform _windowingPlatform;

    public WindowManager(
        WebViewEnvironmentOptions envOptions,
        IServiceProvider services,
        IWindowingPlatform windowingPlatform)
    {
        _envOptions = envOptions;
        _services = services;
        _windowingPlatform = windowingPlatform;
    }

    public IWebViewWindow? GetWindow(string name) => _windows.FirstOrDefault(x => x.Name == name);
    public IWebViewWindow? GetDefaultWindow() => _windows.FirstOrDefault();

    public IWebViewWindow[] GetAllWindows() => _windows.ToArray();

    public virtual async Task<IWebViewWindow> CreateWindowAsync(WebViewWindowOptions options)
    {
        ValidationHelper.Validate(options);

        if (_windows.Any(x => x.Name == options.Name))
        {
            throw new ArgumentException($"Window name '{options.Name}' was already existed");
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            return CreateWindow(options);
        }
        else
        {
            return await Dispatcher.UIThread.InvokeAsync(() => CreateWindow(options));
        }
    }

    protected virtual IWebViewWindow CreateWindow(WebViewWindowOptions windowOptions)
    {
        var window = new WebViewWindow(
            environmentOptions: _envOptions,
            windowOptions: windowOptions,
            windowManager: this,
            serviceProvider: _services,
            windowImpl: _windowingPlatform.CreateWindow());

        _windows.Add(window);
        window.Closed += () => _windows.Remove(window);
        return window;
    }
}
