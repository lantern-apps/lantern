using Lantern.Platform;
using Lantern.Win32.Interop;
using Lantern.Windows;
using static Lantern.Win32.Interop.NativeMethods;

namespace Lantern.Win32;

public partial class TrayIconImpl : ITrayIconImpl, IDisposable
{
#pragma warning disable CA1416 // 验证平台兼容性
    private static readonly IntPtr s_emptyIcon = new System.Drawing.Bitmap(32, 32).GetHicon();
#pragma warning restore CA1416 // 验证平台兼容性

    private int _id = 1;
    private static int s_nextUniqueId;

    //public static readonly TrayIconImpl Instance = new();

    private bool _visible;
    private bool _init;
    private IntPtr _icon;
    private string? _iconname;
    private string? _tooltip;

    internal TrayIconImpl(string? icon = null, string? tooltip = null)
    {
        _id = ++s_nextUniqueId;
        s_trayIcons.Add(_id, this);

        _tooltip = tooltip;
        if (icon != null)
        {
            _iconname = icon;
            _icon = NativeUtilities.LoadIcon(_iconname);
            _visible = true;
            Update();
        }
    }

    public Action? Click { get; set; }

    public Menu? Menu { get; set; }

    public Action? BallonClick { get; set; }

    public string? ToolTip
    {
        get => _tooltip;
        set
        {
            if (_tooltip != value)
            {
                _tooltip = value;
                Update();
            }
        }
    }

    public string? IconPath
    {
        get => _iconname;
        set
        {
            if (_iconname != value)
            {
                _iconname = value;
                _icon = NativeUtilities.LoadIcon(_iconname);
                Update();
            }
        }
    }


    public bool IsVisible
    {
        get => _visible;
        set
        {
            if (_visible != value)
            {
                _visible = value;
                Update();
            }
        }
    }

    private unsafe void Update()
    {
        var data = new NOTIFYICONDATA
        {
            hWnd = Win32Platform.Instance.Handle,
            uID = _id,
            uFlags = NIF.TIP | NIF.MESSAGE,
            uCallbackMessage = (int)WM_TRAYMOUSE,
            hIcon = _icon,
            szTip = _tooltip!,
        };

        if (_visible)
        {
            data.uFlags |= NIF.ICON;
            _ = Shell_NotifyIcon(_init ? NIM.MODIFY : NIM.ADD, data);
            _init = true;
        }
        else
        {
            _ = Shell_NotifyIcon(NIM.DELETE, data);
            _init = false;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
            IsVisible = false;
    }

    ~TrayIconImpl()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
