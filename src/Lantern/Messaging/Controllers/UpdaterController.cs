using Lantern.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Lantern.Aus;

namespace Lantern.Controllers;

public class UpdaterController : UpdaterControllerBase
{
    //这里需要判断是否有注入Aus, 所以不能直接从构造函数注入IAusUpdateManager
    private readonly IServiceProvider _serviceProvider;
    private readonly IAppLifetime _lifetime;

    public UpdaterController(IServiceProvider serviceProvider, IAppLifetime lifetime)
    {
        _serviceProvider = serviceProvider;
        _lifetime = lifetime;
    }

    public override async Task<AppUpdateInfo?> Check(IpcContext context)
    {
        var updater = _serviceProvider.GetService<IAusUpdateManager>();
        if (updater == null)
            return null;

        var result = await updater.CheckForUpdateAsync(_lifetime.ApplicationStopping);

        return new AppUpdateInfo
        {
            Version = result.Patch.Version,
            Size = result.Patch.Files.Sum(x => x.Size)
        };
    }

    public override async Task Perform(IpcContext context)
    {
        var updater = _serviceProvider.GetService<IAusUpdateManager>();
        if (updater == null)
            return;

        await updater.CheckPerformUpdateAsync(_lifetime.ApplicationStopping);
    }

    public override void Launch(IpcContext context)
    {
        _serviceProvider.GetService<IAusUpdateManager>()?.LaunchUpdater(new LaunchUpdaterOptions
        {
            Launched = () => _lifetime.StopApplication()
        });
    }

}
