using Calls;
using Lantern.Windows;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Lantern.Messaging;

internal partial class IpcImpl : Ipc, ILanternService, IDisposable
{
    private readonly ILogger _logger;
    private readonly ICall _call;

    public IpcImpl(
        IpcOptions ipcOptions,
        IAppLifetime lifetime,
        ICall call,
        ILogger<IpcImpl> logger)
    {
        _ipcOptions = ipcOptions;
        _lifetime = lifetime;
        _call = call;
        _logger = logger;
        _thread = new Thread(RunLoop)
        {
            Name = "Lantern message queue processing thread",
            IsBackground = true
        };
    }

    public override bool Emit(IWebViewWindow? sender, string @event, object? body = null)
    {
        try
        {
            List<SubscribeDescription> subscriptions = GetSubscriptions(sender, @event).ToList();

            if (subscriptions.Count == 0)
                return false;

            string? json = null;
            string? jsonWithWindow = null;

            List<SubscribeDescription> removes = new();

            foreach (var subscription in subscriptions)
            {
                if (subscription.Subscriber.WindowClosed.IsCancellationRequested)
                    continue;

                if (sender == null || sender == subscription.Subscriber)
                {
                    json ??= Serialize(null);
                    subscription.Subscriber.PostMessage(json);
                }
                else
                {
                    jsonWithWindow ??= Serialize(sender.Name);
                    subscription.Subscriber.PostMessage(jsonWithWindow);
                }

                if (subscription.Once)
                {
                    removes.Add(subscription);
                }
            }

            _logger.LogTrace($"Ipc -> emit {@event}\r\n{json}");

            foreach (var remove in removes)
            {
                _subscriptions.Remove(remove);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ipc -> emit {@event} error");
            return false;
        }

        string Serialize(string? window = null)
        {
            return JsonSerializer.Serialize(new IpcEvent
            {
                Name = @event,
                Body = body,
                Target = window,
            }, _ipcOptions.JsonSerializerOptions);
        }
    }

}
