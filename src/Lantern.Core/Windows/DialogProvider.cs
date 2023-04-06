using Lantern.Platform;

namespace Lantern.Windows;

public class DialogProvider : IDialogProvider
{
    private readonly IDialogPlatform _dialogProvider;

    public DialogProvider(IDialogPlatform dialogProvider)
    {
        _dialogProvider = dialogProvider;
    }

    public void Alert(IWindow? parent, string title, string body)
    {
        _dialogProvider.Alert(parent == null ? null : ((Window)parent).WindowImpl, title, body);
    }

    public bool Ask(IWindow? parent, string title, string body)
    {
        return _dialogProvider.Ask(parent == null ? null : ((Window)parent).WindowImpl, title, body);
    }

    public bool Confirm(IWindow? parent, string title, string body)
    {
        return _dialogProvider.Confirm(parent == null ? null : ((Window)parent).WindowImpl, title, body);
    }

    public Task<string[]> OpenFileAsync(IWindow? parent, OpenFileDialogOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.DefaultPath))
        {
            ValidationHelper.ValidateDirectoryExists(options.DefaultPath);
        }

        return _dialogProvider.OpenFileAsync(parent == null ? null : ((Window)parent).WindowImpl, new FilePickerOpenOptions
        {
            Title = options.Title,
            AllowMultiple = options.Multiple,
            SuggestedStartLocation = options.DefaultPath,
            FileTypeFilter = options.Filters?.Select(x => new FilePickerFileType(x.Name)
            {
                Patterns = x.Extensions.Select(extension => extension.StartsWith('.') ? extension : "*." + extension).ToList()
            })?.ToList()
        });
    }

    public Task<string[]> OpenFolderAsync(IWindow? parent, OpenFolderDialogOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.DefaultPath))
        {
            ValidationHelper.ValidateDirectoryExists(options.DefaultPath);
        }

        return _dialogProvider.OpenFolderAsync(parent == null ? null : ((Window)parent).WindowImpl, new FolderPickerOpenOptions
        {
            Title = options.Title,
            AllowMultiple = options.Multiple,
            SuggestedStartLocation = options.DefaultPath,
        });
    }

    public Task<string?> SaveFileAsync(IWindow? parent, SaveFileDialogOptions options)
    {
        string? defaultExtension = null;
        string? defaultFileName = null;
        string? startLocation = null;

        if (!string.IsNullOrWhiteSpace(options.DefaultPath))
        {
            options.DefaultPath = ValidationHelper.ValidatePathQualified(options.DefaultPath);

            defaultExtension = Path.GetExtension(options.DefaultPath);

            if (defaultExtension == null)
            {
                startLocation = options.DefaultPath;
            }
            else
            {
                defaultFileName = Path.GetFileName(options.DefaultPath);
                startLocation = Path.GetDirectoryName(options.DefaultPath)!;

                ValidationHelper.ValidateDirectoryExists(startLocation);
            }
        }

        return _dialogProvider.SaveFileAsync(parent == null ? null : ((Window)parent).WindowImpl, new FilePickerSaveOptions
        {
            Title = options.Title,
            SuggestedStartLocation = startLocation,
            DefaultExtension = defaultExtension,
            SuggestedFileName = defaultFileName,
            FileTypeChoices = options.Filters?.Select(x => new FilePickerFileType(x.Name)
            {
                Patterns = x.Extensions.Select(extension => extension.StartsWith('.') ? extension : "*." + extension).ToList()
            })?.ToList()
        });
    }
}
