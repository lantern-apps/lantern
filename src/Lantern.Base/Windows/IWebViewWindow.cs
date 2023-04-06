namespace Lantern.Windows;

public interface IWebViewWindow : IWindow
{
    string Name { get; }
    Task EnsureWebViewInitializedAsync();
    Task<string?> GetUrlAsync();
    Task SetUrlAsync(string url);
}
