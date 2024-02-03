using Calls;

namespace Lantern.Messaging;

public abstract class ClipboardControllerBase
{
    [Callable(KnownMessageNames.ClipboardSetText)] 
    public abstract Task SetText(IpcContext<string> context);

    [Callable(KnownMessageNames.ClipboardGetText)]
    public abstract Task<string?> GetText(IpcContext context);
}