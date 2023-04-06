using Lantern.Messaging;
using Lantern.Platform;

namespace Lantern.Controllers;

public class NotificationController : NotificationControllerBase
{
    private readonly INotifyingPlatform _notifyingPlatform;

    public NotificationController(INotifyingPlatform notifyingPlatform)
    {
        _notifyingPlatform = notifyingPlatform;
    }

    public override void Send(IpcContext<SendNotificationOptions> context)
    {
        _notifyingPlatform.ShowNotification(context.Body.Title!, context.Body.Body);
    }
}
