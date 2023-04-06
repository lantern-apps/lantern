using Lantern.Windows;

namespace Lantern.Messaging;

public abstract class Ipc
{
    public abstract void Listen(IWebViewWindow subscriber, IWebViewWindow window, string @event, bool once);
    public abstract void Unlisten(IWebViewWindow window);
    public abstract void Unlisten(IWebViewWindow subscriber, IWebViewWindow window, string @event);
    public abstract bool Emit(IWebViewWindow? sender, string @event, object? body = null);
    public abstract void Post(IWebViewWindow window, string request);
}
