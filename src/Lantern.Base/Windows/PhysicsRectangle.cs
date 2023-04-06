using System.Text.Json.Serialization;

namespace Lantern.Windows;

public readonly struct PhysicsRectangle : IEquatable<PhysicsRectangle>
{
    [JsonConstructor]
    public PhysicsRectangle(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public PhysicsRectangle(PhysicsPosition position, PhysicsSize size)
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

    public LogisticRectangle ToLogisticRectangle(double scaleFactor) => new(
        (int)(X / scaleFactor),
        (int)(Y / scaleFactor),
        (int)(Width / scaleFactor),
        (int)(Height / scaleFactor));

    public bool Equals(PhysicsRectangle other) => X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
    public override bool Equals(object? obj) => obj is PhysicsRectangle rectangle && Equals(rectangle);
    public override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);
    public static bool operator ==(PhysicsRectangle left, PhysicsRectangle right) => left.Equals(right);
    public static bool operator !=(PhysicsRectangle left, PhysicsRectangle right) => !(left == right);
}