namespace Lantern.Aus;

public class LaunchUpdaterOptions
{
    public bool Restart { get; set; } = true;
    public bool Exit { get; set; } = true;
    public string? Arguments { get; set; }
    public Action? Launched { get; set; }
    public bool ForceUpdate { get; set; } = true;

}