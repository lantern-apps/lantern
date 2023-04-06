using Lantern.Platform;

namespace Lantern.Windows;

public class Tray : ITray
{
    private readonly ITrayIconImpl _trayIcon;
    private Menu? _menu;

    public Tray(string name, ITrayIconImpl trayIcon)
    {
        Name = name;
        _trayIcon = trayIcon;
        _trayIcon.Click = () => Click?.Invoke();
    }

    public string Name { get; }

    public string? IconPath
    {
        get => _trayIcon.IconPath;
        set => _trayIcon.IconPath = value;
    }
    public string? ToolTip
    {
        get => _trayIcon.ToolTip;
        set => _trayIcon.ToolTip = value;
    }
    public bool Visible
    {
        get => _trayIcon.IsVisible;
        set => _trayIcon.IsVisible = value;
    }

    public Menu? Menu
    {
        get => _menu;
        set
        {
            _menu = value;
            _trayIcon.Menu = value;
        }
    }

    public event Action? Click;
}