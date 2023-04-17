using Lantern.Aus.Internal;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace AutoUpdates;

/// <inheritdoc/>
public class UpdateManager : IUpdateManager, IDisposable
{
    private const string UpdaterResourceName = "AutoUpdates.Updater.exe";
    private const string ManifestName = ".manifest";
    private const string FileExtensions = ".deploy";

    private static readonly string _updateDirectory = AppDomain.CurrentDomain.BaseDirectory;
    private static readonly string _manifestFilePath = Path.Combine(_updateDirectory, ManifestName);

    private readonly string _storageDirPath;
    private readonly string _lockFilePath;
    private readonly string _updaterFilePath;
    private readonly string[] _exclusiveFiles;
    private readonly string _applicationDir;

    private readonly Version _version;
    private readonly string _appName;
    private readonly string _entryFilePath;
    private readonly string _updateUrl;
    private bool _updateUrlValid;

    private LockFile? _lockFile;
    private bool _isDisposed;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public UpdateManager(UpdateOptions options)
    {
        Assembly assembly = Assembly.GetEntryAssembly()!;
        _appName = options.AppName ?? assembly.GetName().Name!;
        _version = options.Version ?? assembly.GetName().Version ?? new Version(0, 0, 0);
        _entryFilePath = options.EntryFilePath ?? assembly.Location!;
        _applicationDir = Path.GetDirectoryName(_entryFilePath)!;

        if (options.Exclusives == null)
        {
            _exclusiveFiles = new string[] { ManifestName };
        }
        else
        {
            _exclusiveFiles = new string[options.Exclusives.Count + 1];
            _exclusiveFiles[0] = ManifestName;
            options.Exclusives.CopyTo(_exclusiveFiles, 1);
        }

        _storageDirPath = options.TempFilePath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _appName, "Update");
        _lockFilePath = Path.Combine(_storageDirPath, $"{_appName}.lock");

        if (options.UpdaterName == null)
        {
            _updaterFilePath = Path.Combine(_storageDirPath, $"{_appName}.Updater.exe");
        }
        else
        {
            if (options.UpdaterName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                _updaterFilePath = Path.Combine(_storageDirPath, $"{options.UpdaterName}");
            else
                _updaterFilePath = Path.Combine(_storageDirPath, $"{options.UpdaterName}.exe");
        }

        _updateUrl = options.ServerAddress;
        if (!string.IsNullOrEmpty(_updateUrl) && Uri.IsWellFormedUriString(_updateUrl, UriKind.Absolute))
        {
            _updateUrlValid = true;

            if (!_updateUrl.EndsWith('/'))
            {
                _updateUrl += '/';
            }
        }

        Manifest = null!;
    }

    /// <inheritdoc/>
    public AusManifest Manifest { get; private set; }

    /// <inheritdoc/>
    public async Task EnsurePrepareManifestAsync(CancellationToken cancellationToken = default)
    {
        if (Manifest != null)
            return;

        if (File.Exists(_manifestFilePath))
        {
            try
            {
                Manifest = AusManifest.ParseFile(_manifestFilePath);
                if (Manifest?.Version != null && Manifest.Version >= _version)
                    return;
            }
            catch { }
        }

        Manifest = await AusManifest.LoadAsync(
            _appName,
            _version,
            _updateDirectory,
            _exclusiveFiles,
            cancellationToken);

        Manifest.SaveAs(_manifestFilePath);
    }

    public void EnsurePrepareManifest()
    {
        if (Manifest != null)
            return;

        if (File.Exists(_manifestFilePath))
        {
            try
            {
                Manifest = AusManifest.ParseFile(_manifestFilePath);
                if (Manifest?.Version != null && Manifest.Version >= _version)
                    return;
            }
            catch { }
        }

        Manifest = AusManifest.Load(
            _appName,
            _version,
            _updateDirectory,
            _exclusiveFiles);

        Manifest.SaveAs(_manifestFilePath);
    }

    public async Task<Version?> GetLatestVersionAsync(CancellationToken cancellationToken)
    {
        using HttpClient httpClient = CreateHttpClient();
        var appinfo = await httpClient.GetApplicationInfoAsync(cancellationToken);
        return appinfo?.GetLatestVersion()?.Version;
    }

    /// <inheritdoc/>
    public async Task<UpdatePatch> CheckForUpdateAsync(CancellationToken cancellationToken = default)
    {
        await EnsurePrepareManifestAsync(cancellationToken);

        using HttpClient httpClient = CreateHttpClient();

        var appinfo = await httpClient.GetApplicationInfoAsync(cancellationToken);

        if(appinfo == null)
        {
            return UpdatePatch.Empty;
        }

        var version = appinfo.GetLatestVersion();
        if(version == null)
        {
            return UpdatePatch.Empty;
        }

        if (version.Version == Manifest!.Version)
        {
            return UpdatePatch.Empty;
        }

        var remote = await httpClient.GetManifestAsync(version.Version, cancellationToken);

        if (remote == null)
        {
            return UpdatePatch.Empty;
        }

        var files = Manifest.CheckForUpdates(remote);

        return new UpdatePatch(remote, files, IsUpdatePrepared(remote.Version), version.MapFileExtensions);
    }

    /// <inheritdoc/>
    public async Task CheckPerformUpdateAsync(CancellationToken cancellationToken = default)
    {
        EnsureNotDisposed();
        using var _ = EnsureLockFileAcquired();

        var patch = await CheckForUpdateAsync(cancellationToken);
        if (!patch.CanUpdate)
            return;

        if (cancellationToken.IsCancellationRequested)
            return;

        await PrepareUpdateAsync(patch, cancellationToken);

        LaunchUpdater(patch.Manifest.Version);
    }

