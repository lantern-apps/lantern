using Calls;
using System.Text.Json.Serialization;

namespace Lantern.Messaging;

public abstract class NotificationControllerBase
{
    [Callable(KnownMessageNames.NotificationSend)] 
    public abstract void Send(IpcContext<SendNotificationOptions> context);
}

public sealed class SendNotificationOptions
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("body")]
    public string? Body { get; set; }
}
