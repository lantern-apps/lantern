using MicroCom.Runtime;
using System.Runtime.InteropServices;
using static Lantern.Win32.Interop.NativeMethods;

namespace Lantern.Win32.Interop;

internal class NativeUtilities
{
    public unsafe static IntPtr LoadIcon(string? iconPath)
    {
        if (iconPath == null) return IntPtr.Zero;

        fixed (char* iconNamePtr = iconPath)
            return LoadImage(IntPtr.Zero, iconNamePtr, GdiImageType.IMAGE_ICON, 32, 32, ImageFlags.LR_LOADFROMFILE);
    }

    private static IntPtr s_taskBarList;
    private static HrInit? s_hrInitDelegate;
    private static MarkFullscreenWindow? s_markFullscreenWindowDelegate;

    public delegate void MarkFullscreenWindow(IntPtr This, IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fullscreen);
    public delegate HRESULT HrInit(IntPtr This);

    [DllImport("ole32.dll", PreserveSig = true)]
    internal static extern int CoCreateInstance(ref Guid clsid, IntPtr ignore1, int ignore2, ref Guid iid, [Out] out IntPtr pUnkOuter);

    internal unsafe static T CreateInstance<T>(ref Guid clsid, ref Guid iid) where T : IUnknown
    {
        var hresult = CoCreateInstance(ref clsid, IntPtr.Zero, 1, ref iid, out IntPtr pUnk);
        if (hresult != 0)
        {
            throw new COMException("CreateInstance", hresult);
        }
        using var unk = MicroComRuntime.CreateProxyFor<IUnknown>(pUnk, true);
        return MicroComRuntime.QueryInterface<T>(unk);
    }

    public static bool ShCoreAvailable => LoadLibrary("shcore.dll") != IntPtr.Zero;

    public enum HRESULT : uint
    {
        S_FALSE = 0x0001,
        S_OK = 0x0000,
        E_INVALIDARG = 0x80070057,
        E_OUTOFMEMORY = 0x8007000E,
        E_NOTIMPL = 0x80004001,
        E_UNEXPECTED = 0x8000FFFF,
        E_CANCELLED = 0x800704C7,
    }

    public static class ShellIds
    {
        public static readonly Guid OpenFileDialog = Guid.Parse("DC1C5A9C-E88A-4DDE-A5A1-60F82A20AEF7");
        public static readonly Guid SaveFileDialog = Guid.Parse("C0B4E2F3-BA21-4773-8DBA-335EC946EB8B");
        public static readonly Guid IFileDialog = Guid.Parse("42F85136-DB7E-439C-85F1-E4075D135FC8");
        public static readonly Guid IShellItem = Guid.Parse("43826D1E-E718-42EE-BC55-A1E261C37BFE");
        public static readonly Guid TaskBarList = Guid.Parse("56FDF344-FD6D-11D0-958A-006097C9A090");
        public static readonly Guid ITaskBarList2 = Guid.Parse("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf");
    }

    public struct ITaskBarList2VTable
    {
        public IntPtr IUnknown1;
        public IntPtr IUnknown2;
        public IntPtr IUnknown3;
        public IntPtr HrInit;
        public IntPtr AddTab;
        public IntPtr DeleteTab;
        public IntPtr ActivateTab;
        public IntPtr SetActiveAlt;
        public IntPtr MarkFullscreenWindow;
    }

    /// <summary>
    /// Ported from https://github.com/chromium/chromium/blob/master/ui/views/win/fullscreen_handler.cc
    /// </summary>
    /// <param name="hwnd">The window handle.</param>
    /// <param name="fullscreen">Fullscreen state.</param>
    public static unsafe void MarkFullscreen(IntPtr hwnd, bool fullscreen)
    {
        if (s_taskBarList == IntPtr.Zero)
        {
            Guid clsid = ShellIds.TaskBarList;
            Guid iid = ShellIds.ITaskBarList2;

            int result = CoCreateInstance(ref clsid, IntPtr.Zero, 1, ref iid, out s_taskBarList);

            if (s_taskBarList != IntPtr.Zero)
            {
                var ptr = (ITaskBarList2VTable**)s_taskBarList.ToPointer();

                s_hrInitDelegate ??= Marshal.GetDelegateForFunctionPointer<HrInit>((*ptr)->HrInit);

                if (s_hrInitDelegate(s_taskBarList) != HRESULT.S_OK)
                {
                    s_taskBarList = IntPtr.Zero;
                }
            }
        }

        if (s_taskBarList != IntPtr.Zero)
        {
            var ptr = (ITaskBarList2VTable**)s_taskBarList.ToPointer();

            if (s_markFullscreenWindowDelegate is null)
            {
                s_markFullscreenWindowDelegate = Marshal.GetDelegateForFunctionPointer<MarkFullscreenWindow>((*ptr)->MarkFullscreenWindow);
            }

            s_markFullscreenWindowDelegate(s_taskBarList, hwnd, fullscreen);
        }
    }

}
