using System.Text.Json.Serialization;

namespace Lantern.Windows;

public readonly struct PhysicsSize : IEquatable<PhysicsSize>
{
    [JsonConstructor]
    public PhysicsSize(int width, int height)
    {
        Width = width;
        Height = height;
    }

    [JsonPropertyName("width")]
    public int Width { get; }

    [JsonPropertyName("height")]
    public int Height { get; }

    public LogisticSize ToLogisticSize(double scaleFactor) => new((int)(Width / scaleFactor), (int)(Height / scaleFactor));

    public bool Equals(PhysicsSize other) => Width == other.Width && Height == other.Height;

    public override bool Equals(object? obj) => obj is PhysicsSize size && Equals(size);

    public override int GetHashCode() => HashCode.Combine(Width, Height);

    public static bool operator ==(PhysicsSize left, PhysicsSize right) => left.Equals(right);

    public static bool operator !=(PhysicsSize left, PhysicsSize right) => !(left == right);
}