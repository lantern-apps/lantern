using Lantern.Platform;

namespace Lantern.Threading;

public class Dispatcher : IDispatcher
{
    private static Dispatcher _dispatcher = null!;

    public static void InitializeUIThread(IPlatformThreadingInterface platform)
    {
        DispatcherSynchronizationContext.Install();
        platform.Initialize();
        _dispatcher = new Dispatcher(platform);
    }

    public static Dispatcher UIThread => _dispatcher;

    private readonly JobRunner _jobRunner;
    private readonly IPlatformThreadingInterface _platform;

    public Dispatcher(IPlatformThreadingInterface platform)
    {
        _platform = platform;
        _jobRunner = new JobRunner(platform);
        _platform.Signaled += _jobRunner.RunJobs;
    }

    public bool CheckAccess() => _platform?.CurrentThreadIsLoopThread ?? true;

    public void VerifyAccess()
    {
        if (!CheckAccess())
            throw new InvalidOperationException("Call from invalid thread");
    }

    public void MainLoop(CancellationToken cancellationToken)
    {
        cancellationToken.Register(() => _platform!.Signal(DispatcherPriority.Send));
        _platform!.RunLoop(cancellationToken);
    }

    public void RunJobs()
    {
        _jobRunner.RunJobs(null);
    }

    public void RunJobs(DispatcherPriority minimumPriority) => _jobRunner.RunJobs(minimumPriority);

    public bool HasJobsWithPriority(DispatcherPriority minimumPriority) => _jobRunner.HasJobsWithPriority(minimumPriority);

    public Task InvokeAsync(Action action, DispatcherPriority priority = default)
    {
        _ = action ?? throw new ArgumentNullException(nameof(action));
        return _jobRunner.InvokeAsync(action, priority);
    }

    public Task<TResult> InvokeAsync<TResult>(Func<TResult> function, DispatcherPriority priority = default)
    {
        _ = function ?? throw new ArgumentNullException(nameof(function));
        return _jobRunner.InvokeAsync(function, priority);
    }

    public Task InvokeAsync(Func<Task> function, DispatcherPriority priority = default)
    {
        _ = function ?? throw new ArgumentNullException(nameof(function));
        return _jobRunner.InvokeAsync(function, priority).Unwrap();
    }

    public Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> function, DispatcherPriority priority = default)
    {
        _ = function ?? throw new ArgumentNullException(nameof(function));
        return _jobRunner.InvokeAsync(function, priority).Unwrap();
    }

    public void Post(Action action, DispatcherPriority priority = default)
    {
        _ = action ?? throw new ArgumentNullException(nameof(action));
        _jobRunner.Post(action, priority);
    }

    public void Post(SendOrPostCallback action, object? arg, DispatcherPriority priority = default)
    {
        _ = action ?? throw new ArgumentNullException(nameof(action));
        _jobRunner.Post(action, arg, priority);
    }

    internal void EnsurePriority(DispatcherPriority currentPriority)
    {
        if (currentPriority == DispatcherPriority.MaxValue)
            return;
        currentPriority += 1;
        _jobRunner.RunJobs(currentPriority);
    }
}
