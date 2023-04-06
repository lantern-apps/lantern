using Calls;
using System.Text.Json.Serialization;

namespace Lantern.Messaging;

public abstract class EventControllerBase
{
    [Callable(KnownMessageNames.EventListen)] public abstract void Listen(IpcContext<EventListenOptions> context);
    [Callable(KnownMessageNames.EventUnlisten)] public abstract void Unlisten(IpcContext<EventUnlistenOptions> context);
}

public sealed class EventListenOptions
{
    [JsonPropertyName("event")]
    public string Event { get; set; } = null!;

    [JsonPropertyName("once")]
    public bool Once { get; set; }
}

public sealed class EventUnlistenOptions
{
    [JsonPropertyName("event")]
    public string? Event { get; set; }
}
