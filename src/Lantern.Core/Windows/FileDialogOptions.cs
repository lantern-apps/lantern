using System.Text.Json.Serialization;

namespace Lantern.Windows;

public abstract class FileDialogOptions
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("defaultPath")]
    public string? DefaultPath { get; set; }
}

public sealed class OpenFolderDialogOptions : FileDialogOptions
{
    [JsonPropertyName("multiple")]
    public bool Multiple { get; set; }
}

public sealed class OpenFileDialogOptions : FileDialogOptions
{
    [JsonPropertyName("multiple")]
    public bool Multiple { get; set; }

    [JsonPropertyName("filters")]
    public DialogFilter[]? Filters { get; set; }
}

public sealed class SaveFileDialogOptions : FileDialogOptions
{

    [JsonPropertyName("filters")]
    public DialogFilter[]? Filters { get; set; }
}

public class DialogFilter
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    //without . prefix
    [JsonPropertyName("extensions")]
    public required string[] Extensions { get; set; }
}