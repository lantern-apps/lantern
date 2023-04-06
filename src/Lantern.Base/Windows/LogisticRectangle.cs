using System.Text.Json.Serialization;

namespace Lantern.Windows;

public readonly struct LogisticRectangle : IEquatable<LogisticRectangle>
{
    [JsonConstructor]
    public LogisticRectangle(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public LogisticRectangle(LogisticPosition position, LogisticSize size)
    {
        X = position.X;
        Y = position.Y;
        Width = size.Width;
        Height = size.Height;
    }

    [JsonPropertyName("x")]
    public int X { get; }

    [JsonPropertyName("y")]
    public int Y { get; }

    [JsonPropertyName("width")]
    public int Width { get; }

    [JsonPropertyName("height")]
    public int Height { get; }

    public PhysicsRectangle ToPhysicsRectangle(double scaleFactor) => new(
        (int)(X * scaleFactor),
        (int)(Y * scaleFactor),
        (int)(Width * scaleFactor),
        (int)(Height * scaleFactor));

    public bool Equals(LogisticRectangle other) => X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
    public override bool Equals(object? obj) => obj is LogisticRectangle rectangle && Equals(rectangle);
    public override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);
    public static bool operator ==(LogisticRectangle left, LogisticRectangle right) => left.Equals(right);
    public static bool operator !=(LogisticRectangle left, LogisticRectangle right) => !(left == right);

}
