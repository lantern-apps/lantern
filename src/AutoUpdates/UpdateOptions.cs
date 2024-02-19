using System.Reflection;

namespace AutoUpdates;

/// <inheritdoc/>
public class UpdateOptions
{
    /// <inheritdoc/>
    public UpdateOptions() { }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="serverAddress">Aus Update Server Address</param>
    public UpdateOptions(string serverAddress)
    {
        ServerAddress = serverAddress;
    }

    /// <summary>
    /// Aus Update Server Address
    /// </summary>
    public string ServerAddress { get; set; } = null!;

    /// <summary>
    /// Application name, the default value is entry assembly name.
    /// </summary>
    public string? AppName { get; set; }

    /// <summary>
    /// Initialize version, the default value is entry assembly version.
    /// </summary>
    public Version? Version { get; set; }

    /// <summary>
    /// Entry file path, the default value is entry assembly location.
    /// </summary>
    public string? EntryFilePath { get; set; }

    public string? AppDirectory { get; set; }

    /// <summary>
    /// Update file download path, the default value is %LocalAppData%\{PackageName}\Update
    /// </summary>
    public string? TempFilePath { get; set; }

    /// <summary>
    /// Updater program file name
    /// </summary>
    public string? UpdaterName { get; set; }

    public string? ManifestName { get; set; }

    /// <summary>
    /// Exclusive file path
    /// </summary>
    public List<string> Exclusives { get; set; } = [];

    internal void Validate()
    {
        if (ServerAddress == null)
            throw new ArgumentNullException(nameof(ServerAddress));

        if (!Uri.IsWellFormedUriString(ServerAddress, UriKind.Absolute))
            throw new ArgumentException(nameof(ServerAddress));

        if (ServerAddress.EndsWith('/'))
            ServerAddress = ServerAddress[..^1];

        Assembly assembly = Assembly.GetEntryAssembly()!;
        var assemblyName = assembly.GetName();

        AppName ??= assemblyName.Name;
        Version ??= assemblyName.Version ?? new Version(0, 0, 0);
        EntryFilePath ??= assembly.Location!;
    }
}
