using Lantern.Windows;

namespace Lantern.Messaging;

internal partial class IpcImpl
{
    private readonly IpcOptions _ipcOptions;
    private readonly List<SubscribeDescription> _subscriptions = new();

    public override void Listen(IWebViewWindow subscriber, IWebViewWindow window, string @event, bool once)
    {
        if (subscriber is not WebViewWindow webview)
            return;

        var subscription = _subscriptions.FirstOrDefault(x => x.Subscriber == subscriber && x.Window == window && x.Event == @event);
        if (subscription == null)
        {
            _subscriptions.Add(new SubscribeDescription(webview, window, @event, once));
        }
        else
        {
            subscription.Once = once;
        }
    }

    public override void Unlisten(IWebViewWindow window)
    {
        var subscription = _subscriptions.FirstOrDefault(x => x.Subscriber == window || x.Window == window);
        if (subscription != null)
        {
            _subscriptions.Remove(subscription);
        }
    }

    public override void Unlisten(IWebViewWindow subscriber, IWebViewWindow window, string @event)
    {
        var subscription = _subscriptions.FirstOrDefault(x => x.Subscriber == subscriber && x.Window == window && x.Event == @event);
        if (subscription != null)
        {
            _subscriptions.Remove(subscription);
        }
    }


    private IEnumerable<SubscribeDescription> GetSubscriptions(IWebViewWindow? sender, string @event)
    {
        foreach (var subscription in _subscriptions)
        {
            if ((sender == null || subscription.Window == sender) && subscription.Event == @event)
            {
                yield return subscription;
            }
        }
    }

    private sealed class SubscribeDescription
    {
        public SubscribeDescription(
            WebViewWindow subscriber,
            IWebViewWindow window,
            string @event,
            bool once = false)
        {
            Subscriber = subscriber;
            Window = window;
            Event = @event;
            Once = once;
        }

        public readonly WebViewWindow Subscriber;
        public readonly IWebViewWindow Window;
        public readonly string Event;
        public bool Once;
    }

}
