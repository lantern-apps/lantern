using Lantern.Windows;

namespace Lantern.Platform;

public interface ITrayIconImpl
{
    string? ToolTip { get; set; }
    string? IconPath { get; set; }
    bool IsVisible { get; set; }
    Action? Click { get; set; }
    Action? BallonClick { get; set; }
    Menu? Menu { get; set; }
}
