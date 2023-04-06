using Lantern.Threading;

namespace Lantern.Platform;

public interface IPlatformThreadingInterface
{
    Thread UIThread { get; }
    void Initialize();

    void RunLoop(CancellationToken cancellationToken);

    void Signal(DispatcherPriority priority);

    bool CurrentThreadIsLoopThread { get; }

    event Action<DispatcherPriority?> Signaled;
}
