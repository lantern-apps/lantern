using Lantern.Messaging;

namespace Lantern.Controllers;

internal sealed class EventController : EventControllerBase
{
    public override void Listen(IpcContext<EventListenOptions> context)
    {
        context.Listen(context.Body.Event, context.Body.Once);
    }

    public override void Unlisten(IpcContext<EventUnlistenOptions> context)
    {
        if (context.Body.Event != null)
            context.Unlisten(context.Body.Event);
    }
}
