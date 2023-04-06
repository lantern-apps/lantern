using Lantern.Platform;
using Lantern.Win32.Interop;
using System.Runtime.InteropServices;

namespace Lantern.Win32;

internal class ClipboardImpl : IClipboard
{
    private const int OleRetryCount = 10;
    private const int OleRetryDelay = 100;

    private static async Task<IDisposable> OpenClipboard()
    {
        var i = OleRetryCount;

        while (!NativeMethods.OpenClipboard(IntPtr.Zero))
        {
            if (--i == 0)
                throw new TimeoutException("Timeout opening clipboard.");
            await Task.Delay(OleRetryDelay);
        }

        return ClipboardDisposable.Instance;
    }

    private sealed class ClipboardDisposable : IDisposable
    {
        public static readonly ClipboardDisposable Instance = new();
        private ClipboardDisposable() { }
        public void Dispose() => NativeMethods.CloseClipboard();
    }

    public async Task<string?> GetTextAsync()
    {
        using (await OpenClipboard())
        {
            IntPtr hText = NativeMethods.GetClipboardData(NativeMethods.ClipboardFormat.CF_UNICODETEXT);
            if (hText == IntPtr.Zero)
            {
                return null;
            }

            var pText = NativeMethods.GlobalLock(hText);
            if (pText == IntPtr.Zero)
            {
                return null;
            }

            var rv = Marshal.PtrToStringUni(pText);
            NativeMethods.GlobalUnlock(hText);
            return rv;
        }
    }

    public async Task SetTextAsync(string text)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        using (await OpenClipboard())
        {
            NativeMethods.EmptyClipboard();

            var hGlobal = Marshal.StringToHGlobalUni(text);
            NativeMethods.SetClipboardData(NativeMethods.ClipboardFormat.CF_UNICODETEXT, hGlobal);
        }
    }

    public async Task ClearAsync()
    {
        using (await OpenClipboard())
        {
            NativeMethods.EmptyClipboard();
        }
    }
}