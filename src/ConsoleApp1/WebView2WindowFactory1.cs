using Microsoft.Web.WebView2.Core;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static Lantern.Win32.Interop.NativeMethods;

public static class WebView2WindowFactory1
{
    public static void RunMessagePump()
    {
        var result = 0;
        while ((result = GetMessage(out var msg, IntPtr.Zero, 0, 0)) > 0)
        {
            TranslateMessage(ref msg);
            DispatchMessage(ref msg);
        }
    }

    public static IntPtr CreateWindow()
    {
        // Ensure that the delegate doesn't get garbage collected by storing it as a field.

        var _className = $"Lantern-{Guid.NewGuid():N}";

        // Unique DC helps with performance when using Gpu based rendering
        const ClassStyles WindowClassStyle = ClassStyles.CS_OWNDC | ClassStyles.CS_HREDRAW | ClassStyles.CS_VREDRAW;

        var wndClassEx = new WNDCLASSEX
        {
            cbSize = Marshal.SizeOf<WNDCLASSEX>(),
            style = (int)WindowClassStyle,
            lpfnWndProc = WndProc,
            hInstance = GetModuleHandle(null!),
            hCursor = 0,
            hbrBackground = IntPtr.Zero,
            lpszClassName = _className,
            lpszMenuName = null!,
            hIcon = 0,
        };

        ushort atom = RegisterClassEx(ref wndClassEx);

        if (atom == 0)
        {
            throw new Win32Exception();
        }

        WindowStyles style = WindowStyles.WS_OVERLAPPEDWINDOW | WindowStyles.WS_CLIPCHILDREN;
        WindowStyles exStyle = 0;


        var _hwnd = CreateWindowEx(
            (int)exStyle,
            atom,
            "test",
            (uint)style,
            CW_USEDEFAULT,
            CW_USEDEFAULT,
            CW_USEDEFAULT,
            CW_USEDEFAULT,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero);

        if (_hwnd == IntPtr.Zero)
        {
            throw new Win32Exception();
        }
        return _hwnd;
    }


    static unsafe IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        var message = (WindowsMessage)msg;
        return DefWindowProc(hWnd, msg, wParam, lParam);
    }

    public static void SetDpiAwareness()
    {
        // Ideally we'd set DPI awareness in the manifest but this doesn't work for netcoreapp2.0
        // apps as they are actually dlls run by a console loader. Instead we have to do it in code,
        // but there are various ways to do this depending on the OS version.
        var user32 = LoadLibrary("user32.dll");
        var method = GetProcAddress(user32, nameof(SetProcessDpiAwarenessContext));

        if (method != IntPtr.Zero)
        {
            if (SetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2) ||
                SetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE))
            {
                return;
            }
        }

        var shcore = LoadLibrary("shcore.dll");
        method = GetProcAddress(shcore, nameof(SetProcessDpiAwareness));

        if (method != IntPtr.Zero)
        {
            SetProcessDpiAwareness(PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE);
            return;
        }

        SetProcessDPIAware();
    }

    public static async void AttachWebView2(nint hwnd)
    {
        var _environment = await CoreWebView2Environment.CreateAsync();
        var controller = await _environment.CreateCoreWebView2ControllerAsync(hwnd);
        controller.Bounds = new System.Drawing.Rectangle(0, 0, 1600, 900);
        controller.CoreWebView2.SetVirtualHostNameToFolderMapping("app.lantern", "wwwroot", CoreWebView2HostResourceAccessKind.Allow);
        controller.CoreWebView2.Navigate("https://app.lantern/index.html");

        controller.CoreWebView2.IsDefaultDownloadDialogOpenChanged += CoreWebView2_IsDefaultDownloadDialogOpenChanged;

    }

    private static void CoreWebView2_IsDefaultDownloadDialogOpenChanged(object? sender, object e)
    {

    }
}
