using Lantern.Platform;
using Lantern.Win32.Interop;

namespace Lantern.Win32;

internal partial class Win32DialogPlatform :IDialogPlatform
{
    public bool Ask(IWindowImpl? parent,string caption, string message)
    {
        var result = (NativeMethods.MESSAGEBOX_RESULT)NativeMethods.MessageBox(
            parent?.NativeHandle ?? 0,
            message, caption,
            (uint)(NativeMethods.MESSAGEBOX_STYLE.MB_YESNO));

        return result == NativeMethods.MESSAGEBOX_RESULT.IDYES;
    }

    public bool Confirm(IWindowImpl? parent, string caption, string message)
    {
        var result = (NativeMethods.MESSAGEBOX_RESULT)NativeMethods.MessageBox(
            parent?.NativeHandle ?? 0,
            message, caption,
            (uint)(NativeMethods.MESSAGEBOX_STYLE.MB_OKCANCEL));

        return result == NativeMethods.MESSAGEBOX_RESULT.IDOK;
    }

    public void Alert(IWindowImpl? parent, string caption, string message)
    {
        NativeMethods.MessageBox(
            parent?.NativeHandle ?? 0,
            message, caption,
            (uint)(NativeMethods.MESSAGEBOX_STYLE.MB_OK));
    }

}
