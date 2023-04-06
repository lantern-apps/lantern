using Calls;
using System.Reflection;

namespace Lantern.Messaging;

public interface IEventEmitter
{
    void Emit(string name, object? body);

    void Emit(string name) => Emit(name, null);

    public void Emit(object body)
    {
        if (body == null)
            throw new ArgumentNullException(nameof(body));

        var name = body.GetType().GetCustomAttribute<RoutingKeyAttribute>()?.Name;

        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Message name cannot be null or empty.");

        Emit(name, body);
    }
}