    /// <inheritdoc/>
    public async Task PrepareUpdateAsync(UpdatePatch patch, CancellationToken cancellationToken = default)
    {
        EnsureNotDisposed();

        using var _ = EnsureLockFileAcquired();

        var destDirPath = Path.Combine(_storageDirPath, patch.Manifest.Version.ToString());

        Directory.CreateDirectory(destDirPath);

        using HttpClient httpClient = CreateHttpClient();

        foreach (var file in patch.UpdateFiles)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            var destFilePath = Path.Combine(destDirPath, file.Name);

            if (File.Exists(destFilePath))
            {
                if (file.Size == new FileInfo(destFilePath).Length)
                {
                    continue;
                }
                else
                {
                    File.Delete(destFilePath);
                }
            }

            await httpClient.DownloadAsync(patch.Manifest.Version,
                                           patch.MapFileExtensions == true ? file.Name + FileExtensions : file.Name,
                                           destFilePath,
                                           null,
                                           cancellationToken);
        }

        await EnsurePrepareManifestAsync(cancellationToken);

        var manifest = Manifest.ApplyPatch(patch.Manifest);
        manifest.SaveAs(Path.Combine(destDirPath, ManifestName));

        await Assembly.GetExecutingAssembly().ExtractManifestResourceAsync(UpdaterResourceName, _updaterFilePath);
    }

    /// <inheritdoc/>
    public bool HasUpdatePrepared(out Version version)
    {
        EnsureNotDisposed();

        version = null!;

        if (!Directory.Exists(_storageDirPath))
            return false;

        foreach (var dir in Directory.GetDirectories(_storageDirPath))
        {
            var name = Path.GetFileName(dir);
            if (Version.TryParse(name, out var v))
            {
                if (version == null)
                {
                    version = v;
                }
                else if (v > version)
                {
                    version = v;
                }
            }
        }
        if (version == null || version == _version)
        {
            return false;
        }

        return IsUpdatePrepared(version);
    }

    /// <inheritdoc/>
    public bool IsUpdatePrepared(Version version)
    {
        EnsureNotDisposed();

        var baseDir = Path.Combine(_storageDirPath, version.ToString());

        var manifestFilePath = Path.Combine(baseDir, ManifestName);

        if (!File.Exists(manifestFilePath) || !File.Exists(_updaterFilePath))
            return false;

        var manifest = AusManifest.ParseFile(manifestFilePath);

        EnsurePrepareManifest();

        var patchs = Manifest!.CheckForUpdates(manifest);

        foreach (var file in patchs)
        {
            if (!file.ValidateFile(baseDir))
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc/>
    public void LaunchUpdater(Version? version, LaunchUpdaterOptions? options = null)
    {
        EnsureNotDisposed();

        if (version == null && !HasUpdatePrepared(out version))
        {
            return;
        }

        EnsureUpdaterNotLaunched();

        using var _ = EnsureLockFileAcquired();

        options ??= new();

        // Get package content directory path
        var packageContentDirPath = Path.Combine(_storageDirPath, $"{version}");

        // Get original command line arguments and encode them to avoid issues with quotes
        var routedArgs = options.Arguments == null ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(options.Arguments));

        // Prepare arguments
        var updaterArgs = $"\"{_entryFilePath}\" \"{_applicationDir}\" \"{packageContentDirPath}\" \"{options.Restart}\" \"{routedArgs}\"";

        var updaterStartInfo = new ProcessStartInfo
        {
            FileName = _updaterFilePath,
            Arguments = updaterArgs,
            CreateNoWindow = true,
            UseShellExecute = true,
            Verb = "runas",
        };

        using (var updaterProcess = new Process { StartInfo = updaterStartInfo })
        {
            try
            {
                updaterProcess.Start();
            }
            catch (Win32Exception ex)
            {
                // win32 cancelled 用户取消操作
                if (ex.NativeErrorCode == 1223)
                {
                    if (!options.ForceUpdate)
                        return;

                    //else 什么都不做，后续继续执行，也就是关闭程序
                }
            }
        }

        options.Launched?.Invoke();

        if (options.Exit)
        {
            Dispose();
            KillEntryProcess();
        }
    }

    private LockFile EnsureLockFileAcquired()
    {
        Directory.CreateDirectory(_storageDirPath);
        _lockFile ??= LockFile.TryAcquire(_lockFilePath);

        // If failed to acquire - throw
        if (_lockFile == null)
            throw new Exception("LockFileNotAcquiredException");
        return _lockFile;
    }

    private void KillEntryProcess()
    {
        if (_entryFilePath != Environment.ProcessPath)
        {
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (process.MainWindowHandle != IntPtr.Zero &&
                        process.MainModule != null &&
                        process.MainModule.FileName == _entryFilePath)
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch { }
                        break;
                    }
                }
                //如果程序本身是32位进程，当它访问 64位 进程时（获取MainModule属性）将促发异常
                catch { }
            }
        }

        Environment.Exit(0);
        Process.GetCurrentProcess().Kill();
    }

    private void EnsureUpdaterNotLaunched()
    {
        if (File.Exists(_updaterFilePath) && !FileSystemHelper.CheckFileWriteAccess(_updaterFilePath))
            throw new Exception("UpdaterAlreadyLaunchedException");
    }

    private void EnsureNotDisposed()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(GetType().FullName);
    }

    private HttpClient CreateHttpClient()
    {
        if (!_updateUrlValid)
        {
            throw new UpdateException("Invaild update url.", null);
        }

        return new HttpClient()
        {
            BaseAddress = new Uri(_updateUrl)
        };
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (!_isDisposed)
        {
            _isDisposed = true;
            _lockFile?.Dispose();
        }
    }
}
