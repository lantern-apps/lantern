namespace Lantern;

public class WebViewEnvironmentOptions
{
    public string? BrowserExecutableFolder { get; set; }
    public string? UserDataFolder { get; set; }
    public string? Language { get; set; }
    public List<string>? JavaScriptsOnDocumentCreated { get; set; }
    public WebViewPermissionKind[]? AllowedPermissions { get; set; }
    public WebViewEnvironmentUninstallHanding UninstallHanding { get; set; }

    public IList<WebViewRequestFilterWarpper> Filters { get; set; } = new List<WebViewRequestFilterWarpper>();
    public IList<WebViewVirtualHostMapping> VirtualHosts { get; } = new List<WebViewVirtualHostMapping>();

    public void AddRequestFilter(IWebViewRequestFilter handler)
    {
        Filters ??= new List<WebViewRequestFilterWarpper>();
        Filters.Add(new WebViewRequestFilterWarpper(handler));
    }

    public void AddVirtualHostMapping(string hostName, string folderName)
    {
        VirtualHosts.Add(new(hostName, folderName));
    }
}
