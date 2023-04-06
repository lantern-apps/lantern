using Lantern.Messaging;

namespace Lantern.Controllers;

public sealed class AppController : AppControllerBase
{
    private readonly IAppLifetime _lifetime;
    private readonly LanternOptions _options;

    public AppController(
        LanternOptions lanternOptions,
        IAppLifetime lifetime)
    {
        _options = lanternOptions;
        _lifetime = lifetime;
    }

    public override Version? GetVersion(IpcContext _) => _options.Version;
    public override void Shutdown(IpcContext _) => _lifetime.StopApplication();
}
