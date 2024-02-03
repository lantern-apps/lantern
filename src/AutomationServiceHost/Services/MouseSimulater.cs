using Lantern.AsService;
using Lantern.Threading;

namespace AutomationServiceHost.Services;

public class MouseSimulater
{
    public static async Task SimulateDragAsync(WebViewBrowser browser, int x, int y, int offset, CancellationToken cancellationToken)
    {
        await Dispatcher.UIThread.InvokeAsync(() => browser.Cdp.Input.DispatchMouseEventAsync("mousePressed", x, y, button: "left"));

        var min = (int)Math.Floor(offset / 8.0);
        var max = (int)Math.Ceiling(offset / 4.0);
        var target = x + offset;
        double current_offset = x;

        //增
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            current_offset += Random.Shared.Next(min, max);
            await Dispatcher.UIThread.InvokeAsync(() => browser.Cdp.Input.DispatchMouseEventAsync("mouseMoved", current_offset, y));
            await Task.Delay(Random.Shared.Next(10, 100), cancellationToken);

            if ((current_offset - x) > offset * 1.2)
            {
                break;
            }
        }

        //减少
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            current_offset -= Random.Shared.Next(min, max);
            await Dispatcher.UIThread.InvokeAsync(() => browser.Cdp.Input.DispatchMouseEventAsync("mouseMoved", current_offset, y));
            await Task.Delay(Random.Shared.Next(10, 100), cancellationToken);

            if ((current_offset - x) <= offset * 0.9)
            {
                break;
            }
        }

        cancellationToken.ThrowIfCancellationRequested();

        await Dispatcher.UIThread.InvokeAsync(() => browser.Cdp.Input.DispatchMouseEventAsync("mouseMoved", target, y));
        await Task.Delay(Random.Shared.Next(10, 100), cancellationToken);
        await Dispatcher.UIThread.InvokeAsync(() => browser.Cdp.Input.DispatchMouseEventAsync("mouseReleased", offset, y, button: "left"));
    }
}

//public class MouseHelper
//{
//    private static Task DelayRandom() => Task.Delay(Random.Shared.Next(50, 200));
//    public static async Task SimulateDragAsync(MouseSimulatePoint start, int x)
//    {

//        SetCursorPos(start.GetX(), start.GetY());

//        var startX = (int)(65535.0 / 3440 * start.X);
//        var startY = (int)(65535.0 / 1440 * start.Y);

//        var final = (int)(65535.0 / 3440 * (start.X + x)) + 100;

//        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);

//        await DelayRandom();

//        //mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, x, 0, 0, 0);
//        //await Task.Delay(Random.Shared.Next(50, 200));

//        //mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);


//        int offset = startX;
//        while (true)
//        {
//            offset += Random.Shared.Next(500, 2000);
//            mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, offset, startY, 0, 0);

//            await DelayRandom();

//            if (offset > final * 1.2)
//            {
//                break;
//            }
//        }

//        while (true)
//        {
//            offset -= Random.Shared.Next(500, 2000);
//            mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, offset, startY, 0, 0);

//            await DelayRandom();

//            if (offset < final * 1.1)
//            {
//                break;
//            }
//        }

//        await DelayRandom();
//        mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, final, startY, 0, 0);

//        await DelayRandom();
//        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);


//    }

//    [DllImport("user32.dll")]
//    public static extern bool SetCursorPos(int X, int Y);

//    [DllImport("user32")]
//    public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

//    //移动鼠标 
//    public const int MOUSEEVENTF_MOVE = 0x0001;
//    //模拟鼠标左键按下 
//    public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
//    //模拟鼠标左键抬起 
//    public const int MOUSEEVENTF_LEFTUP = 0x0004;
//    //模拟鼠标右键按下 
//    public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
//    //模拟鼠标右键抬起 
//    public const int MOUSEEVENTF_RIGHTUP = 0x0010;
//    //模拟鼠标中键按下 
//    public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
//    //模拟鼠标中键抬起 
//    public const int MOUSEEVENTF_MIDDLEUP = 0x0040;
//    //标示是否采用绝对坐标 
//    public const int MOUSEEVENTF_ABSOLUTE = 0x8000;
//}