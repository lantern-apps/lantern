using Calls;
using Lantern.Windows;
using System.Text.Json.Serialization;

namespace Lantern.Messaging;

public abstract class DialogControllerBase
{
    [Callable(KnownMessageNames.DialogMessage)] public abstract void Message(IpcContext<MessageDialogOptions> context);
    [Callable(KnownMessageNames.DialogAsk)] public abstract bool Ask(IpcContext<MessageDialogOptions> context);
    [Callable(KnownMessageNames.DialogConfirm)] public abstract bool Confirm(IpcContext<MessageDialogOptions> context);
    [Callable(KnownMessageNames.DialogOpenFile)] public abstract Task<string[]> OpenFile(IpcContext<OpenFileDialogOptions> context);
    [Callable(KnownMessageNames.DialogOpenFolder)] public abstract Task<string[]> OpenFolder(IpcContext<OpenFolderDialogOptions> context);
    [Callable(KnownMessageNames.DialogSaveFile)] public abstract Task<string?> SaveFile(IpcContext<SaveFileDialogOptions> context);
}

public class MessageDialogOptions
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("body")]
    public string? Body { get; set; }
}

