using Lantern.Platform;
using Lantern.Win32.Interop;
using Lantern.Windows;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static Lantern.Win32.Interop.NativeMethods;

namespace Lantern.Win32;

public partial class WindowImpl
{
    //private static readonly IntPtr DefaultCursor = LoadCursor(IntPtr.Zero, new IntPtr((int)UnmanagedMethods.Cursor.IDC_ARROW));
    private static readonly Version Windows8 = new Version(6, 2);
    private static readonly Version Windows7 = new Version(6, 1);

    private static readonly List<WindowImpl> _instances = new();

    private static readonly WindowStyles Style = WindowStyles.WS_OVERLAPPEDWINDOW | WindowStyles.WS_CLIPCHILDREN;
    private static readonly WindowStyles ExStyle = 0;

    private readonly WndProc _wndProcDelegate;

    private string _className;
    private IntPtr _hwnd;

    unsafe public WindowImpl()
    {
        _windowProperties = new WindowProperties
        {
            ShowInTaskbar = true,
            IsResizable = true,
            Decorations = WindowDecorations.Full
        };

        // Ensure that the delegate doesn't get garbage collected by storing it as a field.
        _wndProcDelegate = WndProc;

        _className = $"Lantern-{Guid.NewGuid():N}";

        // Unique DC helps with performance when using Gpu based rendering
        const ClassStyles WindowClassStyle = ClassStyles.CS_OWNDC | ClassStyles.CS_HREDRAW | ClassStyles.CS_VREDRAW;

        var wndClassEx = new WNDCLASSEX
        {
            cbSize = Marshal.SizeOf<WNDCLASSEX>(),
            style = (int)WindowClassStyle,
            lpfnWndProc = _wndProcDelegate,
            hInstance = GetModuleHandle(null!),
            hCursor = 0,
            hbrBackground = 6,
            lpszClassName = _className,
            lpszMenuName = null!,
            hIcon = IntPtr.Zero,
        };

        ushort atom = RegisterClassEx(ref wndClassEx);

        if (atom == 0)
            throw new Win32Exception();

        _hwnd = CreateWindowEx(
            (int)ExStyle,
            atom,
            _title!,
            (uint)Style,
            CW_USEDEFAULT,
            CW_USEDEFAULT,
            CW_USEDEFAULT,
            CW_USEDEFAULT,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero);

        if (_hwnd == IntPtr.Zero)
            throw new Win32Exception();

        if (NativeUtilities.ShCoreAvailable && Environment.OSVersion.Version > Windows8)
        {
            var monitor = MonitorFromWindow(
                _hwnd,
                MONITOR.MONITOR_DEFAULTTONEAREST);

            if (GetDpiForMonitor(
                monitor,
                MONITOR_DPI_TYPE.MDT_EFFECTIVE_DPI,
                out var dpix,
                out var dpiy) == 0)
            {
                _scaling = dpix / 96.0;
            }
        }

        _instances.Add(this);
    }
}
