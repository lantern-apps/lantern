using Lantern.Messaging;
using Lantern.Platform;
using Lantern.Windows;

namespace Lantern.Services;

internal class TrayService : ITrayManager, ILanternService
{
    private readonly Dictionary<string, ITray> _trays = new();

    private readonly LanternOptions _options;
    private readonly IAppLifetime _lifetime;

    private readonly IWindowingPlatform _windowingPlatform;
    private readonly IEventEmitter _eventEmitter;
    private readonly IWindowManager _windowManager;

    public TrayService(
        LanternOptions options,
        IWindowManager windowManager,
        IAppLifetime lifetime,
        IEventEmitter eventEmitter,
        IWindowingPlatform windowingPlatform)
    {
        _options = options;
        _lifetime = lifetime;
        _windowManager = windowManager;
        _eventEmitter = eventEmitter;
        _windowingPlatform = windowingPlatform;
    }

    public ITray CreateTray(TrayOptions options)
    {
        if (_trays.ContainsKey(options.Name))
            throw new Exception();

        Tray tray = new(options.Name, _windowingPlatform.CreateTrayIcon())
        {
            IconPath = options.IconPath,
            ToolTip = options.ToolTip,
            Menu = options.Menu?.Build(),
            Visible = options.Visible
        };

        tray.Click += () =>
        {
            if (options.ClickCommand == null)
            {
                _eventEmitter.Emit(KnownMessageNames.TrayOnClick);
            }
            else
            {
                HandleCommand(options.ClickCommand);
            }
        };

        if (tray.Menu != null)
        {
            tray.Menu.ItemClick += item =>
            {
                string? command = (string?)item.State;
                if (command == null)
                {
                    _eventEmitter.Emit(KnownMessageNames.TrayOnMenuItemClick, item.Id);
                }
                else
                {
                    HandleCommand(command);
                }
            };
        }

        _trays.Add(options.Name, tray);

        return tray;
    }

    private void HandleCommand(string command)
    {
        switch (command)
        {
            case Commands.ActivateMainWindow:
                _windowManager.GetDefaultWindow()?.Activate();
                break;
            case Commands.Shutdown:
                _lifetime.StopApplication();
                break;
        }
    }

    public ITray[] GetAllTarys() => _trays.Values.ToArray();

    public ITray? GetDefaultTray() => _trays.Values.FirstOrDefault();

    public ITray? GetTray(string name)
    {
        _trays.TryGetValue(name, out var tray);
        return tray;
    }

    public void Initialize()
    {
        _lifetime.ApplicationStopping.Register(() =>
        {
            foreach (var tray in _trays.Values)
            {
                tray.Visible = false;
            }
        });

        foreach (var options in _options.Trays)
        {
            CreateTray(options);
        }
    }
}
