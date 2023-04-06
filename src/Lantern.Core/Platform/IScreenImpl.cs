using Lantern.Windows;

namespace Lantern.Platform;

public interface IScreenImpl
{
    int ScreenCount { get; }
    Screen[] AllScreens { get; }
    Screen? ScreenFromPoint(PhysicsPosition point);
    Screen? ScreenFromRect(PhysicsRectangle rect);
    Screen? ScreenFromWindow(IWindowImpl window);
}