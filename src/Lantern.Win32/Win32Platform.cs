using Lantern.Platform;
using Lantern.Threading;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Lantern.Win32.Interop.NativeMethods;

namespace Lantern.Win32;

public class Win32Platform : IPlatformThreadingInterface, IWindowingPlatform, INotifyingPlatform
{
    internal static readonly Win32Platform Instance = new();
    private static Thread? _uiThread;

    private WndProc _wndProcDelegate = null!;
    private IntPtr _hwnd;

    private Win32Platform()
    {
        SetDpiAwareness();
    }

    public bool CurrentThreadIsLoopThread => _uiThread == Thread.CurrentThread;

    public event Action<DispatcherPriority?>? Signaled;
    public event Func<bool>? ShutdownRequested;
    internal IntPtr Handle => _hwnd;
    public IScreenImpl Screen { get; } = new ScreenImpl();
    public Thread UIThread => _uiThread!;

    public void Initialize()
    {
        _uiThread = Thread.CurrentThread;
        CreateMessageWindow();
    }

    public void RunLoop(CancellationToken cancellationToken)
    {
        //消息泵创建在UI线程
        //同时启动消息泵前需创建至少一个窗口

        var result = 0;
        while (!cancellationToken.IsCancellationRequested && (result = GetMessage(out var msg, IntPtr.Zero, 0, 0)) > 0)
        {
            TranslateMessage(ref msg);
            DispatchMessage(ref msg);
        }

        if (result < 0)
        {
            //Logging.Logger.TryGet(Logging.LogEventLevel.Error, Logging.LogArea.Win32Platform)
            //    ?.Log(this, "Unmanaged error in {0}. Error Code: {1}", nameof(RunLoop), Marshal.GetLastWin32Error());
        }
    }

    private const int SignalW = unchecked((int)0xdeadbeaf);
    private const int SignalL = unchecked((int)0x12345678);

    public bool ShowNotification(string title, string? info) => TrayIconImpl.GetDefault()?.ShowNotification(title, info) ?? false;

    public void Signal(DispatcherPriority priority)
    {
        PostMessage(_hwnd, (int)WindowsMessage.WM_DISPATCH_WORK_ITEM, new IntPtr(SignalW), new IntPtr(SignalL));
    }

    private void CreateMessageWindow()
    {
        // Ensure that the delegate doesn't get garbage collected by storing it as a field.
        _wndProcDelegate = new WndProc(WndProc);

        WNDCLASSEX wndClassEx = new()
        {
            cbSize = Marshal.SizeOf<WNDCLASSEX>(),
            lpfnWndProc = _wndProcDelegate,
            hInstance = GetModuleHandle(null),
            lpszClassName = "Lantern-Message-" + Guid.NewGuid().ToString("N"),
        };

        ushort atom = RegisterClassEx(ref wndClassEx);

        if (atom == 0)
        {
            throw new Win32Exception();
        }

        _hwnd = CreateWindowEx(0, atom, null!, 0, 0, 0, 0, 0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

        if (_hwnd == IntPtr.Zero)
        {
            throw new Win32Exception();
        }
    }

    private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        if (msg == (int)WindowsMessage.WM_DISPATCH_WORK_ITEM && wParam.ToInt64() == SignalW && lParam.ToInt64() == SignalL)
        {
            try
            {
                Signaled?.Invoke(null);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        if (msg == (uint)WindowsMessage.WM_QUERYENDSESSION)
        {
            if (ShutdownRequested != null)
            {
                if (ShutdownRequested())
                {
                    return IntPtr.Zero;
                }
            }
        }

        //if (msg == (uint)WindowsMessage.WM_SETTINGCHANGE
        //    && PlatformSettings is Win32PlatformSettings win32PlatformSettings)
        //{
        //    var changedSetting = Marshal.PtrToStringAuto(lParam);
        //    if (changedSetting == "ImmersiveColorSet" // dark/light mode
        //        || changedSetting == "WindowsThemeElement") // high contrast mode
        //    {
        //        win32PlatformSettings.OnColorValuesChanged();
        //    }
        //}

        TrayIconImpl.ProcWnd(hWnd, msg, wParam, lParam);

        return DefWindowProc(hWnd, msg, wParam, lParam);
    }

    public IWindowImpl CreateWindow() => new WindowImpl();
    //public IWindowImpl CreateLoadingWindow() => new LoadingWindowImpl();

    public ITrayIconImpl CreateTrayIcon(string? icon = null, string? tooltip = null) => new TrayIconImpl(icon, tooltip);

    private static void SetDpiAwareness()
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
}
