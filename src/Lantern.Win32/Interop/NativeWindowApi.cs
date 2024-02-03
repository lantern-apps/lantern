namespace Lantern.Win32.Interop;

public class NativeWindowApi
{
    public static void ShowWindow(nint hWnd)
    {
        NativeMethods.ShowWindowCommand command = NativeMethods.IsWindowVisible(hWnd) ? NativeMethods.ShowWindowCommand.Restore : NativeMethods.ShowWindowCommand.Normal;
        NativeMethods.ShowWindow(hWnd, command);
        NativeMethods.SetFocus(hWnd);
        NativeMethods.SetForegroundWindow(hWnd);
    }

    public static void HideWindow(nint hWnd)
    {
        NativeMethods.ShowWindow(hWnd, NativeMethods.ShowWindowCommand.Hide);
    }

}
