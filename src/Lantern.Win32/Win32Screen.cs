using Lantern.Windows;

namespace Lantern.Win32;

public class Win32Screen : Screen
{
    private readonly IntPtr _hMonitor;

    public Win32Screen(double scaling, PhysicsRectangle bounds, PhysicsRectangle workingArea, bool isPrimary, IntPtr hMonitor)
        : base(scaling, bounds, workingArea, isPrimary)
    {
        _hMonitor = hMonitor;
    }

    public IntPtr Handle => _hMonitor;

}
