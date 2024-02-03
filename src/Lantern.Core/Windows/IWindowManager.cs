namespace Lantern.Windows;

public interface IWindowManager
{
    IWebViewWindow? GetWindow(string name);
    IWebViewWindow? GetDefaultWindow();
    IWebViewWindow[] GetAllWindows();
    Task<IWebViewWindow> CreateWindowAsync(WebViewWindowOptions options);
}