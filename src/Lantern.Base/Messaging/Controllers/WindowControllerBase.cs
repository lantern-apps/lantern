using Calls;
using Lantern.Windows;
using System.Text.Json.Serialization;

namespace Lantern.Messaging;

public abstract class WindowControllerBase
{
    [Callable(KnownMessageNames.WindowGetAll)] public abstract string[] GetAll(IpcContext context);
    [Callable(KnownMessageNames.WindowCreate)] public abstract Task Create(IpcContext<WindowCreateOptions> context);

    [Callable(KnownMessageNames.WindowRestore)] public abstract void Restore(IpcContext context);
    [Callable(KnownMessageNames.WindowMaximize)] public abstract void Maximize(IpcContext context);
    [Callable(KnownMessageNames.WindowMinimize)] public abstract void Minimize(IpcContext context);
    [Callable(KnownMessageNames.WindowFullScreen)] public abstract void FullScreen(IpcContext context);
    [Callable(KnownMessageNames.WindowClose)] public abstract void Close(IpcContext context);
    [Callable(KnownMessageNames.WindowHide)] public abstract void Hide(IpcContext context);
    [Callable(KnownMessageNames.WindowShow)] public abstract void Show(IpcContext context);
    [Callable(KnownMessageNames.WindowActivate)] public abstract void Activate(IpcContext context);
    [Callable(KnownMessageNames.WindowCenter)] public abstract void Center(IpcContext context);

    [Callable(KnownMessageNames.WindowStartDragMove)] public abstract void StartDragMove(IpcContext context);

    [Callable(KnownMessageNames.WindowSetAlwaysOnTop)] public abstract void SetAlwaysOnTop(IpcContext<bool> context);
    [Callable(KnownMessageNames.WindowSetSize)] public abstract void SetSize(IpcContext<LogisticSize> context);
    [Callable(KnownMessageNames.WindowSetMinSize)] public abstract void SetMinSize(IpcContext<LogisticSize> context);
    [Callable(KnownMessageNames.WindowSetMaxSize)] public abstract void SetMaxSize(IpcContext<LogisticSize> context);
    [Callable(KnownMessageNames.WindowSetPosition)] public abstract void SetPosition(IpcContext<LogisticPosition> context);
    [Callable(KnownMessageNames.WindowSetTitle)] public abstract void SetTitle(IpcContext<string> context);
    [Callable(KnownMessageNames.WindowSetUrl)] public abstract void SetUrl(IpcContext<string> context);

    [Callable(KnownMessageNames.WindowIsMaximized)] public abstract bool IsMaximized(IpcContext context);
    [Callable(KnownMessageNames.WindowIsMinimized)] public abstract bool IsMinimized(IpcContext context);
    [Callable(KnownMessageNames.WindowIsFullScreen)] public abstract bool IsFullScreen(IpcContext context);

    [Callable(KnownMessageNames.WindowIsAlwaysOnTop)] public abstract bool IsAlwaysOnTop(IpcContext context);
    [Callable(KnownMessageNames.WindowIsVisible)] public abstract bool IsVisible(IpcContext context);
    [Callable(KnownMessageNames.WindowIsActive)] public abstract bool IsActive(IpcContext context);
    [Callable(KnownMessageNames.WindowGetSize)] public abstract LogisticSize GetSize(IpcContext context);
    [Callable(KnownMessageNames.WindowGetPosition)] public abstract LogisticPosition GetPosition(IpcContext context);
    [Callable(KnownMessageNames.WindowGetTitle)] public abstract string? GetTitle(IpcContext context);
    [Callable(KnownMessageNames.WindowGetUrl)] public abstract Task<string?> GetUrl(IpcContext context);
    [Callable(KnownMessageNames.WindowGetMinSize)] public abstract LogisticSize GetMinSize(IpcContext context);
    [Callable(KnownMessageNames.WindowGetMaxSize)] public abstract LogisticSize GetMaxSize(IpcContext context);


    [Callable(KnownMessageNames.WindowIsSkipTaskbar)] public abstract bool IsSkipTaskbar(IpcContext context);
    [Callable(KnownMessageNames.WindowSetSkipTaskbar)] public abstract void SetSkipTaskbar(IpcContext<bool> context);

    [Callable(KnownMessageNames.WindowIsResizable)] public abstract bool IsResizable(IpcContext context);
    [Callable(KnownMessageNames.WindowSetResizable)] public abstract void SetResizable(IpcContext<bool> context);

    [Callable(KnownMessageNames.WindowGetTitleBarStyle)] public abstract WindowTitleBarStyle GetTitleBarStyle(IpcContext context);
    [Callable(KnownMessageNames.WindowSetTitleBarStyle)] public abstract void SetTitleBarStyle(IpcContext<WindowTitleBarStyle> context);
}

public class WindowCreateOptions
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("visible")]
    public bool Visible { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("width")]
    public int? Width { get; set; }

    [JsonPropertyName("height")]
    public int? Height { get; set; }

    [JsonPropertyName("x")]
    public int? X { get; set; }

    [JsonPropertyName("y")]
    public int? Y { get; set; }

    [JsonPropertyName("minWidth")]
    public int? MinWidth { get; set; }

    [JsonPropertyName("minheight")]
    public int? MinHeight { get; set; }

    [JsonPropertyName("maxWidth")]
    public int? MaxWidth { get; set; }

    [JsonPropertyName("maxheight")]
    public int? MaxHeight { get; set; }

    [JsonPropertyName("center")]
    public bool Center { get; set; } = false;

    [JsonPropertyName("alwaysOnTop")]
    public bool AlwaysOnTop { get; set; } = false;

    [JsonPropertyName("skipTaskbar")]
    public bool SkipTaskbar { get; set; } = false;

    [JsonPropertyName("resizeable")]
    public bool Resizeable { get; set; } = true;

    [JsonPropertyName("titleBarStyle")]
    public WindowTitleBarStyle TitleBarStyle { get; set; } = WindowTitleBarStyle.Default;

}