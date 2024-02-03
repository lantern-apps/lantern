namespace Lantern.Windows;

public class TrayOptions
{
    public string Name { get; set; } = null!;
    public string? IconPath { get; set; }
    public string? ToolTip { get; set; }
    public bool Visible { get; set; } = true;
    public MenuOptions? Menu { get; set; }

    public string? ClickCommand { get; set; }
}

