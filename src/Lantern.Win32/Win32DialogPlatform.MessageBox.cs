using Lantern.Platform;
using Lantern.Win32.Interop;
using Lantern.Windows;

namespace Lantern.Win32;

internal partial class Win32DialogPlatform : IDialogPlatform
{
    public bool Ask(IWindowImpl? parent, string caption, string message, MessageIconType iconType = MessageIconType.None)
    {
        uint icon = GetIcon(iconType);

        var result = (NativeMethods.MESSAGEBOX_RESULT)NativeMethods.MessageBox(
            parent?.NativeHandle ?? 0,
            message, caption,
            (uint)(NativeMethods.MESSAGEBOX_STYLE.MB_YESNO) | icon);

        return result == NativeMethods.MESSAGEBOX_RESULT.IDYES;
    }


    public bool Confirm(IWindowImpl? parent, string caption, string message, MessageIconType iconType = MessageIconType.None)
    {
        uint icon = GetIcon(iconType);

        var result = (NativeMethods.MESSAGEBOX_RESULT)NativeMethods.MessageBox(
            parent?.NativeHandle ?? 0,
            message, caption,
            (uint)(NativeMethods.MESSAGEBOX_STYLE.MB_OKCANCEL) | icon);

        return result == NativeMethods.MESSAGEBOX_RESULT.IDOK;
    }

    public void Alert(IWindowImpl? parent, string caption, string message, MessageIconType iconType = MessageIconType.None)
    {
        uint icon = GetIcon(iconType);

        NativeMethods.MessageBox(
            parent?.NativeHandle ?? 0,
            message, caption,
            (uint)(NativeMethods.MESSAGEBOX_STYLE.MB_OK) | icon);
    }

    private static uint GetIcon(MessageIconType iconType) => iconType switch
    {
        MessageIconType.Error => (uint)NativeMethods.MESSAGEBOX_STYLE.MB_ICONERROR,
        MessageIconType.Information => (uint)NativeMethods.MESSAGEBOX_STYLE.MB_ICONINFORMATION,
        MessageIconType.Waring => (uint)NativeMethods.MESSAGEBOX_STYLE.MB_ICONWARNING,
        MessageIconType.Question => (uint)NativeMethods.MESSAGEBOX_STYLE.MB_ICONQUESTION,
        _ => 0u,
    };
}
