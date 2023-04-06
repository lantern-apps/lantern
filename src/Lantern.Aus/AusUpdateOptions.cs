using System.Reflection;

namespace Lantern.Aus;

/// <inheritdoc/>
public class AusUpdateOptions
{
    /// <inheritdoc/>
    public AusUpdateOptions() { }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="serverAddress">Aus Update Server Address</param>
    public AusUpdateOptions(string serverAddress)
    {
        ServerAddress = serverAddress;
    }

    /// <summary>
    /// Aus Update Server Address
    /// </summary>
    public string ServerAddress { get; set; } = null!;

    /// <summary>
    /// Program package name, the default value is entry assembly name.
    /// </summary>
    public string? PackageName { get; set; }

    /// <summary>
    /// Initialize version, the default value is entry assembly version.
    /// </summary>
    public Version? InitVersion { get; set; }

    /// <summary>
    /// Entry file path, the default value is entry assembly location.
    /// </summary>
    public string? EntryFilePath { get; set; }

    /// <summary>
    /// Update file download path, the default value is %LocalAppData%\{PackageName}\Update
    /// </summary>
    public string? TempFilePath { get; set; }

    /// <summary>
    /// Updater program file name
    /// </summary>
    public string? UpdaterName { get; set; }

    /// <summary>
    /// Exclusive file path
    /// </summary>
    public List<string> Exclusives { get; set; } = new();

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

        PackageName ??= assemblyName.Name;
        InitVersion ??= assemblyName.Version ?? new Version(0, 0, 0);
        EntryFilePath ??= assembly.Location!;
    }
}
