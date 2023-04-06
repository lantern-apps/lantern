using Calls;
using Lantern.Windows;

namespace Lantern.Messaging;

public abstract class TrayControllerBase
{
    [Callable(KnownMessageNames.TrayShow)] public abstract void Show(IpcContext context);
    [Callable(KnownMessageNames.TrayHide)] public abstract void Hide(IpcContext context);
    [Callable(KnownMessageNames.TrayIsVisible)] public abstract bool IsVisible(IpcContext context);
    [Callable(KnownMessageNames.TraySetTooltip)] public abstract void SetTooltip(IpcContext<string> context);
    [Callable(KnownMessageNames.TraySetMenu)] public abstract void SetMenu(IpcContext<MenuOptions> context);
}