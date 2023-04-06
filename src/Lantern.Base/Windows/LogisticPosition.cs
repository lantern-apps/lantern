using System.Text.Json.Serialization;

namespace Lantern.Windows;

public readonly struct LogisticPosition : IEquatable<LogisticPosition>
{
    [JsonConstructor]
    public LogisticPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    [JsonPropertyName("x")]
    public int X { get; }

    [JsonPropertyName("y")]
    public int Y { get; }

    public PhysicsPosition ToPhysicsPosition(double scaleFactor) => new((int)(X * scaleFactor), (int)(Y * scaleFactor));

    public bool Equals(LogisticPosition other) => X == other.X && Y == other.Y;

    public override bool Equals(object? obj) => obj is LogisticPosition position && Equals(position);

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static bool operator ==(LogisticPosition left, LogisticPosition right) => left.Equals(right);

    public static bool operator !=(LogisticPosition left, LogisticPosition right) => !(left == right);
}
