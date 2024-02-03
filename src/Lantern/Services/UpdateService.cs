using AutoUpdates;
using Lantern.Messaging;
using Microsoft.Extensions.Logging;

namespace Lantern.Services;

public class UpdateService : ILanternService
{
    private readonly UpdaterOptions _options;
    private readonly ILogger<UpdateService> _logger;
    private readonly IUpdateManager _updateManager;
    private readonly IAppLifetime _lifetime;
    private readonly IEventEmitter _eventEmitter;
    private Timer? _timer;

    public UpdateService(
        LanternOptions options,
        IUpdateManager updateManager,
        IAppLifetime lifetime,
        IEventEmitter eventEmitter,
        ILogger<UpdateService> logger)
    {
        _options = options.Updater;
        _lifetime = lifetime;
        _updateManager = updateManager;
        _eventEmitter = eventEmitter;
        _logger = logger;
    }

    public UpdatePatch? Patch { get; private set; }

    public void Initialize()
    {
        if (_options.ServerAddress == null)
        {
            return;
        }

        CheckSetupUpdate(true);

        _lifetime.ApplicationStarted.Register(() =>
        {
            _timer = new Timer(async state =>
            {
                await CheckPrepareUpdateAsync(_lifetime.ApplicationStopping);

                try
                {
                    await Task.Delay(_options.CheckPeriod, _lifetime.ApplicationStopping);
                }
                catch (OperationCanceledException)
                {

                }
            }, null, _options.CheckDelay, _options.CheckPeriod);
        });

        _lifetime.ApplicationStopping.Register(() =>
        {
            _timer?.Change(Timeout.Infinite, 0);
        });
    }

    private async Task CheckPrepareUpdateAsync(CancellationToken stoppingToken)
    {
        try
        {
            Patch = await _updateManager.CheckForUpdateAsync(stoppingToken);

            if (Patch.CanUpdate && !Patch.IsPrepared)
            {
                _logger.LogInformation($"Checked update {Patch.Manifest.Version}");

                OnUpdateCheckedAsync(Patch.Manifest);

                await _updateManager.PrepareUpdateAsync(Patch, stoppingToken);

                _logger.LogInformation($"Prepared update {Patch.Manifest.Version}");

                OnUpdatePreparedAsync(Patch.Manifest);
            }
        }
        catch (OperationCanceledException)
        {

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "exception on check and prepare update");
        }
    }

    private void OnUpdateCheckedAsync(AusManifest update)
    {
        _eventEmitter.Emit(KnownMessageNames.UpdaterOnChecked, new AppUpdateInfo
        {
            Version = update.Version,
            Size = update.Files.Sum(x => x.Size),
        });
    }

    private void OnUpdatePreparedAsync(AusManifest update)
    {
        _eventEmitter.Emit(KnownMessageNames.UpdaterOnPrepared, new AppUpdateInfo
        {
            Version = update.Version,
            Size = update.Files.Sum(x => x.Size),
        });
    }

    private void CheckSetupUpdate(bool restart)
    {
        try
        {
            if (_updateManager.HasUpdatePrepared(out var version))
            {
                _logger.LogInformation($"launching update {version}");

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

    private void OnUpdateLaunched() => _lifetime.StopApplication();
}
