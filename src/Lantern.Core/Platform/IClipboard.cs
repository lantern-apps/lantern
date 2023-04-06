namespace Lantern.Platform;

public interface IClipboard
{
    Task<string?> GetTextAsync();
    Task SetTextAsync(string text);
    Task ClearAsync();
}
