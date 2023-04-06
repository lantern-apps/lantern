namespace Lantern;

public interface IAppLifetime
{
    CancellationToken ApplicationStarted { get; }
    CancellationToken ApplicationStopped { get; }
    CancellationToken ApplicationStopping { get; }
    void StopApplication();
}
