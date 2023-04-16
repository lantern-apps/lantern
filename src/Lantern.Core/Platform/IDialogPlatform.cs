using Lantern.Windows;

namespace Lantern.Platform;

public interface IDialogPlatform
{
    void Alert(IWindowImpl? parent, string caption, string message, MessageIconType iconType = MessageIconType.None);
    bool Ask(IWindowImpl? parent, string caption, string message, MessageIconType iconType = MessageIconType.None);
    bool Confirm(IWindowImpl? parent, string caption, string message, MessageIconType iconType = MessageIconType.None);

    Task<string[]> OpenFileAsync(IWindowImpl? parent, FilePickerOpenOptions options);
    Task<string?> SaveFileAsync(IWindowImpl? parent, FilePickerSaveOptions options);
    Task<string[]> OpenFolderAsync(IWindowImpl? parent, FolderPickerOpenOptions options);
}

public class PickerOptions
{
    /// <summary>
    /// Gets or sets the text that appears in the title bar of a folder dialog.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the initial location where the file open picker looks for files to present to the user.
    /// </summary>
    public string? SuggestedStartLocation { get; set; }
}

/// <summary>
/// Options class for <see cref="IDialogPlatform.OpenFileAsync"/> method.
/// </summary>
public class FilePickerOpenOptions : PickerOptions
{
    /// <summary>
    /// Gets or sets an option indicating whether open picker allows users to select multiple files.
    /// </summary>
    public bool AllowMultiple { get; set; }

    /// <summary>
    /// Gets or sets the collection of file types that the file open picker displays.
    /// </summary>
    public IReadOnlyList<FilePickerFileType>? FileTypeFilter { get; set; }
}

public sealed class FilePickerFileType
{
    public FilePickerFileType(string name)
    {
        Name = name;
    }

    /// <summary>
    /// File type name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// List of extensions in GLOB format. I.e. "*.png" or "*.*".
    /// </summary>
    /// <remarks>
    /// Used on Windows and Linux systems.
    /// </remarks>
    public IReadOnlyList<string>? Patterns { get; set; }

    /// <summary>
    /// List of extensions in MIME format.
    /// </summary>
    /// <remarks>
    /// Used on Android, Browser and Linux systems.
    /// </remarks>
    public IReadOnlyList<string>? MimeTypes { get; set; }

    /// <summary>
    /// List of extensions in Apple uniform format.
    /// </summary>
    /// <remarks>
    /// Used only on Apple devices.
    /// See https://developer.apple.com/documentation/uniformtypeidentifiers/system_declared_uniform_type_identifiers.
    /// </remarks>
    public IReadOnlyList<string>? AppleUniformTypeIdentifiers { get; set; }
}

public class FolderPickerOpenOptions : PickerOptions
{
    /// <summary>
    /// Gets or sets an option indicating whether open picker allows users to select multiple folders.
    /// </summary>
    public bool AllowMultiple { get; set; }
}

public static class FilePickerFileTypes
{
    public static FilePickerFileType All { get; } = new("All")
    {
        Patterns = new[] { "*.*" },
        MimeTypes = new[] { "*/*" }
    };

    public static FilePickerFileType TextPlain { get; } = new("Plain Text")
    {
        Patterns = new[] { "*.txt" },
        AppleUniformTypeIdentifiers = new[] { "public.plain-text" },
        MimeTypes = new[] { "text/plain" }
    };

    public static FilePickerFileType ImageAll { get; } = new("All Images")
    {
        Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.gif", "*.bmp" },
        AppleUniformTypeIdentifiers = new[] { "public.image" },
        MimeTypes = new[] { "image/*" }
    };

    public static FilePickerFileType ImageJpg { get; } = new("JPEG image")
    {
        Patterns = new[] { "*.jpg", "*.jpeg" },
        AppleUniformTypeIdentifiers = new[] { "public.jpeg" },
        MimeTypes = new[] { "image/jpeg" }
    };

    public static FilePickerFileType ImagePng { get; } = new("PNG image")
    {
        Patterns = new[] { "*.png" },
        AppleUniformTypeIdentifiers = new[] { "public.png" },
        MimeTypes = new[] { "image/png" }
    };

    public static FilePickerFileType Pdf { get; } = new("PDF document")
    {
        Patterns = new[] { "*.pdf" },
        AppleUniformTypeIdentifiers = new[] { "com.adobe.pdf" },
        MimeTypes = new[] { "application/pdf" }
    };
}

public class FilePickerSaveOptions : PickerOptions
{
    /// <summary>
    /// Gets or sets the file name that the file save picker suggests to the user.
    /// </summary>
    public string? SuggestedFileName { get; set; }

    /// <summary>
    /// Gets or sets the default extension to be used to save the file.
    /// </summary>
    public string? DefaultExtension { get; set; }

    /// <summary>
    /// Gets or sets the collection of valid file types that the user can choose to assign to a file.
    /// </summary>
    public IReadOnlyList<FilePickerFileType>? FileTypeChoices { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether file open picker displays a warning if the user specifies the name of a file that already exists.
    /// </summary>
    public bool? ShowOverwritePrompt { get; set; }
}