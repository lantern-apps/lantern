using Lantern.Messaging;
using Lantern.Platform;

namespace Lantern.Controllers;

public class ClipboardController : ClipboardControllerBase
{
    private readonly IClipboard _clipboard;

    public ClipboardController(IClipboard clipboard)
    {
        _clipboard = clipboard;
    }

    public override Task<string?> GetText(IpcContext context)
    {
        return _clipboard.GetTextAsync();
    }

    public override Task SetText(IpcContext<string> context)
    {
        if (context.Body == null)
            return _clipboard.ClearAsync();
        else
            return _clipboard.SetTextAsync(context.Body);
    }
}
