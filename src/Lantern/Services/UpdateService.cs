using Lantern.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Lantern.Aus;

namespace Lantern.Services;

internal class UpdateService : ILanternService
{
    private readonly UpdaterOptions _options;
    private readonly ILogger<UpdateService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IAppLifetime _lifetime;
    private readonly IEventEmitter _eventEmitter;
    private bool _disabled;
    private Timer? _timer;

    public UpdateService(
        LanternOptions options,
        IServiceProvider serviceProvider,
        IAppLifetime lifetime,
        IEventEmitter eventEmitter,
        ILogger<UpdateService> logger)
    {
        _options = options.Updater;
        _lifetime = lifetime;
        _serviceProvider = serviceProvider;
        _eventEmitter = eventEmitter;
        _logger = logger;
    }

    public void Initialize()
    {
        if(_options.ServerAddress == null)
        {
            return;
        }

        CheckSetupUpdate(true);

        if (_disabled)
        {
            return;
        }

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
        using var scope = _serviceProvider.CreateScope();

        try
        {
            using var manager = scope.ServiceProvider.GetService<IAusUpdateManager>();
            if(manager == null)
            {
                _disabled = true;
                return;
            }

            var result = await manager.CheckForUpdateAsync(stoppingToken);
            if (result.CanUpdate && !result.IsPrepared)
            {
                _logger.LogInformation($"Checked update {result.Patch.Version}");

                OnUpdateCheckedAsync(result.Patch);

                await manager.PrepareUpdateAsync(result, stoppingToken);

                _logger.LogInformation($"Prepared update {result.Patch.Version}");

                OnUpdatePreparedAsync(result.Patch);
            }
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
        using var scope = _serviceProvider.CreateScope();
        try
        {
            using var manager = scope.ServiceProvider.GetService<IAusUpdateManager>();
            if (manager == null)
            {
                return;
            }

            if (manager.HasUpdatePrepared(out var version))
            {
                _logger.LogInformation($"launching update {version}");

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

    private void OnUpdateLaunched() => _lifetime.StopApplication();
}
