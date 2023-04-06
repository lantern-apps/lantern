using Lantern.Platform;
using Lantern.Windows;
using System.Drawing;
using static Lantern.Win32.Interop.NativeMethods;

namespace Lantern.Win32;

public class ScreenImpl : IScreenImpl
{
    private Screen[]? _allScreens;

    public int ScreenCount
    {
        get => GetSystemMetrics(SystemMetric.SM_CMONITORS);
    }

    public Screen[] AllScreens => GetAllScreens();


    public Screen? ScreenFromPoint(PhysicsPosition point)
    {
        var monitor = MonitorFromPoint(new POINT
        {
            X = point.X,
            Y = point.Y
        }, MONITOR.MONITOR_DEFAULTTONULL);

        return FindScreenByHandle(monitor);
    }

    public Screen? ScreenFromWindow(IWindowImpl window)
    {
        var handle = ((WindowImpl)window).NativeHandle;
        var monitor = MonitorFromWindow(handle, MONITOR.MONITOR_DEFAULTTONULL);
        return FindScreenByHandle(monitor);
    }

    public Screen? ScreenFromRect(PhysicsRectangle rect)
    {
        var monitor = MonitorFromRect(new RECT
        {
            left = rect.X,
            top = rect.Y,
            right = rect.X + rect.Width,
            bottom = rect.Y + rect.Height
        }, MONITOR.MONITOR_DEFAULTTONULL);

        return FindScreenByHandle(monitor);
    }

    private Screen? FindScreenByHandle(IntPtr handle)
    {
        return AllScreens.Cast<Win32Screen>().FirstOrDefault(m => m.Handle == handle)!;
    }

    private Screen[] GetAllScreens()
    {
        if (_allScreens == null)
        {
            int index = 0;
            Screen[] screens = new Screen[ScreenCount];
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
                (IntPtr monitor, IntPtr hdcMonitor, ref Rectangle lprcMonitor, IntPtr data) =>
                {
                    MONITORINFO monitorInfo = MONITORINFO.Create();
                    if (GetMonitorInfo(monitor, ref monitorInfo))
                    {
                        var dpi = 1.0;

                        var shcore = LoadLibrary("shcore.dll");
                        var method = GetProcAddress(shcore, nameof(GetDpiForMonitor));
                        if (method != IntPtr.Zero)
                        {
                            GetDpiForMonitor(monitor, MONITOR_DPI_TYPE.MDT_EFFECTIVE_DPI, out var x, out _);
                            dpi = (double)x;
                        }
                        else
                        {
                            var hdc = GetDC(IntPtr.Zero);

                            double virtW = GetDeviceCaps(hdc, DEVICECAP.HORZRES);
                            double physW = GetDeviceCaps(hdc, DEVICECAP.DESKTOPHORZRES);

                            dpi = (96d * physW / virtW);

                            ReleaseDC(IntPtr.Zero, hdc);
                        }

                        RECT bounds = monitorInfo.rcMonitor;
                        RECT workingArea = monitorInfo.rcWork;
                        PhysicsRectangle rectBounds = new(bounds.left, bounds.top, bounds.Width, bounds.Height);
                        PhysicsRectangle rectWorkArea = new(workingArea.left, workingArea.top, workingArea.Width, workingArea.Height);
                        screens[index] = new Win32Screen(dpi / 96.0d, rectBounds, rectWorkArea, monitorInfo.dwFlags == 1, monitor);
                        index++;
                    }
                    return true;
                }, IntPtr.Zero);
            _allScreens = screens;
        }
        return _allScreens;
    }
}
