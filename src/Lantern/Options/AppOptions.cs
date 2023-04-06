namespace Lantern;

public class AppOptions
{
    public string AppName { get; set; } = "Lantern";
    public string? DefaultIconPath { get; set; }
    public bool IsSingleInstance { get; set; } = true;
    public AppShutdownCondition ShutdownCondition { get; set; } = AppShutdownCondition.WhenAllVisibleWindowClosed;
    public Version? Version { get; set; }
    public string UserDataFolder { get; set; } = null!;
}
