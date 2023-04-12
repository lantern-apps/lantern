using static Lantern.Win32.Interop.NativeMethods;

namespace Lantern.Win32;

public partial class TrayIconImpl
{
    public bool ShowNotification(string title, string? info)
    {
        const int NOTIFYICON_VERSION = 0x03;

        NOTIFYICONDATA data = new();
        data.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(data);
        data.uID = _id;
        data.hWnd = Win32Platform.Instance.Handle;
        data.uFlags = NIF.ICON | NIF.TIP | NIF.INFO | NIF.SHOWTIP | NIF.MESSAGE;
        data.uTimeoutOrVersion = 10 * 1000 | NOTIFYICON_VERSION;
        data.dwInfoFlags = NIIF.NONE;// NIIF.WARNING;
        data.hIcon = _icon == 0 ? s_emptyIcon : _icon;
        data.uCallbackMessage = (int)WM_TRAYMOUSE;
        data.szTip = _tooltip!;
        data.szInfoTitle = title;
        data.szInfo = info!;

        int result;
        if (_visible)
        {
            result = Shell_NotifyIcon(NIM.MODIFY, data);
        }
        else
        {
            result = Shell_NotifyIcon(_init ? NIM.MODIFY : NIM.ADD, data);
            Shell_NotifyIcon(NIM.DELETE, data);

        }

        return result != 0;
    }
}
