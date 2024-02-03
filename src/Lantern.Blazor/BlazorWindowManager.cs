using Lantern.Platform;
using Lantern.Threading;
using Lantern.Windows;

namespace Lantern.Blazor;

public class BlazorWindowManager : IWindowManager, IDisposable
{
    private readonly List<BlazorWebViewWindow> _windows = new();
    private readonly IWindowingPlatform _windowingPlatform;
    private readonly WebViewEnvironmentOptions _environmentOptions;
    private readonly IServiceProvider _serviceProvider;

    public BlazorWindowManager(
        WebViewEnvironmentOptions environmentOptions,
        IWindowingPlatform windowingPlatform,
        IServiceProvider serviceProvider)
    {
        _windowingPlatform = windowingPlatform;
        _environmentOptions = environmentOptions;
        _serviceProvider = serviceProvider;
    }

    public BlazorWebViewWindow? Get(string name)
    {
        return _windows.FirstOrDefault(x => x.Name == name);
    }

    public IReadOnlyList<BlazorWebViewWindow> GetAll() => _windows.ToList().AsReadOnly();

    public async Task<BlazorWebViewWindow> CreateAsync(WebViewWindowOptions windowOptions)
    {
        BlazorWebViewWindow window;
        if (Dispatcher.UIThread.CheckAccess())
        {
            window = CreateWindowCore(windowOptions);
        }
        else
        {
            window = await Dispatcher.UIThread.InvokeAsync(() => CreateWindowCore(windowOptions));
        }

        await window.EnsureWebViewInitializedAsync();
        return window!;
    }

    protected virtual BlazorWebViewWindow CreateWindowCore(WebViewWindowOptions windowOptions)
    {

        windowOptions.Name ??= $"Lantern-Window-{Guid.NewGuid():N}";

        var window = new BlazorWebViewWindow(
            environmentOptions: _environmentOptions,
            windowOptions: windowOptions,
            windowManager: this,
            windowImpl: _windowingPlatform.CreateWindow(),
            _serviceProvider);

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

    IWebViewWindow? IWindowManager.GetWindow(string name)
    {
        return Get(name);
    }

    IWebViewWindow? IWindowManager.GetDefaultWindow()
    {
        return _windows.First();
    }

    IWebViewWindow[] IWindowManager.GetAllWindows()
    {
        return _windows.Cast<IWebViewWindow>().ToArray();
    }

    async Task<IWebViewWindow> IWindowManager.CreateWindowAsync(WebViewWindowOptions options)
    {
        return await CreateAsync(options);
    }
}