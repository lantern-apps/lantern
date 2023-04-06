using Calls;
using System.Text.Json.Serialization;

namespace Lantern.Messaging;

public abstract class ShellControllerBase
{
    [Callable(KnownMessageNames.ShellOpen)]
    public abstract void Open(IpcContext<ShellOpenOptions> context);
}

public sealed class ShellOpenOptions
{
    [JsonPropertyName("path")]
    public required string Path { get; set; }
}