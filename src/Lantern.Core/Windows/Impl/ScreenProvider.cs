using Lantern.Platform;

namespace Lantern.Windows;

public class ScreenProvider : IScreenProvider
{
    private readonly IWindowingPlatform _windowingPlatform;

    public ScreenProvider(IWindowingPlatform windowingPlatform)
    {
        _windowingPlatform = windowingPlatform;
    }

    public Screen[] Screens => _windowingPlatform.Screen.AllScreens;
}
