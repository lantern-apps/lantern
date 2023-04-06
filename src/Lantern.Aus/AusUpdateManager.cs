using Lantern.Aus.Internal;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Lantern.Aus;

/// <inheritdoc/>
public class AusUpdateManager : IAusUpdateManager, IDisposable
{
    private const string UpdaterResourceName = "Lantern.Aus.Updater.exe";
    private const string ManifestName = ".manifest";
    private const string PatchName = ".patch";

    private static readonly string _updateDirectory = AppDomain.CurrentDomain.BaseDirectory;
    private static readonly string _manifestFilePath = Path.Combine(_updateDirectory, ManifestName);

    /// <summary>
    /// Current program manifest
    /// </summary>
    public static AusManifest Manifest { get; private set; } = null!;

    /// <summary>
    /// Update patch
    /// </summary>
    public static AusManifest? Patch { get; private set; }

    private readonly string _storageDirPath;
    private readonly string _lockFilePath;
    private readonly string _updaterFilePath;
    private readonly string[] _exclusiveFiles;

    private readonly AusUpdateClient _updateClient;
    private readonly Version _initVersion;
    private readonly string _package;
    private readonly string _entryFilePath;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public AusUpdateManager(AusUpdateOptions options)
    {
        if (string.IsNullOrEmpty(options.ServerAddress))
            throw new ArgumentNullException(nameof(options.ServerAddress), $"The {nameof(AusUpdateOptions)}.{nameof(options.ServerAddress)} value cannot be null or empty.");

        Assembly assembly = Assembly.GetEntryAssembly()!;
        _package = options.PackageName ?? assembly.GetName().Name!;
        _initVersion = options.InitVersion ?? assembly.GetName().Version ?? new Version(0, 0, 0);
        _entryFilePath = options.EntryFilePath ?? assembly.Location!;

        if (options.Exclusives == null)
        {
            _exclusiveFiles = new string[] { ManifestName, PatchName };
        }
        else
        {
            _exclusiveFiles = new string[options.Exclusives.Count + 2];
            _exclusiveFiles[0] = ManifestName;
            _exclusiveFiles[1] = PatchName;
            options.Exclusives.CopyTo(_exclusiveFiles, 2);
        }

        _updateClient = new AusUpdateClient(options.ServerAddress, _package);

        _storageDirPath = options.TempFilePath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _package, "Update");
        _lockFilePath = Path.Combine(_storageDirPath, $"{_package}.lock");

        if (options.UpdaterName == null)
        {
            _updaterFilePath = Path.Combine(_storageDirPath, $"{_package}.Updater.exe");
        }
        else
        {
            if (options.UpdaterName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                _updaterFilePath = Path.Combine(_storageDirPath, $"{options.UpdaterName}");
            else
                _updaterFilePath = Path.Combine(_storageDirPath, $"{options.UpdaterName}.exe");
        }
    }

    private LockFile? _lockFile;
    private bool _isDisposed;

    /// <inheritdoc/>
    public async Task EnsurePrepareManifestAsync(CancellationToken cancellationToken = default)
    {
        if (Manifest != null)
            return;

        if (File.Exists(_manifestFilePath))
        {
            try
            {
                Manifest = AusManifest.LoadFromFile(_manifestFilePath);
                if (Manifest?.Version != null && Manifest.Version >= _initVersion)
                    return;
            }
            catch { }
        }

        var files = await AusManifest.LoadAsync(_updateDirectory, _exclusiveFiles, cancellationToken);
        Manifest = new()
        {
            Name = _package,
            Version = _initVersion,
            Files = files
        };
        Manifest.SaveAs(_manifestFilePath);
    }

    /// <inheritdoc/>
    public async Task<AusUpdateResult> CheckForUpdateAsync(CancellationToken cancellationToken = default)
    {
        await EnsurePrepareManifestAsync(cancellationToken);

        var remote = await _updateClient.GetUpdateAsync(Manifest.Version, cancellationToken);

        if (remote == null)
            return new AusUpdateResult(Manifest, null, null, false);

        var files = Manifest.CheckForUpdates(remote);

        if (IsUpdatePrepared(remote.Version))
            return new AusUpdateResult(Manifest, remote, files, true);
        else
            return new AusUpdateResult(Manifest, remote, files, false);
    }

    /// <inheritdoc/>
    public async Task CheckPerformUpdateAsync(CancellationToken cancellationToken = default)
    {
        EnsureNotDisposed();
        EnsureLockFileAcquired();

        var result = await CheckForUpdateAsync(cancellationToken);
        if (!result.CanUpdate)
            return;

        if (cancellationToken.IsCancellationRequested)
            return;

        await PrepareUpdateAsync(result, cancellationToken);

        LaunchUpdater(result.Patch.Version);
    }

