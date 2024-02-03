namespace Lantern.AsService;

public partial class WebViewBrowser
{
    public async Task DispatchMouseDownEventAsync(string selector)
    {
        var rect = await GetBoundingClientRect(selector);
        var center = rect.GetCenter();
        await DispatchMouseDownEventAsync(center.X, center.Y);
    }

    public async Task DispatchMouseUpEventAsync(string selector)
    {
        var rect = await GetBoundingClientRect(selector);
        var center = rect.GetCenter();
        await DispatchMouseUpEventAsync(center.X, center.Y);
    }

    public async Task DispatchMouseClickEventAsync(string selector)
    {
        var rect = await GetBoundingClientRect(selector);
        var center = rect.GetCenter();
        await DispatchMouseClickEventAsync(center.X, center.Y);
    }

    public async Task DispatchMouseDownEventAsync(double x, double y)
    {
        await InvokeAsync(() => Cdp.Input.DispatchMouseEventAsync("mousePressed", x, y, button: "left"));
    }

    public async Task DispatchMouseUpEventAsync(double x, double y)
    {
        await InvokeAsync(() => Cdp.Input.DispatchMouseEventAsync("mouseReleased", x, y, button: "left"));
    }

    public async Task DispatchMouseMoveEventAsync(double x, double y)
    {
        await InvokeAsync(() => Cdp.Input.DispatchMouseEventAsync("mouseMoved", x, y));
    }

    public async Task DispatchMouseClickEventAsync(double x, double y)
    {
        await InvokeAsync(() => Cdp.Input.DispatchMouseEventAsync("mousePressed", x, y, button: "left", clickCount: 1));
        await Task.Delay(50);
        await InvokeAsync(() => Cdp.Input.DispatchMouseEventAsync("mouseReleased", x, y, button: "left"));
    }

}
