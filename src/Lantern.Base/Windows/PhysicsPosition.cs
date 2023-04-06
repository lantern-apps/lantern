using System.Text.Json.Serialization;

namespace Lantern.Windows;

public readonly struct PhysicsPosition : IEquatable<PhysicsPosition>
{
    [JsonConstructor]
    public PhysicsPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    [JsonPropertyName("x")]
    public int X { get; }

    [JsonPropertyName("y")]
    public int Y { get; }

    public LogisticPosition ToLogisticPosition(double scaleFactor) => new((int)(X / scaleFactor), (int)(Y / scaleFactor));

    public bool Equals(PhysicsPosition other) => X == other.X && Y == other.Y;

    public override bool Equals(object? obj) => obj is PhysicsPosition position && Equals(position);

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static bool operator ==(PhysicsPosition left, PhysicsPosition right) => left.Equals(right);

    public static bool operator !=(PhysicsPosition left, PhysicsPosition right) => !(left == right);
}
