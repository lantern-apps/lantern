namespace Lantern.Threading;

/// <summary>
/// SynchronizationContext to be used on main thread
/// </summary>
public class DispatcherSynchronizationContext : SynchronizationContext
{
    public static void Install()
    {
        if (Current is DispatcherSynchronizationContext)
            return;
        SetSynchronizationContext(new DispatcherSynchronizationContext());
    }

    /// <inheritdoc/>
    public override void Post(SendOrPostCallback d, object? state)
    {
        Dispatcher.UIThread.Post(d, state, DispatcherPriority.Background);
    }

    /// <inheritdoc/>
    public override void Send(SendOrPostCallback d, object? state)
    {
        if (Dispatcher.UIThread.CheckAccess())
            d(state);
        else
            Dispatcher.UIThread.InvokeAsync(() => d(state), DispatcherPriority.Send).GetAwaiter().GetResult();
    }
}
