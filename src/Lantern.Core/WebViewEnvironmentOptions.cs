
public class WebViewEnvironmentOptions
{
    public string? BrowserExecutableFolder { get; set; }
    public string? UserDataFolder { get; set; }
    public string? Language { get; set; }
    public List<string>? JavaScriptsOnDocumentCreated { get; set; }
    public WebViewPermissionKind[]? AllowedPermissions { get; set; }

    public IList<VirtualHostMapping> VirtualHosts { get; } = new List<VirtualHostMapping>();
    public IDictionary<string, Type> HostObjects { get; } = new Dictionary<string, Type>();

    public void AddHostObject<T>(string name) => HostObjects.Add(name, typeof(T));
    public void AddVirtualHostMapping(string hostName, string folderName) => VirtualHosts.Add(new(hostName, folderName));
    public WebView2EnvironmentUninstallHanding UninstallHanding { get; set; }

}

public enum WebView2EnvironmentUninstallHanding
{
    DownloadAndInstall,
    Exit,
}

public class VirtualHostMapping
{
    public VirtualHostMapping(string hostName, string folderName)
    {
        HostName = hostName;
        FolderName = folderName;
    }

    public string HostName { get; }
    public string FolderName { get; }
};

public enum WebViewPermissionKind
{
    //
    // 摘要:
    //     Indicates an unknown permission.
    UnknownPermission,
    //
    // 摘要:
    //     Indicates permission to capture audio.
    Microphone,
    //
    // 摘要:
    //     Indicates permission to capture video.
    Camera,
    //
    // 摘要:
    //     Indicates permission to access geolocation.
    Geolocation,
    //
    // 摘要:
    //     Indicates permission to send web notifications. Apps that would like to show
    //     notifications should handle Microsoft.Web.WebView2.Core.CoreWebView2.PermissionRequested
    //     and/or Microsoft.Web.WebView2.Core.CoreWebView2Frame.PermissionRequested events
    //     and no browser permission prompt will be shown for notification requests. Note
    //     that push notifications are currently unavailable in WebView2.
    Notifications,
    //
    // 摘要:
    //     Indicates permission to access generic sensor. Generic Sensor covers ambient-light-sensor,
    //     accelerometer, gyroscope, and magnetometer.
    OtherSensors,
    //
    // 摘要:
    //     Indicates permission to read the system clipboard without a user gesture.
    ClipboardRead,
    //
    // 摘要:
    //     Indicates permission to automatically download multiple files.
    //
    // 言论：
    //     Permission is requested when multiple downloads are triggered in quick succession.
    MultipleAutomaticDownloads,
    //
    // 摘要:
    //     Indicates permission to read and write to files or folders on the device.
    //
    // 言论：
    //     Permission is requested when developers use the [File System Access API](https://developer.mozilla.org/en-US/docs/Web/API/File_System_Access_API)
    //     to show the file or folder picker to the end user, and then request "readwrite"
    //     permission for the user's selection.
    FileReadWrite,
    //
    // 摘要:
    //     Indicates permission to play audio and video automatically on sites.
    //
    // 言论：
    //     This permission affects the autoplay attribute and play method of the audio and
    //     video HTML elements, and the start method of the Web Audio API. See the [Autoplay
    //     guide for media and Web Audio APIs](https://developer.mozilla.org/en-US/docs/Web/Media/Autoplay_guide)
    //     for details.
    Autoplay,
    //
    // 摘要:
    //     Indicates permission to use fonts on the device.
    //
    // 言论：
    //     Permission is requested when developers use the [Local Font Access API](https://wicg.github.io/local-font-access/)
    //     to query the system fonts available for styling web content.
    LocalFonts,
    //
    // 摘要:
    //     Indicates permission to send and receive system exclusive messages to/from MIDI
    //     (Musical Instrument Digital Interface) devices.
    //
    // 言论：
    //     Permission is requested when developers use the [Web MIDI API](https://developer.mozilla.org/en-US/docs/Web/API/Web_MIDI_API)
    //     to request access to system exclusive MIDI messages.
    MidiSystemExclusiveMessages

}