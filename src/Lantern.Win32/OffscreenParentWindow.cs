using System.ComponentModel;
using System.Runtime.InteropServices;
using static Lantern.Win32.Interop.NativeMethods;

namespace Lantern.Win32;

internal static class OffscreenParentWindow
{
    public static IntPtr Handle { get; } = CreateParentWindow();
    private static WndProc? s_wndProcDelegate;

    private static IntPtr CreateParentWindow()
    {
        s_wndProcDelegate = new WndProc(ParentWndProc);

        var wndClassEx = new WNDCLASSEX
        {
            cbSize = Marshal.SizeOf<WNDCLASSEX>(),
            hInstance = GetModuleHandle(null),
            lpfnWndProc = s_wndProcDelegate,
            lpszClassName = $"Lantern-EmbeddedWindow-{Guid.NewGuid():N}",
        };

        var atom = RegisterClassEx(ref wndClassEx);

        if (atom == 0)
        {
            throw new Win32Exception();
        }

        var hwnd = CreateWindowEx(
            0,
            atom,
            null!,
            (int)WindowStyles.WS_OVERLAPPEDWINDOW,
            CW_USEDEFAULT,
            CW_USEDEFAULT,
            CW_USEDEFAULT,
            CW_USEDEFAULT,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero);

        if (hwnd == IntPtr.Zero)
        {
            throw new Win32Exception();
        }

        return hwnd;
    }

    private static IntPtr ParentWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam) => DefWindowProc(hWnd, msg, wParam, lParam);
}
