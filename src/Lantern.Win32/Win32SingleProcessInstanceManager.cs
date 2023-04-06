using Lantern.Platform;
using Lantern.Win32.Interop;
using System.Diagnostics;

namespace Lantern.Win32;

public class Win32SingleProcessInstanceManager : ISingleProcessInstanceManager, IDisposable
{
    private static Mutex? _mutex;
    private static readonly string _processName = Process.GetCurrentProcess().ProcessName;

    public void ActivateOtherProcessMainWindow()
    {
        var processes = Process.GetProcessesByName(_processName);
        foreach (Process p in processes)
        {
            if (Environment.ProcessId == p.Id)
                continue;

            if (0 != p.MainWindowHandle)
            {
                if (NativeMethods.IsIconic(p.MainWindowHandle) != 0)
                    NativeMethods.ShowWindow(p.MainWindowHandle, NativeMethods.ShowWindowCommand.Restore);

                NativeMethods.SetForegroundWindow(p.MainWindowHandle);
            }
            return;
        }
    }

    public bool Lock()
    {
        if (_mutex != null)
            return false;

        var mutex = new Mutex(true, "Lantern", out bool prevInstance);
        if (prevInstance)
            _mutex = mutex;

        return prevInstance;
    }

    public void Dispose() => _mutex?.Dispose();

}