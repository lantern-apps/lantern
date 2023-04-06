using Lantern.Windows;

namespace Lantern.Messaging;

public class IpcContextImpl : IpcContext
{
    private readonly Ipc _ipc;
    private readonly WebViewWindow _callee;

    public IpcContextImpl(
        WebViewWindow callee,
        IWebViewWindow window,
        Ipc ipc)
    {
        _callee = callee;
        Window = window;
        _ipc = ipc;
    }

    public IWebViewWindow Window { get; }

    public void Unlisten(string name) => _ipc.Unlisten(_callee, Window, name);
    public void UnlistenAll() => _ipc.Unlisten(_callee);
    public void Listen(string name, bool once = false) => _ipc.Listen(_callee, Window, name, once);
    public bool Emit(string name, object? body) => _ipc.Emit(_callee, name, body);
}

public sealed class IpcContextImpl<T> : IpcContextImpl, IpcContext<T>
{
    public IpcContextImpl(
        WebViewWindow callee,
        IWebViewWindow window,
        Ipc ipc,
        T body) : base(callee, window, ipc)
    {
        Body = body;
    }

    public T Body { get; }
}