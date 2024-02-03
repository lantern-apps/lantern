using AutoUpdates;
using Lantern.Messaging;

namespace Lantern.Controllers;

public class UpdaterController : UpdaterControllerBase
{
    //这里需要判断是否有注入Aus, 所以不能直接从构造函数注入IAusUpdateManager
    private readonly IUpdateManager _updateManager;
    private readonly IAppLifetime _lifetime;

    public UpdaterController(IUpdateManager updateManager, IAppLifetime lifetime)
    {
        _updateManager = updateManager;
        _lifetime = lifetime;
    }

    public override async Task<AppUpdateInfo?> Check(IpcContext context)
    {
        var patch = await _updateManager.CheckForUpdateAsync(_lifetime.ApplicationStopping);

        return new AppUpdateInfo
        {
            Version = patch.Manifest.Version,
            Size = patch.UpdateFiles.Sum(x => x.Size)
        };
    }

    public override async Task Perform(IpcContext context)
    {
        await _updateManager.CheckPerformUpdateAsync(_lifetime.ApplicationStopping);
    }

    public override void Launch(IpcContext context)
    {
        _updateManager.LaunchUpdater(null, new LaunchUpdaterOptions
        {
            Launched = () => _lifetime.StopApplication()
        });
    }

}
