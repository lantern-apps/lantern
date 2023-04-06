using Lantern.Windows;
using System.Text.Json.Serialization;

namespace Lantern;

public class LanternOptions : AppOptions
{
    public LanternOptions(AppOptions? appOptions = null)
    {
        if (appOptions != null)
        {
            AppName = appOptions.AppName;
            UserDataFolder = appOptions.UserDataFolder;
            DefaultIconPath = appOptions.DefaultIconPath;
            IsSingleInstance = appOptions.IsSingleInstance;
            Version = appOptions.Version;
            ShutdownCondition = appOptions.ShutdownCondition;
        }
    }

    [JsonPropertyName("webViewEnvironment")]
    public WebViewEnvironmentOptions WebViewEnvironment { get; set; } = new();

    [JsonPropertyName("windows")]
    public List<WebViewWindowOptions> Windows { get; set; } = new();

    [JsonPropertyName("trays")]
    public List<TrayOptions> Trays { get; set; } = new();

    [JsonPropertyName("updater")]
    public UpdaterOptions Updater { get; set; } = new();
}
