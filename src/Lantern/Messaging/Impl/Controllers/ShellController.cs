using System.Diagnostics;
using Lantern.Messaging;

namespace Lantern.Controllers;

public class ShellController : ShellControllerBase
{
    public override void Open(IpcContext<ShellOpenOptions> context)
    {
        var path = context.Body.Path;
        if (path == null)
        {
            ThrowHelper.ThrowIpcRequestArgumentNullException(nameof(context.Body.Path));
        }

        if (Path.IsPathFullyQualified(path))
        {
            if (!File.Exists(path))
            {
                ThrowHelper.ThrowFileNotFoundException(path);
            }

            using var _ = Process.Start(new ProcessStartInfo(path)
            {
                UseShellExecute = true
            });
        }
        else if (Uri.IsWellFormedUriString(path, UriKind.Absolute))
        {
            using var _ = Process.Start(new ProcessStartInfo(path)
            {
                UseShellExecute = true
            });
        }
        else
        {
            ThrowHelper.ThrowIpcRequestArgumentException(nameof(context.Body.Path), "The argument 'Path' must be local file path or url");
        }
    }
}
