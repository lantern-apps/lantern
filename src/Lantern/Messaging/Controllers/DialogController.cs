using Lantern.Messaging;
using Lantern.Windows;

namespace Lantern.Controllers;

public class DialogController : DialogControllerBase
{
    private readonly IDialogProvider _dialogManager;

    public DialogController(IDialogProvider dialogManager)
    {
        _dialogManager = dialogManager;
    }

    public override void Message(IpcContext<MessageDialogOptions> context)
    {
        var options = context.Body;
        var title = options.Title ?? context.Window.Title;
        _dialogManager.Alert(context.Window, title!, options.Body ?? string.Empty);
    }

    public override bool Ask(IpcContext<MessageDialogOptions> context)
    {
        var options = context.Body;
        var title = options.Title ?? context.Window.Title;
        return _dialogManager.Ask(context.Window, title!, options.Body ?? string.Empty);
    }

    public override bool Confirm(IpcContext<MessageDialogOptions> context)
    {
        var options = context.Body;
        var title = options.Title ?? context.Window.Title;
        return _dialogManager.Confirm(context.Window, title!, options.Body ?? string.Empty);
    }

    public override async Task<string[]> OpenFolder(IpcContext<OpenFolderDialogOptions> context)
    {
        return await _dialogManager.OpenFolderAsync(context.Window, context.Body);
    }

    public override async Task<string[]> OpenFile(IpcContext<OpenFileDialogOptions> context)
    {
        return await _dialogManager.OpenFileAsync(context.Window, context.Body);
    }

    public override async Task<string?> SaveFile(IpcContext<SaveFileDialogOptions> context)
    {
        return await _dialogManager.SaveFileAsync(context.Window, context.Body);
    }
}