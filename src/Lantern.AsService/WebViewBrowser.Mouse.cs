namespace Lantern.AsService;

public partial class WebViewBrowser
{
    public async Task SimulateMouseDownEventAsync(string selector)
    {
        var rect = await GetBoundingClientRect(selector);
        if (rect.IsEmpty)
            return;
        var center = rect.GetCenter();
        await SimulateMouseDownEventAsync(center.X, center.Y);
    }

    public async Task SimulateMouseUpEventAsync(string selector)
    {
        var rect = await GetBoundingClientRect(selector);
        if (rect.IsEmpty)
            return;
        var center = rect.GetCenter();
        await SimulateMouseUpEventAsync(center.X, center.Y);
    }

    public async Task SimulateMouseClickEventAsync(string selector)
    {
        var rect = await GetBoundingClientRect(selector);
        if (rect.IsEmpty)
            return;
        var center = rect.GetCenter();
        await SimulateMouseClickEventAsync(center.X, center.Y);
    }

    public async Task SimulateMouseDownEventAsync(double x, double y)
    {
        await InvokeAsync(() => Cdp.Input.DispatchMouseEventAsync("mousePressed", x, y, button: "left"));
    }

    public async Task SimulateMouseUpEventAsync(double x, double y)
    {
        await InvokeAsync(() => Cdp.Input.DispatchMouseEventAsync("mouseReleased", x, y, button: "left"));
    }

    public async Task SimulateMouseMoveEventAsync(double x, double y)
    {
        await InvokeAsync(() => Cdp.Input.DispatchMouseEventAsync("mouseMoved", x, y));
    }

    public async Task SimulateMouseClickEventAsync(double x, double y)
    {
        await InvokeAsync(() => Cdp.Input.DispatchMouseEventAsync("mousePressed", x, y, button: "left", clickCount: 1));
        await Task.Delay(50);
        await InvokeAsync(() => Cdp.Input.DispatchMouseEventAsync("mouseReleased", x, y, button: "left"));
    }

    public async Task SimulateInputAsync(string selector, string input)
    {
        var rect = await GetBoundingClientRect(selector);
        if (rect.IsEmpty)
            return;

        var center = rect.GetCenter();
        await SimulateMouseClickEventAsync(center.X, center.Y);
        await Task.Delay(50);
        await InvokeAsync(() => Cdp.Input.InsertTextAsync(input));
    }
}
