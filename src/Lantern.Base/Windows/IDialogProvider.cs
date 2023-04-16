namespace Lantern.Windows;

public interface IDialogProvider
{
    void Alert(IWindow? parent, string title, string body, MessageIconType iconType = MessageIconType.None);
    bool Ask(IWindow? parent, string title, string body, MessageIconType iconType = MessageIconType.None);
    bool Confirm(IWindow? parent, string title, string body, MessageIconType iconType = MessageIconType.None);

    Task<string[]> OpenFileAsync(IWindow? parent, OpenFileDialogOptions options);
    Task<string?> SaveFileAsync(IWindow? parent, SaveFileDialogOptions options);
    Task<string[]> OpenFolderAsync(IWindow? parent, OpenFolderDialogOptions options);
}

