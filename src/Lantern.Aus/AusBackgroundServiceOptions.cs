namespace Lantern.Aus;

public class AusBackgroundServiceOptions : AusUpdateOptions
{
    public bool PerformUpdateOnStart { get; set; } = true;
    public bool PerformUpdateOnExit { get; set; } = true;

    public TimeSpan CheckInterval { get; set; } = TimeSpan.FromHours(1);
}
