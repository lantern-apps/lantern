using Calls;

namespace Lantern.Messaging;

public abstract class AppControllerBase
{
    [Callable(KnownMessageNames.AppShutdown)] 
    public abstract void Shutdown(IpcContext context);

    [Callable(KnownMessageNames.AppGetVersion)]
    public abstract Version? GetVersion(IpcContext context);
}