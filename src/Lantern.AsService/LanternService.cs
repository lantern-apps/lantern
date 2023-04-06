using Lantern.Platform;
using Lantern.Threading;

namespace Lantern.AsService;

public class LanternService : ILanternHost
{
    private readonly Thread _thread;
    private readonly IPlatformThreadingInterface _threadingPlatform;
    private readonly CancellationTokenSource _stoppedSource = new();
    private readonly CancellationTokenSource _startedSource = new();
    private readonly CancellationTokenSource _stoppingSource = new();

    public LanternService(
        IServiceProvider serviceProvider,
        IPlatformThreadingInterface threadingPlatform)
    {
        Services = serviceProvider;

        _threadingPlatform = threadingPlatform;
        _thread = new Thread(RunCore)
        {
            IsBackground = true,
            Name = "Lantern Service backgournd thread"
        };
        if (OperatingSystem.IsWindows())
            _thread.SetApartmentState(ApartmentState.STA);
    }

    public CancellationToken ApplicationStarted => _startedSource.Token;
    public CancellationToken ApplicationStopping => _stoppingSource.Token;
    public CancellationToken ApplicationStopped => _stoppedSource.Token;

    public IServiceProvider Services { get; }

    public void Start() => _thread.Start();

    public void Stop() => _stoppingSource.Cancel();

    public void Run()
    {
        _thread.Start();
        _thread.Join();
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        TaskCompletionSource completion = new(TaskCreationOptions.RunContinuationsAsynchronously);

        cancellationToken.Register(() => _stoppingSource.Cancel());
        ApplicationStopped.Register(() => completion.SetResult());

        _thread.Start();

        await completion.Task.ConfigureAwait(false);
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        TaskCompletionSource completion = new(TaskCreationOptions.RunContinuationsAsynchronously);
        ApplicationStarted.Register(() => completion.SetResult());

        _thread.Start();

        await completion.Task.ConfigureAwait(false);
    }

    private void RunCore(object? state)
    {
        Dispatcher.InitializeUIThread(_threadingPlatform);

        _startedSource.Cancel();

        Dispatcher.UIThread.MainLoop(_stoppingSource.Token);

        _stoppedSource.Cancel();
    }

    public void Dispose()
    {
        if (!_stoppingSource.IsCancellationRequested)
            _stoppingSource.Cancel(throwOnFirstException: false);

        try
        {
            _thread.Join(1000);
        }
        catch (ThreadStateException) { }

        if (!_stoppedSource.IsCancellationRequested)
            _stoppedSource.Cancel(throwOnFirstException: false);
    }
}