using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lantern.Aus;

public class AusBackgroundService : BackgroundService
{
    protected readonly ILogger _logger;
    protected readonly AusBackgroundServiceOptions _options;
    protected readonly IServiceProvider _serviceProvider;

    public AusBackgroundService(
        AusBackgroundServiceOptions options,
        IServiceProvider serviceProvider,
        ILogger<AusBackgroundService> logger)
    {
        _options = options;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_options.PerformUpdateOnStart)
        {
            await CheckSetupUpdate(true);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckPrepareUpdateAsync(stoppingToken);

            try
            {
                await Task.Delay(_options.CheckInterval, stoppingToken);
            }
            catch (TimeoutException)
            {
                break;
            }
        }

        if (_options.PerformUpdateOnExit)
        {
            await CheckSetupUpdate(false);
        }
    }

    private async Task CheckPrepareUpdateAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();

        try
        {
            using var manager = scope.ServiceProvider.GetRequiredService<IAusUpdateManager>();
            var result = await manager.CheckForUpdateAsync(stoppingToken);
            if (result.CanUpdate && !result.IsPrepared)
            {
                _logger.LogInformation($"Checked update {result.Patch.Version}");

                await OnUpdateCheckedAsync(result.Patch);

                await manager.PrepareUpdateAsync(result, stoppingToken);

                _logger.LogInformation($"Prepared update {result.Patch.Version}");

                await OnUpdatePreparedAsync(result.Patch);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "exception on check and prepare update");
        }
    }

    private async Task CheckSetupUpdate(bool restart)
    {
        using var scope = _serviceProvider.CreateScope();
        try
        {
            using var manager = scope.ServiceProvider.GetRequiredService<IAusUpdateManager>();
            if (manager.HasUpdatePrepared(out var version))
            {
                _logger.LogInformation($"launching update {version}");

                await OnUpdateLaunchingAsync(AusUpdateManager.Patch!);

                manager.LaunchUpdater(version, new LaunchUpdaterOptions
                {
                    Restart = restart,
                    Launched = OnUpdateLaunched,
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "在检查更像时发生异常");
        }
    }

    /// <summary>
    /// On checked update
    /// </summary>
    /// <param name="update">Update patch</param>
    /// <returns></returns>
    protected virtual Task OnUpdateCheckedAsync(AusManifest update) => Task.CompletedTask;

    /// <summary>
    /// On prepared update files
    /// </summary>
    /// <param name="update">Update patch</param>
    /// <returns></returns>
    protected virtual Task OnUpdatePreparedAsync(AusManifest update) => Task.CompletedTask;

    /// <summary>
    /// On launching update program
    /// </summary>
    /// <param name="update">Update patch</param>
    /// <returns></returns>
    protected virtual Task OnUpdateLaunchingAsync(AusManifest update) => Task.CompletedTask;

    /// <summary>
    /// On update program launched
    /// </summary>
    /// <returns></returns>
    protected virtual void OnUpdateLaunched() { }
}
