using Lantern.Messaging;
using Lantern.Windows;

namespace Lantern.Controllers;

internal sealed class TrayController : TrayControllerBase
{
    private readonly ITrayManager _trayManager;
    private ITray? _tray;

    public TrayController(ITrayManager trayIconManager)
    {
        _trayManager = trayIconManager;
    }

    private ITray GetDefaultTray() => _tray ??= _trayManager.GetDefaultTray() ?? throw new Exception();

    public override bool IsVisible(IpcContext context) => GetDefaultTray().Visible;

    public override void SetTooltip(IpcContext<string> context) => GetDefaultTray().ToolTip = context.Body;

    public override void Show(IpcContext context) => GetDefaultTray().Visible = true;

    public override void Hide(IpcContext context) => GetDefaultTray().Visible = false;

    public override void SetMenu(IpcContext<MenuOptions> context) => GetDefaultTray().Menu = context.Body?.Build();
}
