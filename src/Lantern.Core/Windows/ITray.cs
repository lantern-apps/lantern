namespace Lantern.Windows;

public interface ITray
{
    event Action? Click;

    string Name { get; }
    string? IconPath { get; set; }
    string? ToolTip { get; set; }
    bool Visible { get; set; }
    Menu? Menu { get; set; }
}