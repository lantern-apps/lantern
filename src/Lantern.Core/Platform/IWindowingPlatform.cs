namespace Lantern.Platform;

public interface IWindowingPlatform
{
    IScreenImpl Screen { get; }
    IWindowImpl CreateWindow();
    ITrayIconImpl CreateTrayIcon(string? icon = null, string? tooltip = null);
}
