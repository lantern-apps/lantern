using static Lantern.Win32.Interop.NativeMethods;

namespace Lantern.Win32;

public partial class TrayIconImpl
{
    private const uint WM_TRAYMOUSE = (uint)WindowsMessage.WM_USER + 1024;
    private static readonly uint WM_TASKBARCREATED = RegisterWindowMessage("TaskbarCreated");
    private static readonly Dictionary<int, TrayIconImpl> s_trayIcons = new();

    private const uint NIN_BALLOONSHOW = (uint)WindowsMessage.WM_USER + 2;
    private const uint NIN_BALLOONHIDE = (uint)WindowsMessage.WM_USER + 3;
    private const uint NIN_BALLOONTIMEOUT = (uint)WindowsMessage.WM_USER + 4;
    private const uint NIN_BALLOONUSERCLICK = (uint)WindowsMessage.WM_USER + 5;


    public static TrayIconImpl? GetDefault() => s_trayIcons.Values.FirstOrDefault();

    internal void WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        switch (lParam.ToInt32())
        {
            case (int)WindowsMessage.WM_LBUTTONUP:
                Click?.Invoke();
                break;

            case (int)WindowsMessage.WM_RBUTTONUP:
                ShowContextMenu();
                break;

            case (int)NIN_BALLOONUSERCLICK:
                BallonClick?.Invoke();
                break;
            case (int)NIN_BALLOONHIDE:
            case (int)NIN_BALLOONTIMEOUT:
                break;
        }
    }

    internal static void ProcWnd(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        if (msg == WM_TRAYMOUSE)
        {
            if (s_trayIcons.ContainsKey(wParam.ToInt32()))
            {
                s_trayIcons[wParam.ToInt32()].WndProc(hWnd, msg, wParam, lParam);
            }
        }
        else if (msg == WM_TASKBARCREATED)
        {
            foreach (var tray in s_trayIcons.Values)
            {
                if (tray._visible)
                {
                    tray.IsVisible = false;
                    tray.IsVisible = true;
                }
            }
        }
    }

}
