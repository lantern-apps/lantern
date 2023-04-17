using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lantern.Aus;

public class AusBackgroundService : BackgroundService
{
    protected readonly ILogger _logger;
    protected readonly AusBackgroundServiceOptions _options;
    protected readonly IAusUpdateManager _updateManager;

    public AusBackgroundService(
        AusBackgroundServiceOptions options,
        IAusUpdateManager updateManager,
        ILogger<AusBackgroundService> logger)
    {
        _options = options;
        _updateManager = updateManager;
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
        try
        {
            var patch = await _updateManager.CheckForUpdateAsync(stoppingToken);

            if (patch.CanUpdate && !patch.IsPrepared)
            {
                _logger.LogInformation($"Checked update {patch.Manifest.Version}");

                await OnUpdateCheckedAsync(patch.Manifest);

                await _updateManager.PrepareUpdateAsync(patch, stoppingToken);

                _logger.LogInformation($"Prepared update {patch.Manifest.Version}");

                await OnUpdatePreparedAsync(patch.Manifest);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "exception on check and prepare update");
        }
    }

    private async Task CheckSetupUpdate(bool restart)
    {
        try
        {
            if (_updateManager.HasUpdatePrepared(out var version))
            {
                _logger.LogInformation($"Launching update {version}");

                _updateManager.LaunchUpdater(version, new LaunchUpdaterOptions
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
    /// On update program launched
    /// </summary>
    /// <returns></returns>
    protected virtual void OnUpdateLaunched() { }
}
