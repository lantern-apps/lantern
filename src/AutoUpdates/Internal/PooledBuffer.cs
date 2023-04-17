using System.Buffers;

namespace Lantern.Aus.Internal;

internal readonly struct PooledBuffer : IDisposable
{
    public static PooledBuffer ForStream() => new(81920);

    public byte[] Array { get; }
    public PooledBuffer(int minimumLength) => Array = ArrayPool<byte>.Shared.Rent(minimumLength);
    public void Dispose() => ArrayPool<byte>.Shared.Return(Array);
}