    /// <inheritdoc/>
    public async Task PrepareUpdateAsync(AusUpdateResult result, CancellationToken cancellationToken = default)
    {
        EnsureNotDisposed();
        EnsureLockFileAcquired();

        if (!result.CanUpdate)
            return;

        var destDirPath = Path.Combine(_storageDirPath, result.Patch.Version.ToString());

        Directory.CreateDirectory(destDirPath);

        foreach (var file in result.UpdateFiles)
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

            await _updateClient.DownloadAsync(file.FromVersion ?? result.Patch.Version, file.Name, destFilePath, null, cancellationToken);
        }

        var patch = new AusManifest
        {
            Name = result.Patch.Name,
            Version = result.Patch.Version,
            Files = result.UpdateFiles.ToList()
        };
        patch.SaveAs(Path.Combine(destDirPath, PatchName));

        Patch = patch;

        var manifest = result.Manifest.ApplyPatch(patch);
        manifest.ClearFileVersion(true);
        manifest.SaveAs(Path.Combine(destDirPath, ManifestName));

        if (cancellationToken.IsCancellationRequested)
            return;

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
        if (version == null || version < _initVersion)
            return false;

        return IsUpdatePrepared(version);
    }

    /// <inheritdoc/>
    public bool IsUpdatePrepared(Version version)
    {
        EnsureNotDisposed();
        var manifestFilePath = Path.Combine(_storageDirPath, version.ToString(), ManifestName);
        var patchFilePath = Path.Combine(_storageDirPath, version.ToString(), PatchName);

        if (!File.Exists(manifestFilePath) ||
            !File.Exists(patchFilePath) ||
            !File.Exists(_updaterFilePath))
            return false;

        var patch = AusManifest.LoadFromFile(patchFilePath);

        foreach (var file in patch.Files)
        {
            var fileInfo = new FileInfo(Path.Combine(_storageDirPath, version.ToString(), file.Name));
            if (!fileInfo.Exists)
                return false;

            if (fileInfo.Length != file.Size)
                return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public void LaunchUpdater(Version? version, LaunchUpdaterOptions? options = null)
    {
        if (version == null && !HasUpdatePrepared(out version))
        {
            return;
        }

        EnsureNotDisposed();
        EnsureLockFileAcquired();
        EnsureUpdaterNotLaunched();
        EnsureUpdatePrepared(version);

        options ??= new();

        // Get package content directory path
        var packageContentDirPath = Path.Combine(_storageDirPath, $"{version}");

        // Get original command line arguments and encode them to avoid issues with quotes
        var routedArgs = options.Arguments == null ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(options.Arguments));

        // Prepare arguments
        var updaterArgs = $"\"{_entryFilePath}\" \"{packageContentDirPath}\" \"{options.Restart}\" \"{routedArgs}\"";

        // Create updater process start info
        var updaterStartInfo = new ProcessStartInfo
        {
            FileName = _updaterFilePath,
            Arguments = updaterArgs,
            CreateNoWindow = true,
            UseShellExecute = true,
            Verb = "runas",
        };

        //// Decide if updater needs to be elevated
        //var updateeDirPath = Path.GetDirectoryName(_updateDirectory);

        //// If updater needs to be elevated - use shell execute with "runas"
        //var updaterNeedsElevation = !string.IsNullOrWhiteSpace(updateeDirPath) && !FileSystemHelper.CheckDirectoryWriteAccess(updateeDirPath);
        //if (updaterNeedsElevation)
        //{
        //    updaterStartInfo.Verb = "runas";
        //    updaterStartInfo.UseShellExecute = true;
        //}

        //var patchFilePath = Path.Combine(_storageDirPath, version.ToString(), PatchName);
        //if (File.Exists(patchFilePath))
        //    File.Delete(patchFilePath);

        // Create and start updater process
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

    private void EnsureLockFileAcquired()
    {
        Directory.CreateDirectory(_storageDirPath);
        _lockFile ??= LockFile.TryAcquire(_lockFilePath);

        // If failed to acquire - throw
        if (_lockFile == null)
            throw new Exception("LockFileNotAcquiredException");
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

    private void EnsureUpdatePrepared(Version version)
    {
        if (!IsUpdatePrepared(version))
            throw new Exception("UpdateNotPreparedException");
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            _isDisposed = true;
            _lockFile?.Dispose();
            _updateClient?.Dispose();
        }
    }
}
