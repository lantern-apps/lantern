using System.Buffers;

namespace Lantern.Aus.Internal;

internal readonly struct PooledBuffer(int minimumLength) : IDisposable
{
    public static PooledBuffer ForStream() => new(81920);

    public byte[] Array { get; } = ArrayPool<byte>.Shared.Rent(minimumLength);

    public void Dispose() => ArrayPool<byte>.Shared.Return(Array);
}

