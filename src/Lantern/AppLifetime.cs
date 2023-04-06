namespace Lantern;

public class AppLifetime : IAppLifetime
{
    private readonly CancellationTokenSource _startedSource = new();
    private readonly CancellationTokenSource _stoppedSource = new();
    private readonly CancellationTokenSource _stoppingSource = new();

    public CancellationToken ApplicationStarted => _startedSource.Token;
    public CancellationToken ApplicationStopped => _stoppedSource.Token;
    public CancellationToken ApplicationStopping => _stoppingSource.Token;

    internal void NotifyStarted() => ExecuteHandlers(_startedSource);
    internal void NotifyStopped() => ExecuteHandlers(_stoppedSource);
    public void StopApplication() => ExecuteHandlers(_stoppingSource);

    private static void ExecuteHandlers(CancellationTokenSource cancel)
    {
        // Noop if this is already cancelled
        if (cancel.IsCancellationRequested)
        {
            return;
        }

        // Run the cancellation token callbacks
        cancel.Cancel(throwOnFirstException: false);
    }

}