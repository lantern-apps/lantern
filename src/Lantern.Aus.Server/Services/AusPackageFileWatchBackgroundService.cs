using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;

namespace Lantern.Aus.Server.Services;

public partial class AusPackageFileWatchBackgroundService : BackgroundService
{
    private class FileChange
    {
        public required string FullPath;
        public required WatcherChangeTypes ChangeType;
    }

    private readonly AusPackageFileWatchOptions _options;
    private readonly FileSystemWatcher _watcher;
    private readonly IAusManifestCache _manifestStore;
    private readonly ILogger _logger;

    private readonly Channel<FileChange> _channel = Channel.CreateUnbounded<FileChange>(new UnboundedChannelOptions
    {
        SingleReader = true,
        SingleWriter = true,
    });

    public AusPackageFileWatchBackgroundService(
        AusPackageFileWatchOptions options,
        IAusManifestCache manifestStore,
        ILogger<AusPackageFileWatchBackgroundService> logger)
    {
        _options = options;
        _manifestStore = manifestStore;
        _logger = logger;

        _watcher = new FileSystemWatcher(_options.PackagesDirectory);

        _watcher.Created += async (s, e) => await _channel.Writer.WriteAsync(new FileChange { ChangeType = WatcherChangeTypes.Created, FullPath = e.FullPath });
        _watcher.Deleted += async (s, e) => await _channel.Writer.WriteAsync(new FileChange { ChangeType = WatcherChangeTypes.Deleted, FullPath = e.FullPath });
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.Register(() => _watcher.EnableRaisingEvents = false);
        await LoadAsync();

        _watcher.EnableRaisingEvents = true;

        try
        {
            await foreach (var change in _channel.Reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    if (change.ChangeType == WatcherChangeTypes.Created)
                    {
                        await FileCreated(change.FullPath);
                    }
                    else if (change.ChangeType == WatcherChangeTypes.Deleted)
                    {
                        FileDeleted(change.FullPath);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "在处理文件变更时发生异常");
                }
            }
        }
        catch (OperationCanceledException)
        {
            return;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "在监听文件系统时发生致命错误");
        }
        finally
        {
            _watcher.EnableRaisingEvents = false;
        }
    }

    private async Task LoadAsync()
    {
        var files = Directory.GetFiles(_options.PackagesDirectory);
        foreach (var file in files)
        {
            var extension = Path.GetExtension(file);
            if (extension == ".zip")
            {
                await FileCreated(file);
            }
            else if (extension == ".manifest")
            {
                var manifest = AusManifest.LoadFromFile(file);
                if (manifest.Name != null && manifest.Version != null)
                    _manifestStore.Create(manifest);
            }
        }
    }

    private async Task FileCreated(string path)
    {
        _logger.LogInformation($"extracting {path}");

        var filename = Path.GetFileName(path);

        Group group = CreateRegex().Match(filename).Groups[1];
        if (!Version.TryParse(group.Value, out var version))
            return;

        var name = filename[..(group.Index - 1)];
        if (string.IsNullOrWhiteSpace(name))
            return;

        var dest = Path.Combine(_options.PackagesDirectory, name, version.ToString());
        Directory.CreateDirectory(dest);
        ZipFile.ExtractToDirectory(path, dest, Encoding.GetEncoding("GB2312"), true);

        var files = await AusManifest.LoadAsync(dest, Array.Empty<string>());
        AusManifest manifest = new()
        {
            Name = name,
            Version = version,
            Files = files
        };

        manifest.SaveAs(Path.Combine(_options.PackagesDirectory, Path.GetFileNameWithoutExtension(filename) + ".manifest"));
        _manifestStore.Create(manifest);

        File.Delete(path);

        _logger.LogInformation($"extracted {path}");
    }

    private void FileDeleted(string path)
    {
        var filename = Path.GetFileName(path);
        var extension = Path.GetExtension(filename);
        if (extension != ".manifest")
            return;

        Group group = CreateRegex().Match(Path.ChangeExtension(filename, ".zip")).Groups[1];
        if (!Version.TryParse(group.Value, out var version))
            return;

        var name = filename[..(group.Index - 1)];
        if (string.IsNullOrWhiteSpace(name))
            return;

        _logger.LogInformation($"deleting {name} {version}");

        var dest = Path.Combine(_options.PackagesDirectory, name, version.ToString());
        if (Directory.Exists(dest))
        {
            try
            {
                Directory.Delete(dest, true);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"directory delete fail {name} {version}, {ex.Message}");
            }
        }

        _manifestStore.Delete(name, version);

        _logger.LogInformation($"deleted {name} {version}");
    }


    [GeneratedRegex("\\.(\\d+\\.\\d+(?:\\.\\d+)?(?:\\.\\d+)?)\\.zip$")]
    private static partial Regex CreateRegex();
}