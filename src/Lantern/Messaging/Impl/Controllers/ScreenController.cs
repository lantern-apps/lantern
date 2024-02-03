using Lantern.Messaging;
using Lantern.Windows;

namespace Lantern.Controllers;

public sealed class ScreenController : ScreenControllerBase
{
    private readonly IScreenProvider _screenProvider;

    public ScreenController(IScreenProvider screenProvider)
    {
        _screenProvider = screenProvider;
    }

    public override ScreenDto[] All(IpcContext context)
    {
        return _screenProvider.Screens.Select(screen => new ScreenDto
        {
            ScaleFactor = screen.Scaling,
            Bounds = screen.Bounds,
            WorkingArea = screen.WorkingArea,
            Primary = screen.IsPrimary,
        }).ToArray();
    }

    public override ScreenDto? Current(IpcContext context)
    {
        var screen = context.Window.GetScreen();
        if (screen == null)
            return null;

        return new ScreenDto
        {
            ScaleFactor = screen.Scaling,
            Bounds = screen.Bounds,
            WorkingArea = screen.WorkingArea,
            Primary = screen.IsPrimary,
        };
    }
}
