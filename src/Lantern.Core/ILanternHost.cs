namespace Lantern;

public interface ILanternHost : IDisposable
{
    IServiceProvider Services { get; }

    void Run();
    void Stop();
    Task StartAsync(CancellationToken cancellationToken = default);
    Task RunAsync(CancellationToken cancellationToken = default);
}