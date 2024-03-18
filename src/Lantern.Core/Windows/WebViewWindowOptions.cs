using System.Text.Json.Serialization;

namespace Lantern.Windows;

public class WebViewWindowOptions
{
    public static readonly WebViewWindowOptions Default = new();

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("icon")]
    public string? IconPath { get; set; }

    [JsonPropertyName("width")]
    public int? Width { get; set; }

    [JsonPropertyName("height")]
    public int? Height { get; set; }

    [JsonPropertyName("x")]
    public int? X { get; set; }

    [JsonPropertyName("y")]
    public int? Y { get; set; }

    [JsonPropertyName("minWidth")]
    public int? MinWidth { get; set; }

    [JsonPropertyName("minheight")]
    public int? MinHeight { get; set; }

    [JsonPropertyName("maxWidth")]
    public int? MaxWidth { get; set; }

    [JsonPropertyName("maxheight")]
    public int? MaxHeight { get; set; }

    [JsonPropertyName("center")]
    public bool Center { get; set; } = false;

    [JsonPropertyName("awaysOnTop")]
    public bool AwaysOnTop { get; set; } = false;

    [JsonPropertyName("skipTaskbar")]
    public bool SkipTaskbar { get; set; } = false;

    [JsonPropertyName("resizable")]
    public bool Resizable { get; set; } = true;

    [JsonPropertyName("titleBarStyle")]
    public WindowTitleBarStyle TitleBarStyle { get; set; } = WindowTitleBarStyle.Default;

    [JsonPropertyName("newWindowHandling")]
    public WebViewNewWindowHandling NewWindowHandling { get; set; } = WebViewNewWindowHandling.OpenSystemDefaultBrowser;

    [JsonPropertyName("titleHandling")]
    public WebViewWindowTitleHandling WindowTitleHandling { get; set; } = WebViewWindowTitleHandling.BindWebViewDocumentTitle;

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("visible")]
    public bool Visible { get; set; } = true;

    public string? ProxyServer { get; set; }
    public string? ProfileName { get; set; }
    public string? UserDataFolder { get; set; }
    public bool IsInPrivateModeEnabled { get; set; }
    public string? Language { get; set; }

    public string? UserAgent { get; set; }
    

    public bool AreDefaultContextMenusEnabled { get; set; } = false;
    public bool AreBrowserAcceleratorKeysEnabled { get; set; } = false;
    public bool AreDevToolsEnabled { get; set; } = false;
    public bool IsZoomControlEnabled { get; set; } = false;
    public bool IsStatusBarEnabled { get; set; } = false;
    public bool IsPinchZoomEnabled { get; set; } = false;
    public bool IsPasswordAutosaveEnabled { get; set; } = false;

    public bool IsGeneralAutofillEnabled { get; set; } = true;
    public bool IsSwipeNavigationEnabled { get; set; } = true;
    public bool IsScriptEnabled { get; set; } = true;
    public bool IsWebMessageEnabled { get; set; } = true;
    public bool AreHostObjectsAllowed { get; set; } = false;
    public bool IsBuiltInErrorPageEnabled { get; set; } = true;
}