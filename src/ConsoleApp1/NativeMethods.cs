using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using static Lantern.Win32.Interop.NativeMethods;

namespace Lantern.Win32.Interop;

internal static class NativeMethods
{
    public const int CW_USEDEFAULT = unchecked((int)0x80000000);

    public static readonly IntPtr DPI_AWARENESS_CONTEXT_UNAWARE = new IntPtr(-1);
    public static readonly IntPtr DPI_AWARENESS_CONTEXT_SYSTEM_AWARE = new IntPtr(-2);
    public static readonly IntPtr DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE = new IntPtr(-3);
    public static readonly IntPtr DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = new IntPtr(-4);


    public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetModuleHandle(string? lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr LoadLibrary(string fileName);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

    [DllImport("shcore.dll")]
    public static extern void SetProcessDpiAwareness(PROCESS_DPI_AWARENESS value);

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetProcessDpiAwarenessContext(IntPtr dpiAWarenessContext);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetProcessDPIAware();


    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint RegisterWindowMessage(string lpString);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "RegisterClassExW")]
    public static extern ushort RegisterClassEx(ref WNDCLASSEX lpwcx);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr CreateWindowEx(
       int dwExStyle,
       uint lpClassName,
       string lpWindowName,
       uint dwStyle,
       int x,
       int y,
       int nWidth,
       int nHeight,
       IntPtr hWndParent,
       IntPtr hMenu,
       IntPtr hInstance,
       IntPtr lpParam);

    [DllImport("user32.dll", EntryPoint = "DefWindowProcW")]
    public static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", EntryPoint = "GetMessageW", SetLastError = true)]
    public static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "PostMessageW")]
    public static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", EntryPoint = "SendMessage")]
    public static extern int SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern bool TranslateMessage(ref MSG lpMsg);

    [DllImport("user32.dll", EntryPoint = "DispatchMessageW")]
    public static extern IntPtr DispatchMessage(ref MSG lpmsg);


    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommand nCmdShow);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "SetWindowTextW")]
    public static extern bool SetWindowText(IntPtr hwnd, string lpString);

    [DllImport("user32.dll")]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern bool SetFocus(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosFlags uFlags);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowLongPtr(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLong")]
    public static extern uint GetWindowLong32b(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    public static extern bool SetParent(IntPtr hWnd, IntPtr hWndNewParent);

    [DllImport("user32.dll")]
    public static extern IntPtr GetParent(IntPtr hWnd);


    public static uint GetWindowLong(IntPtr hWnd, int nIndex) => IntPtr.Size == 4 ? GetWindowLong32b(hWnd, nIndex) : GetWindowLongPtr(hWnd, nIndex);

    [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLong")]
    private static extern uint SetWindowLong32b(IntPtr hWnd, int nIndex, uint value);

    [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLongPtr")]
    private static extern IntPtr SetWindowLong64b(IntPtr hWnd, int nIndex, IntPtr value);

    public static uint SetWindowLong(IntPtr hWnd, int nIndex, uint value) => IntPtr.Size == 4 ? SetWindowLong32b(hWnd, nIndex, value) : (uint)SetWindowLong64b(hWnd, nIndex, new IntPtr((uint)value)).ToInt32();

    public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr handle)
    {
        if (IntPtr.Size == 4)
        {
            return new IntPtr(SetWindowLong32b(hWnd, nIndex, (uint)handle.ToInt32()));
        }
        else
        {
            return SetWindowLong64b(hWnd, nIndex, handle);
        }
    }

    [DllImport("user32.dll")]
    public static extern bool GetClientRect(IntPtr hwnd, out RECT lpRect);

    [DllImport("user32.dll")]
    public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool AdjustWindowRectEx(ref RECT lpRect, uint dwStyle, bool bMenu, uint dwExStyle);

    [DllImport("user32.dll")]
    public static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

    [DllImport("user32.dll")]
    public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

    [DllImport("user32.dll", ExactSpelling = true, EntryPoint = "LoadImageW", SetLastError = true)]
    public static unsafe extern IntPtr LoadImage(IntPtr hInst, char* name, GdiImageType type, int cx, int cy, ImageFlags fuLoad);


    [DllImport("shell32", CharSet = CharSet.Auto)]
    public static extern int Shell_NotifyIcon(NIM dwMessage, NOTIFYICONDATA lpData);

    [DllImport("USER32.dll", ExactSpelling = true, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern bool DestroyMenu(IntPtr hMenu);


    [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
    internal static extern IntPtr CreatePopupMenu();

    [DllImport("user32.dll", ExactSpelling = true, EntryPoint = "AppendMenuW", SetLastError = true)]
    internal static unsafe extern int AppendMenu(IntPtr hMenu, MENU_ITEM_FLAGS uFlags, nuint uIDNewItem, char* lpNewItem);

    internal static unsafe int AppendMenu(SafeHandle hMenu, MENU_ITEM_FLAGS uFlags, nuint uIDNewItem, string? lpNewItem)
    {
        if (hMenu is null)
            throw new ArgumentNullException(nameof(hMenu));

        bool hMenuAddRef = false;
        try
        {
            fixed (char* lpNewItemLocal = lpNewItem)
            {
                IntPtr hMenuLocal;
                hMenu.DangerousAddRef(ref hMenuAddRef);
                hMenuLocal = hMenu.DangerousGetHandle();
                return AppendMenu(hMenuLocal, uFlags, uIDNewItem, lpNewItemLocal);
            }
        }
        finally
        {
            if (hMenuAddRef) hMenu.DangerousRelease();
        }
    }

    [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
    internal static extern unsafe int TrackPopupMenuEx(IntPtr hMenu, uint uFlags, int x, int y, IntPtr hwnd, [Optional] TPMPARAMS* lptpm);

    internal static unsafe int TrackPopupMenuEx(SafeHandle hMenu, uint uFlags, int x, int y, IntPtr hwnd, TPMPARAMS? lptpm)
    {
        bool hMenuAddRef = false;
        try
        {
            IntPtr hMenuLocal;
            if (hMenu is object)
            {
                hMenu.DangerousAddRef(ref hMenuAddRef);
                hMenuLocal = hMenu.DangerousGetHandle();
            }
            else
                throw new ArgumentNullException(nameof(hMenu));
            TPMPARAMS lptpmLocal = lptpm ?? default;
            int __result = TrackPopupMenuEx(hMenuLocal, uFlags, x, y, hwnd, lptpm.HasValue ? &lptpmLocal : null);
            return __result;
        }
        finally
        {
            if (hMenuAddRef)
                hMenu.DangerousRelease();
        }
    }

    public delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rectangle lprcMonitor, IntPtr dwData);

    [DllImport("user32.dll")]
    public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

    [DllImport("user32", EntryPoint = "GetMonitorInfoW", ExactSpelling = true, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetMonitorInfo([In] IntPtr hMonitor, ref MONITORINFO lpmi);

    [DllImport("user32.dll")]
    public static extern IntPtr MonitorFromPoint(POINT pt, MONITOR dwFlags);

    [DllImport("user32.dll")]
    public static extern IntPtr MonitorFromRect(RECT rect, MONITOR dwFlags);

    [DllImport("user32.dll")]
    public static extern IntPtr MonitorFromWindow(IntPtr hwnd, MONITOR dwFlags);

    [DllImport("shcore.dll")]
    public static extern long GetDpiForMonitor(IntPtr hmonitor, MONITOR_DPI_TYPE dpiType, out uint dpiX, out uint dpiY);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);
    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(SystemMetric smIndex);

    [DllImport("dwmapi.dll")]
    public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

    [DllImport("gdi32.dll")]
    public static extern int GetDeviceCaps(IntPtr hdc, DEVICECAP nIndex);

    [StructLayout(LayoutKind.Sequential)]
    internal struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    public enum MONITOR
    {
        MONITOR_DEFAULTTONULL = 0x00000000,
        MONITOR_DEFAULTTOPRIMARY = 0x00000001,
        MONITOR_DEFAULTTONEAREST = 0x00000002,
    }

    public enum SystemMetric
    {
        SM_CXSCREEN = 0,  // 0x00
        SM_CYSCREEN = 1,  // 0x01
        SM_CXVSCROLL = 2,  // 0x02
        SM_CYHSCROLL = 3,  // 0x03
        SM_CYCAPTION = 4,  // 0x04
        SM_CXBORDER = 5,  // 0x05
        SM_CYBORDER = 6,  // 0x06
        SM_CXDLGFRAME = 7,  // 0x07
        SM_CXFIXEDFRAME = 7,  // 0x07
        SM_CYDLGFRAME = 8,  // 0x08
        SM_CYFIXEDFRAME = 8,  // 0x08
        SM_CYVTHUMB = 9,  // 0x09
        SM_CXHTHUMB = 10, // 0x0A
        SM_CXICON = 11, // 0x0B
        SM_CYICON = 12, // 0x0C
        SM_CXCURSOR = 13, // 0x0D
        SM_CYCURSOR = 14, // 0x0E
        SM_CYMENU = 15, // 0x0F
        SM_CXFULLSCREEN = 16, // 0x10
        SM_CYFULLSCREEN = 17, // 0x11
        SM_CYKANJIWINDOW = 18, // 0x12
        SM_MOUSEPRESENT = 19, // 0x13
        SM_CYVSCROLL = 20, // 0x14
        SM_CXHSCROLL = 21, // 0x15
        SM_DEBUG = 22, // 0x16
        SM_SWAPBUTTON = 23, // 0x17
        SM_CXMIN = 28, // 0x1C
        SM_CYMIN = 29, // 0x1D
        SM_CXSIZE = 30, // 0x1E
        SM_CYSIZE = 31, // 0x1F
        SM_CXSIZEFRAME = 32, // 0x20
        SM_CXFRAME = 32, // 0x20
        SM_CYSIZEFRAME = 33, // 0x21
        SM_CYFRAME = 33, // 0x21
        SM_CXMINTRACK = 34, // 0x22
        SM_CYMINTRACK = 35, // 0x23
        SM_CXDOUBLECLK = 36, // 0x24
        SM_CYDOUBLECLK = 37, // 0x25
        SM_CXICONSPACING = 38, // 0x26
        SM_CYICONSPACING = 39, // 0x27
        SM_MENUDROPALIGNMENT = 40, // 0x28
        SM_PENWINDOWS = 41, // 0x29
        SM_DBCSENABLED = 42, // 0x2A
        SM_CMOUSEBUTTONS = 43, // 0x2B
        SM_SECURE = 44, // 0x2C
        SM_CXEDGE = 45, // 0x2D
        SM_CYEDGE = 46, // 0x2E
        SM_CXMINSPACING = 47, // 0x2F
        SM_CYMINSPACING = 48, // 0x30
        SM_CXSMICON = 49, // 0x31
        SM_CYSMICON = 50, // 0x32
        SM_CYSMCAPTION = 51, // 0x33
        SM_CXSMSIZE = 52, // 0x34
        SM_CYSMSIZE = 53, // 0x35
        SM_CXMENUSIZE = 54, // 0x36
        SM_CYMENUSIZE = 55, // 0x37
        SM_ARRANGE = 56, // 0x38
        SM_CXMINIMIZED = 57, // 0x39
        SM_CYMINIMIZED = 58, // 0x3A
        SM_CXMAXTRACK = 59, // 0x3B
        SM_CYMAXTRACK = 60, // 0x3C
        SM_CXMAXIMIZED = 61, // 0x3D
        SM_CYMAXIMIZED = 62, // 0x3E
        SM_NETWORK = 63, // 0x3F
        SM_CLEANBOOT = 67, // 0x43
        SM_CXDRAG = 68, // 0x44
        SM_CYDRAG = 69, // 0x45
        SM_SHOWSOUNDS = 70, // 0x46
        SM_CXMENUCHECK = 71, // 0x47
        SM_CYMENUCHECK = 72, // 0x48
        SM_SLOWMACHINE = 73, // 0x49
        SM_MIDEASTENABLED = 74, // 0x4A
        SM_MOUSEWHEELPRESENT = 75, // 0x4B
        SM_XVIRTUALSCREEN = 76, // 0x4C
        SM_YVIRTUALSCREEN = 77, // 0x4D
        SM_CXVIRTUALSCREEN = 78, // 0x4E
        SM_CYVIRTUALSCREEN = 79, // 0x4F
        SM_CMONITORS = 80, // 0x50
        SM_SAMEDISPLAYFORMAT = 81, // 0x51
        SM_IMMENABLED = 82, // 0x52
        SM_CXFOCUSBORDER = 83, // 0x53
        SM_CYFOCUSBORDER = 84, // 0x54
        SM_TABLETPC = 86, // 0x56
        SM_MEDIACENTER = 87, // 0x57
        SM_STARTER = 88, // 0x58
        SM_SERVERR2 = 89, // 0x59
        SM_MOUSEHORIZONTALWHEELPRESENT = 91, // 0x5B
        SM_CXPADDEDBORDER = 92, // 0x5C
        SM_DIGITIZER = 94, // 0x5E
        SM_MAXIMUMTOUCHES = 95, // 0x5F

        SM_REMOTESESSION = 0x1000, // 0x1000
        SM_SHUTTINGDOWN = 0x2000, // 0x2000
        SM_REMOTECONTROL = 0x2001, // 0x2001

        SM_CONVERTABLESLATEMODE = 0x2003,
        SM_SYSTEMDOCKED = 0x2004,
    }

    public enum DEVICECAP
    {
        HORZRES = 8,
        DESKTOPHORZRES = 118
    }

    public enum GdiImageType : uint
    {
        IMAGE_BITMAP = 0U,
        IMAGE_CURSOR = 2U,
        IMAGE_ICON = 1U,
    }

    [Flags]
    public enum ImageFlags : uint
    {
        LR_CREATEDIBSECTION = 0x00002000,
        LR_DEFAULTCOLOR = 0x00000000,
        LR_DEFAULTSIZE = 0x00000040,
        LR_LOADFROMFILE = 0x00000010,
        LR_LOADMAP3DCOLORS = 0x00001000,
        LR_LOADTRANSPARENT = 0x00000020,
        LR_MONOCHROME = 0x00000001,
        LR_SHARED = 0x00008000,
        LR_VGACOLOR = 0x00000080,
        LR_COPYDELETEORG = 0x00000008,
        LR_COPYFROMRESOURCE = 0x00004000,
        LR_COPYRETURNORG = 0x00000004,
    }


    public struct MSG
    {
        public IntPtr hwnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public POINT pt;
    }

    public struct POINT
    {
        public int X;
        public int Y;
    }

    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public int Width => right - left;
        public int Height => bottom - top;
        //public RECT(Rect rect)
        //{
        //    left = (int)rect.X;
        //    top = (int)rect.Y;
        //    right = (int)(rect.X + rect.Width);
        //    bottom = (int)(rect.Y + rect.Height);
        //}

        public void Offset(POINT pt)
        {
            left += pt.X;
            right += pt.X;
            top += pt.Y;
            bottom += pt.Y;
        }
    }

    public static class WindowPosZOrder
    {
        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
    }


    [Flags]
    public enum SetWindowPosFlags : uint
    {
        SWP_ASYNCWINDOWPOS = 0x4000,
        SWP_DEFERERASE = 0x2000,
        SWP_DRAWFRAME = 0x0020,
        SWP_FRAMECHANGED = 0x0020,
        SWP_HIDEWINDOW = 0x0080,
        SWP_NOACTIVATE = 0x0010,
        SWP_NOCOPYBITS = 0x0100,
        SWP_NOMOVE = 0x0002,
        SWP_NOOWNERZORDER = 0x0200,
        SWP_NOREDRAW = 0x0008,
        SWP_NOREPOSITION = 0x0200,
        SWP_NOSENDCHANGING = 0x0400,
        SWP_NOSIZE = 0x0001,
        SWP_NOZORDER = 0x0004,
        SWP_SHOWWINDOW = 0x0040,

        SWP_RESIZE = SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOZORDER
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WNDCLASSEX
    {
        public int cbSize;
        public int style;
        public WndProc lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        public string lpszMenuName;
        public string lpszClassName;
        public IntPtr hIconSm;
    }

    public enum WindowsMessage : uint
    {
        WM_NULL = 0x0000,
        WM_CREATE = 0x0001,
        WM_DESTROY = 0x0002,
        WM_MOVE = 0x0003,
        WM_SIZE = 0x0005,
        WM_ACTIVATE = 0x0006,
        WM_SETFOCUS = 0x0007,
        WM_KILLFOCUS = 0x0008,
        WM_ENABLE = 0x000A,
        WM_SETREDRAW = 0x000B,
        WM_SETTEXT = 0x000C,
        WM_GETTEXT = 0x000D,
        WM_GETTEXTLENGTH = 0x000E,
        WM_PAINT = 0x000F,
        WM_CLOSE = 0x0010,
        WM_QUERYENDSESSION = 0x0011,
        WM_QUERYOPEN = 0x0013,
        WM_ENDSESSION = 0x0016,
        WM_QUIT = 0x0012,
        WM_ERASEBKGND = 0x0014,
        WM_SYSCOLORCHANGE = 0x0015,
        WM_SHOWWINDOW = 0x0018,
        WM_WININICHANGE = 0x001A,
        WM_SETTINGCHANGE = WM_WININICHANGE,
        WM_DEVMODECHANGE = 0x001B,
        WM_ACTIVATEAPP = 0x001C,
        WM_FONTCHANGE = 0x001D,
        WM_TIMECHANGE = 0x001E,
        WM_CANCELMODE = 0x001F,
        WM_SETCURSOR = 0x0020,
        WM_MOUSEACTIVATE = 0x0021,
        WM_CHILDACTIVATE = 0x0022,
        WM_QUEUESYNC = 0x0023,
        WM_GETMINMAXINFO = 0x0024,
        WM_PAINTICON = 0x0026,
        WM_ICONERASEBKGND = 0x0027,
        WM_NEXTDLGCTL = 0x0028,
        WM_SPOOLERSTATUS = 0x002A,
        WM_DRAWITEM = 0x002B,
        WM_MEASUREITEM = 0x002C,
        WM_DELETEITEM = 0x002D,
        WM_VKEYTOITEM = 0x002E,
        WM_CHARTOITEM = 0x002F,
        WM_SETFONT = 0x0030,
        WM_GETFONT = 0x0031,
        WM_SETHOTKEY = 0x0032,
        WM_GETHOTKEY = 0x0033,
        WM_QUERYDRAGICON = 0x0037,
        WM_COMPAREITEM = 0x0039,
        WM_GETOBJECT = 0x003D,
        WM_COMPACTING = 0x0041,
        WM_WINDOWPOSCHANGING = 0x0046,
        WM_WINDOWPOSCHANGED = 0x0047,
        WM_COPYDATA = 0x004A,
        WM_CANCELJOURNAL = 0x004B,
        WM_NOTIFY = 0x004E,
        WM_INPUTLANGCHANGEREQUEST = 0x0050,
        WM_INPUTLANGCHANGE = 0x0051,
        WM_TCARD = 0x0052,
        WM_HELP = 0x0053,
        WM_USERCHANGED = 0x0054,
        WM_NOTIFYFORMAT = 0x0055,
        WM_CONTEXTMENU = 0x007B,
        WM_STYLECHANGING = 0x007C,
        WM_STYLECHANGED = 0x007D,
        WM_DISPLAYCHANGE = 0x007E,
        WM_GETICON = 0x007F,
        WM_SETICON = 0x0080,
        WM_NCCREATE = 0x0081,
        WM_NCDESTROY = 0x0082,
        WM_NCCALCSIZE = 0x0083,
        WM_NCHITTEST = 0x0084,
        WM_NCPAINT = 0x0085,
        WM_NCACTIVATE = 0x0086,
        WM_GETDLGCODE = 0x0087,
        WM_SYNCPAINT = 0x0088,
        WM_NCMOUSEMOVE = 0x00A0,
        WM_NCLBUTTONDOWN = 0x00A1,
        WM_NCLBUTTONUP = 0x00A2,
        WM_NCLBUTTONDBLCLK = 0x00A3,
        WM_NCRBUTTONDOWN = 0x00A4,
        WM_NCRBUTTONUP = 0x00A5,
        WM_NCRBUTTONDBLCLK = 0x00A6,
        WM_NCMBUTTONDOWN = 0x00A7,
        WM_NCMBUTTONUP = 0x00A8,
        WM_NCMBUTTONDBLCLK = 0x00A9,
        WM_NCXBUTTONDOWN = 0x00AB,
        WM_NCXBUTTONUP = 0x00AC,
        WM_NCXBUTTONDBLCLK = 0x00AD,
        WM_INPUT_DEVICE_CHANGE = 0x00FE,
        WM_INPUT = 0x00FF,
        WM_KEYFIRST = 0x0100,
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,
        WM_CHAR = 0x0102,
        WM_DEADCHAR = 0x0103,
        WM_SYSKEYDOWN = 0x0104,
        WM_SYSKEYUP = 0x0105,
        WM_SYSCHAR = 0x0106,
        WM_SYSDEADCHAR = 0x0107,
        WM_UNICHAR = 0x0109,
        WM_KEYLAST = 0x0109,
        WM_IME_STARTCOMPOSITION = 0x010D,
        WM_IME_ENDCOMPOSITION = 0x010E,
        WM_IME_COMPOSITION = 0x010F,
        WM_IME_KEYLAST = 0x010F,
        WM_INITDIALOG = 0x0110,
        WM_COMMAND = 0x0111,
        WM_SYSCOMMAND = 0x0112,
        WM_TIMER = 0x0113,
        WM_HSCROLL = 0x0114,
        WM_VSCROLL = 0x0115,
        WM_INITMENU = 0x0116,
        WM_INITMENUPOPUP = 0x0117,
        WM_MENUSELECT = 0x011F,
        WM_MENUCHAR = 0x0120,
        WM_ENTERIDLE = 0x0121,
        WM_MENURBUTTONUP = 0x0122,
        WM_MENUDRAG = 0x0123,
        WM_MENUGETOBJECT = 0x0124,
        WM_UNINITMENUPOPUP = 0x0125,
        WM_MENUCOMMAND = 0x0126,
        WM_CHANGEUISTATE = 0x0127,
        WM_UPDATEUISTATE = 0x0128,
        WM_QUERYUISTATE = 0x0129,
        WM_CTLCOLORMSGBOX = 0x0132,
        WM_CTLCOLOREDIT = 0x0133,
        WM_CTLCOLORLISTBOX = 0x0134,
        WM_CTLCOLORBTN = 0x0135,
        WM_CTLCOLORDLG = 0x0136,
        WM_CTLCOLORSCROLLBAR = 0x0137,
        WM_CTLCOLORSTATIC = 0x0138,
        WM_MOUSEFIRST = 0x0200,
        WM_MOUSEMOVE = 0x0200,
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_LBUTTONDBLCLK = 0x0203,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_RBUTTONDBLCLK = 0x0206,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208,
        WM_MBUTTONDBLCLK = 0x0209,
        WM_MOUSEWHEEL = 0x020A,
        WM_XBUTTONDOWN = 0x020B,
        WM_XBUTTONUP = 0x020C,
        WM_XBUTTONDBLCLK = 0x020D,
        WM_MOUSEHWHEEL = 0x020E,
        WM_MOUSELAST = 0x020E,
        WM_PARENTNOTIFY = 0x0210,
        WM_ENTERMENULOOP = 0x0211,
        WM_EXITMENULOOP = 0x0212,
        WM_NEXTMENU = 0x0213,
        WM_SIZING = 0x0214,
        WM_CAPTURECHANGED = 0x0215,
        WM_MOVING = 0x0216,
        WM_POWERBROADCAST = 0x0218,
        WM_DEVICECHANGE = 0x0219,
        WM_MDICREATE = 0x0220,
        WM_MDIDESTROY = 0x0221,
        WM_MDIACTIVATE = 0x0222,
        WM_MDIRESTORE = 0x0223,
        WM_MDINEXT = 0x0224,
        WM_MDIMAXIMIZE = 0x0225,
        WM_MDITILE = 0x0226,
        WM_MDICASCADE = 0x0227,
        WM_MDIICONARRANGE = 0x0228,
        WM_MDIGETACTIVE = 0x0229,
        WM_MDISETMENU = 0x0230,
        WM_ENTERSIZEMOVE = 0x0231,
        WM_EXITSIZEMOVE = 0x0232,
        WM_DROPFILES = 0x0233,
        WM_MDIREFRESHMENU = 0x0234,

        WM_POINTERDEVICECHANGE = 0x0238,
        WM_POINTERDEVICEINRANGE = 0x239,
        WM_POINTERDEVICEOUTOFRANGE = 0x23A,
        WM_NCPOINTERUPDATE = 0x0241,
        WM_NCPOINTERDOWN = 0x0242,
        WM_NCPOINTERUP = 0x0243,
        WM_POINTERUPDATE = 0x0245,
        WM_POINTERDOWN = 0x0246,
        WM_POINTERUP = 0x0247,
        WM_POINTERENTER = 0x0249,
        WM_POINTERLEAVE = 0x024A,
        WM_POINTERACTIVATE = 0x024B,
        WM_POINTERCAPTURECHANGED = 0x024C,
        WM_TOUCHHITTESTING = 0x024D,
        WM_POINTERWHEEL = 0x024E,
        WM_POINTERHWHEEL = 0x024F,
        DM_POINTERHITTEST = 0x0250,

        WM_IME_SETCONTEXT = 0x0281,
        WM_IME_NOTIFY = 0x0282,
        WM_IME_CONTROL = 0x0283,
        WM_IME_COMPOSITIONFULL = 0x0284,
        WM_IME_SELECT = 0x0285,
        WM_IME_CHAR = 0x0286,
        WM_IME_REQUEST = 0x0288,
        WM_IME_KEYDOWN = 0x0290,
        WM_IME_KEYUP = 0x0291,
        WM_MOUSEHOVER = 0x02A1,
        WM_MOUSELEAVE = 0x02A3,
        WM_NCMOUSEHOVER = 0x02A0,
        WM_NCMOUSELEAVE = 0x02A2,
        WM_WTSSESSION_CHANGE = 0x02B1,
        WM_TABLET_FIRST = 0x02c0,
        WM_TABLET_LAST = 0x02df,
        WM_DPICHANGED = 0x02E0,
        WM_CUT = 0x0300,
        WM_COPY = 0x0301,
        WM_PASTE = 0x0302,
        WM_CLEAR = 0x0303,
        WM_UNDO = 0x0304,
        WM_RENDERFORMAT = 0x0305,
        WM_RENDERALLFORMATS = 0x0306,
        WM_DESTROYCLIPBOARD = 0x0307,
        WM_DRAWCLIPBOARD = 0x0308,
        WM_PAINTCLIPBOARD = 0x0309,
        WM_VSCROLLCLIPBOARD = 0x030A,
        WM_SIZECLIPBOARD = 0x030B,
        WM_ASKCBFORMATNAME = 0x030C,
        WM_CHANGECBCHAIN = 0x030D,
        WM_HSCROLLCLIPBOARD = 0x030E,
        WM_QUERYNEWPALETTE = 0x030F,
        WM_PALETTEISCHANGING = 0x0310,
        WM_PALETTECHANGED = 0x0311,
        WM_HOTKEY = 0x0312,
        WM_PRINT = 0x0317,
        WM_PRINTCLIENT = 0x0318,
        WM_APPCOMMAND = 0x0319,
        WM_THEMECHANGED = 0x031A,
        WM_CLIPBOARDUPDATE = 0x031D,
        WM_DWMCOMPOSITIONCHANGED = 0x031E,
        WM_DWMNCRENDERINGCHANGED = 0x031F,
        WM_DWMCOLORIZATIONCOLORCHANGED = 0x0320,
        WM_DWMWINDOWMAXIMIZEDCHANGE = 0x0321,
        WM_GETTITLEBARINFOEX = 0x033F,
        WM_HANDHELDFIRST = 0x0358,
        WM_HANDHELDLAST = 0x035F,
        WM_AFXFIRST = 0x0360,
        WM_AFXLAST = 0x037F,
        WM_PENWINFIRST = 0x0380,
        WM_PENWINLAST = 0x038F,
        WM_TOUCH = 0x0240,
        WM_APP = 0x8000,
        WM_USER = 0x0400,

        WM_DISPATCH_WORK_ITEM = WM_USER,
    }

    [Flags]
    public enum ClassStyles : uint
    {
        CS_VREDRAW = 0x0001,
        CS_HREDRAW = 0x0002,
        CS_DBLCLKS = 0x0008,
        CS_OWNDC = 0x0020,
        CS_CLASSDC = 0x0040,
        CS_PARENTDC = 0x0080,
        CS_NOCLOSE = 0x0200,
        CS_SAVEBITS = 0x0800,
        CS_BYTEALIGNCLIENT = 0x1000,
        CS_BYTEALIGNWINDOW = 0x2000,
        CS_GLOBALCLASS = 0x4000,
        CS_IME = 0x00010000,
        CS_DROPSHADOW = 0x00020000
    }

    [Flags]
    public enum WindowStyles : uint
    {
        WS_BORDER = 0x800000,
        WS_CAPTION = 0xc00000,
        WS_CHILD = 0x40000000,
        WS_CLIPCHILDREN = 0x2000000,
        WS_CLIPSIBLINGS = 0x4000000,
        WS_DISABLED = 0x8000000,
        WS_DLGFRAME = 0x400000,
        WS_GROUP = 0x20000,
        WS_HSCROLL = 0x100000,
        WS_MAXIMIZE = 0x1000000,
        WS_MAXIMIZEBOX = 0x10000,
        WS_MINIMIZE = 0x20000000,
        WS_MINIMIZEBOX = 0x20000,
        WS_OVERLAPPED = 0x0,
        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
        WS_POPUP = 0x80000000u,
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
        WS_SIZEFRAME = 0x40000,
        WS_SYSMENU = 0x80000,
        WS_TABSTOP = 0x10000,
        WS_THICKFRAME = 0x40000,
        WS_VISIBLE = 0x10000000,
        WS_VSCROLL = 0x200000,
        WS_EX_DLGMODALFRAME = 0x00000001,
        WS_EX_NOPARENTNOTIFY = 0x00000004,
        WS_EX_NOREDIRECTIONBITMAP = 0x00200000,
        WS_EX_TOPMOST = 0x00000008,
        WS_EX_ACCEPTFILES = 0x00000010,
        WS_EX_TRANSPARENT = 0x00000020,
        WS_EX_MDICHILD = 0x00000040,
        WS_EX_TOOLWINDOW = 0x00000080,
        WS_EX_WINDOWEDGE = 0x00000100,
        WS_EX_CLIENTEDGE = 0x00000200,
        WS_EX_CONTEXTHELP = 0x00000400,
        WS_EX_RIGHT = 0x00001000,
        WS_EX_LEFT = 0x00000000,
        WS_EX_RTLREADING = 0x00002000,
        WS_EX_LTRREADING = 0x00000000,
        WS_EX_LEFTSCROLLBAR = 0x00004000,
        WS_EX_RIGHTSCROLLBAR = 0x00000000,
        WS_EX_CONTROLPARENT = 0x00010000,
        WS_EX_STATICEDGE = 0x00020000,
        WS_EX_APPWINDOW = 0x00040000,
        WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
        WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
        WS_EX_LAYERED = 0x00080000,
        WS_EX_NOINHERITLAYOUT = 0x00100000,
        WS_EX_LAYOUTRTL = 0x00400000,
        WS_EX_COMPOSITED = 0x02000000,
        WS_EX_NOACTIVATE = 0x08000000
    }

    public enum ShowWindowCommand
    {
        Hide = 0,
        Normal = 1,
        ShowMinimized = 2,
        Maximize = 3,
        ShowMaximized = 3,
        ShowNoActivate = 4,
        Show = 5,
        Minimize = 6,
        ShowMinNoActive = 7,
        ShowNA = 8,
        Restore = 9,
        ShowDefault = 10,
        ForceMinimize = 11
    }

    public enum PROCESS_DPI_AWARENESS
    {
        PROCESS_DPI_UNAWARE = 0,
        PROCESS_SYSTEM_DPI_AWARE = 1,
        PROCESS_PER_MONITOR_DPI_AWARE = 2
    }

    public enum SizeCommand
    {
        Restored,
        Minimized,
        Maximized,
        MaxShow,
        MaxHide,
    }

    public enum WindowActivate
    {
        WA_INACTIVE,
        WA_ACTIVE,
        WA_CLICKACTIVE,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PAINTSTRUCT
    {
        public IntPtr hdc;
        public bool fErase;
        public RECT rcPaint;
        public bool fRestore;
        public bool fIncUpdate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] rgbReserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal class NOTIFYICONDATA
    {
        public int cbSize = Marshal.SizeOf<NOTIFYICONDATA>();
        public IntPtr hWnd;
        public int uID;
        public NIF uFlags;
        public int uCallbackMessage;
        public IntPtr hIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szTip;
        public int dwState = 0;
        public int dwStateMask = 0;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szInfo;
        public int uTimeoutOrVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szInfoTitle;
        public NIIF dwInfoFlags;
    }

    public enum HitTestValues
    {
        HTERROR = -2,
        HTTRANSPARENT = -1,
        HTNOWHERE = 0,
        HTCLIENT = 1,
        HTCAPTION = 2,
        HTSYSMENU = 3,
        HTGROWBOX = 4,
        HTMENU = 5,
        HTHSCROLL = 6,
        HTVSCROLL = 7,
        HTMINBUTTON = 8,
        HTMAXBUTTON = 9,
        HTLEFT = 10,
        HTRIGHT = 11,
        HTTOP = 12,
        HTTOPLEFT = 13,
        HTTOPRIGHT = 14,
        HTBOTTOM = 15,
        HTBOTTOMLEFT = 16,
        HTBOTTOMRIGHT = 17,
        HTBORDER = 18,
        HTOBJECT = 19,
        HTCLOSE = 20,
        HTHELP = 21
    }

    internal enum NIM : uint
    {
        ADD = 0x00000000,
        MODIFY = 0x00000001,
        DELETE = 0x00000002,
        SETFOCUS = 0x00000003,
        SETVERSION = 0x00000004
    }

    [Flags]
    internal enum NIF : uint
    {
        MESSAGE = 0x00000001,
        ICON = 0x00000002,
        TIP = 0x00000004,
        STATE = 0x00000008,
        INFO = 0x00000010,
        GUID = 0x00000020,
        REALTIME = 0x00000040,
        SHOWTIP = 0x00000080
    }

    [Flags]
    internal enum NIIF : uint
    {
        NONE = 0x00000000,
        INFO = 0x00000001,
        WARNING = 0x00000002,
        ERROR = 0x00000003,
        USER = 0x00000004,
        ICON_MASK = 0x0000000F,
        NOSOUND = 0x00000010,
        LARGE_ICON = 0x00000020,
        RESPECT_QUIET_TIME = 0x00000080
    }

    [Flags]
    internal enum MENU_ITEM_FLAGS : uint
    {
        MF_BYCOMMAND = 0x00000000,
        MF_BYPOSITION = 0x00000400,
        MF_BITMAP = 0x00000004,
        MF_CHECKED = 0x00000008,
        MF_DISABLED = 0x00000002,
        MF_ENABLED = 0x00000000,
        MF_GRAYED = 0x00000001,
        MF_MENUBARBREAK = 0x00000020,
        MF_MENUBREAK = 0x00000040,
        MF_OWNERDRAW = 0x00000100,
        MF_POPUP = 0x00000010,
        MF_SEPARATOR = 0x00000800,
        MF_STRING = 0x00000000,
        MF_UNCHECKED = 0x00000000,
        MF_INSERT = 0x00000000,
        MF_CHANGE = 0x00000080,
        MF_APPEND = 0x00000100,
        MF_DELETE = 0x00000200,
        MF_REMOVE = 0x00001000,
        MF_USECHECKBITMAPS = 0x00000200,
        MF_UNHILITE = 0x00000000,
        MF_HILITE = 0x00000080,
        MF_DEFAULT = 0x00001000,
        MF_SYSMENU = 0x00002000,
        MF_HELP = 0x00004000,
        MF_RIGHTJUSTIFY = 0x00004000,
        MF_MOUSESELECT = 0x00008000,
        MF_END = 0x00000080,
    }

    [Flags]
    internal enum TRACK_POPUP_MENU_FLAGS : uint
    {
        TPM_LEFTBUTTON = 0x00000000,
        TPM_RIGHTBUTTON = 0x00000002,
        TPM_LEFTALIGN = 0x00000000,
        TPM_CENTERALIGN = 0x00000004,
        TPM_RIGHTALIGN = 0x00000008,
        TPM_TOPALIGN = 0x00000000,
        TPM_VCENTERALIGN = 0x00000010,
        TPM_BOTTOMALIGN = 0x00000020,
        TPM_HORIZONTAL = 0x00000000,
        TPM_VERTICAL = 0x00000040,
        TPM_NONOTIFY = 0x00000080,
        TPM_RETURNCMD = 0x00000100,
        TPM_RECURSE = 0x00000001,
        TPM_HORPOSANIMATION = 0x00000400,
        TPM_HORNEGANIMATION = 0x00000800,
        TPM_VERPOSANIMATION = 0x00001000,
        TPM_VERNEGANIMATION = 0x00002000,
        TPM_NOANIMATION = 0x00004000,
        TPM_LAYOUTRTL = 0x00008000,
        TPM_WORKAREA = 0x00010000,
    }

    internal partial struct TPMPARAMS
    {
        internal uint cbSize;
        internal RECT rcExclude;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MINMAXINFO
    {
        public POINT ptReserved;
        public POINT ptMaxSize;
        public POINT ptMaxPosition;
        public POINT ptMinTrackSize;
        public POINT ptMaxTrackSize;
    }

    internal class DestroyMenuSafeHandle : SafeHandle
    {
        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1L);

        internal DestroyMenuSafeHandle() : base(INVALID_HANDLE_VALUE, true) { }

        internal DestroyMenuSafeHandle(IntPtr preexistingHandle, bool ownsHandle = true) : base(INVALID_HANDLE_VALUE, ownsHandle)
        {
            this.SetHandle(preexistingHandle);
        }

        public override bool IsInvalid => this.handle.ToInt64() == -1L || this.handle.ToInt64() == 0L;

        protected override bool ReleaseHandle() => DestroyMenu(handle);
    }

    public enum WindowLongParam
    {
        GWL_WNDPROC = -4,
        GWL_HINSTANCE = -6,
        GWL_HWNDPARENT = -8,
        GWL_ID = -12,
        GWL_STYLE = -16,
        GWL_EXSTYLE = -20,
        GWL_USERDATA = -21
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MONITORINFO
    {
        public int cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public int dwFlags;

        public static MONITORINFO Create()
        {
            return new MONITORINFO() { cbSize = Marshal.SizeOf<MONITORINFO>() };
        }

        public enum MonitorOptions : uint
        {
            MONITOR_DEFAULTTONULL = 0x00000000,
            MONITOR_DEFAULTTOPRIMARY = 0x00000001,
            MONITOR_DEFAULTTONEAREST = 0x00000002
        }
    }

    public enum MONITOR_DPI_TYPE
    {
        MDT_EFFECTIVE_DPI = 0,
        MDT_ANGULAR_DPI = 1,
        MDT_RAW_DPI = 2,
        MDT_DEFAULT = MDT_EFFECTIVE_DPI
    }

}

