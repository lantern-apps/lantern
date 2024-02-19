namespace Lantern;

public class UpdaterOptions
{
    /// <summary>
    /// Aus Update Server Address
    /// </summary>
    public string ServerAddress { get; set; } = null!;

    /// <summary>
    /// Update file download path, the default value is %LocalAppData%\{PackageName}\Update
    /// </summary>
    public string? UpdateDataPath { get; set; }

    /// <summary>
    /// Updater program file name
    /// </summary>
    public string? UpdaterName { get; set; }


    public string? ManifestName { get; set; }

    /// <summary>
    /// Exclusive file path
    /// </summary>
    public List<string> Exclusives { get; } = new()
    {
        "*.exe.WebView2/**",
    };

    public TimeSpan CheckPeriod { get; set; } = TimeSpan.FromHours(1);

    public TimeSpan CheckDelay { get; set; } = TimeSpan.FromSeconds(5);

}
