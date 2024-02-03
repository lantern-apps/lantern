using Calls;
using Lantern.Windows;
using System.Text.Json.Serialization;

namespace Lantern.Messaging;

public abstract class ScreenControllerBase
{
    [Callable(KnownMessageNames.ScreenGetAll)] 
    public abstract ScreenDto[] All(IpcContext context);

    [Callable(KnownMessageNames.ScreenGetCurrent)] 
    public abstract ScreenDto? Current(IpcContext context);
}

public class ScreenDto
{
    [JsonPropertyName("scaleFactor")]
    public double ScaleFactor { get; set; }

    [JsonPropertyName("bounds")]
    public LogisticRectangle? Bounds { get; set; }

    [JsonPropertyName("workingArea")]
    public LogisticRectangle? WorkingArea { get; set; }

    [JsonPropertyName("primary")]
    public bool Primary { get; set; }
}