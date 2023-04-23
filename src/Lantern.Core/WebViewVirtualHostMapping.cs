namespace Lantern;

public class WebViewVirtualHostMapping
{
    public WebViewVirtualHostMapping(string hostName, string folderName)
    {
        HostName = hostName;
        FolderName = folderName;
    }

    public string HostName { get; }
    public string FolderName { get; }
};
