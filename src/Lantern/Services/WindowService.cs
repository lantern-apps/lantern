using Lantern.Messaging;
using Lantern.Platform;
using Lantern.Windows;

namespace Lantern.Services;

internal class WindowService : WindowManager, ILanternService
{
    private readonly LanternOptions _options;
    private readonly Ipc _ipcService;
    private readonly IAppLifetime _lifetime;

    public WindowService(
        LanternOptions options,
        WebViewEnvironmentOptions envOptions,
        Ipc ipcService,
        IAppLifetime lifetime,
        IWindowingPlatform windowingPlatform) : base(envOptions, windowingPlatform)
    {
        _options = options;
        _ipcService = ipcService;
        _lifetime = lifetime;
    }

    public override async Task<IWebViewWindow> CreateWindowAsync(WebViewWindowOptions windowOptions)
    {
        var window = await base.CreateWindowAsync(windowOptions);
        _ipcService.Emit(null, KnownMessageNames.WindowCreate, window.Name);
        return window;
    }

    public void Initialize()
    {
        foreach (var windowOptions in _options.Windows)
        {
            CreateWindow(windowOptions);
        }
    }

    protected override IWebViewWindow CreateWindow(WebViewWindowOptions windowOptions)
    {
        var window = (WebViewWindow)base.CreateWindow(windowOptions);

        window.Closing += () => _ipcService.Emit(window, KnownMessageNames.WindowOnClosing);
        window.Resized += size => _ipcService.Emit(window, KnownMessageNames.WindowOnResized, size);
        window.Moved += position => _ipcService.Emit(window, KnownMessageNames.WindowOnMoved, position);
        window.Closed += () =>
        {
            _ipcService.Emit(window, KnownMessageNames.WindowOnClosed);
            _ipcService.Unlisten(window);

            if (_options.ShutdownCondition != AppShutdownCondition.Manual)
            {
                if (_options.ShutdownCondition == AppShutdownCondition.WhenAllWindowClosed && _windows.Count == 0)
                {
                    _lifetime.StopApplication();
                }
                else if (!_windows.Any(x => x.IsVisible))
                {
                    _lifetime.StopApplication();
                }
            }
        };

        window.StateChanged += state =>
        {
            var eventName = state switch
            {
                WindowState.Normal => KnownMessageNames.WindowOnRestore,
                WindowState.Minimized => KnownMessageNames.WindowOnMinimized,
                WindowState.Maximized => KnownMessageNames.WindowOnMaximized,
                WindowState.FullScreen => KnownMessageNames.WindowOnFullScreen,
                _ => null
            };

            _ipcService.Emit(window, eventName!);
        };

        window.WebViewMessageReceived += message => _ipcService.Post(window, message);

        return window;
    }

}
