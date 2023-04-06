using Calls;
using System.Text.Json.Serialization;

namespace Lantern.Messaging;

public abstract class UpdaterControllerBase
{
    [Callable(KnownMessageNames.UpdaterLaunch)] public abstract void Launch(IpcContext context);

    [Callable(KnownMessageNames.UpdaterPerform)] public abstract Task Perform(IpcContext context);

    [Callable(KnownMessageNames.UpdaterCheck)]public abstract Task<AppUpdateInfo?> Check(IpcContext context);
}

public class AppUpdateInfo
{
    [JsonPropertyName("version")]
    public Version? Version { get; set; }

    [JsonPropertyName("size")]
    public long Size { get; set; }
}