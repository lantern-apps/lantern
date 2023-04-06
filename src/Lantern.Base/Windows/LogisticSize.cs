using System.Text.Json.Serialization;

namespace Lantern.Windows;

public readonly struct LogisticSize : IEquatable<LogisticSize>
{
    [JsonConstructor]
    public LogisticSize(int width, int height)
    {
        Width = width;
        Height = height;
    }

    [JsonPropertyName("width")]
    public int Width { get; }

    [JsonPropertyName("height")]
    public int Height { get; }

    public PhysicsSize ToPhysicsSize(double scaleFactor) => new((int)(Width * scaleFactor), (int)(Height * scaleFactor));

    public bool Equals(LogisticSize other) => Width == other.Width && Height == other.Height;

    public override bool Equals(object? obj) => obj is LogisticSize size && Equals(size);

    public override int GetHashCode() => HashCode.Combine(Width, Height);

    public static bool operator ==(LogisticSize left, LogisticSize right) => left.Equals(right);

    public static bool operator !=(LogisticSize left, LogisticSize right) => !(left == right);
}
