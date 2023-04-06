using Lantern.Windows;

namespace Lantern.Messaging;

#pragma warning disable IDE1006 // 命名样式

public interface IpcContext
{
    IWebViewWindow Window { get; }
    void Unlisten(string name);
    void UnlistenAll();
    void Listen(string name, bool once = false);
    bool Emit(string name, object? body);
}

public interface IpcContext<T> : IpcContext
{
    T Body { get; }
}
