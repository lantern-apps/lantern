namespace Lantern.Windows;

public interface IDialogProvider
{
    void Alert(IWindow? parent, string title, string body);
    bool Ask(IWindow? parent, string title, string body);
    bool Confirm(IWindow? parent, string title, string body);

    Task<string[]> OpenFileAsync(IWindow? parent, OpenFileDialogOptions options);
    Task<string?> SaveFileAsync(IWindow? parent, SaveFileDialogOptions options);
    Task<string[]> OpenFolderAsync(IWindow? parent, OpenFolderDialogOptions options);
}

