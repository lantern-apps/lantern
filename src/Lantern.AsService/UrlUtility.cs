namespace Lantern.AsService;

public static class UrlUtility
{
    public static string GetUrlWithoutQueryString(string url)
    {
        ArgumentNullException.ThrowIfNull(nameof(url));

        var index = url.IndexOf('?');
        if (index > 0)
            return url[..index].TrimEnd('/');

        return url.TrimEnd('/');
    }

    public static string GetQueryString(string url)
    {
        ArgumentNullException.ThrowIfNull(nameof(url));

        var index = url.IndexOf('?');
        if (index > 0)
            return url[index..].TrimEnd('/');

        return string.Empty;
    }

}
