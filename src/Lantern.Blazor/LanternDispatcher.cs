using Microsoft.AspNetCore.Components;

namespace Lantern.Blazor;

public class LanternDispatcher : Dispatcher
{
    public override bool CheckAccess()
    {
        return Threading.Dispatcher.UIThread.CheckAccess();
    }

    public override Task InvokeAsync(Action workItem)
    {
        return Threading.Dispatcher.UIThread.InvokeAsync(workItem);
    }

    public override Task InvokeAsync(Func<Task> workItem)
    {
        return Threading.Dispatcher.UIThread.InvokeAsync(workItem);
    }

    public override Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem)
    {
        return Threading.Dispatcher.UIThread.InvokeAsync(workItem);
    }

    public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem)
    {
        return Threading.Dispatcher.UIThread.InvokeAsync(workItem);
    }
}
