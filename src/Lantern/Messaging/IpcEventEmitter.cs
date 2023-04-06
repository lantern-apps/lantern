namespace Lantern.Messaging;

internal class IpcEventEmitter : IEventEmitter
{
    private readonly Ipc _ipc;

    public IpcEventEmitter(Ipc ipc)
    {
        _ipc = ipc;
    }

    public void Emit(string name, object? body) => _ipc.Emit(null, name, body);
}
