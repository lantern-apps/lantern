using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Lantern;

internal static class ThrowHelper
{
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowArgumentNullException(string paramName) => throw new ArgumentNullException(paramName, $"Argument '{paramName}' cannot be null.");

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowMissCommandNameAttributeException() => throw new Exception();

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIpcRequestUnregistedException(string requestName) => throw new Exception();

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIpcRequestEmptyNameException() => throw new Exception();

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowTimoutException() => throw new Exception();

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIpcRequestArgumentNullException(string paramName) => throw new ArgumentNullException(paramName);

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIpcRequestArgumentException(string paramName, string message) => throw new ArgumentException(message, paramName);

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowFileNotFoundException(string fileName) => throw new FileNotFoundException($"File Not found '{fileName}'.");

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowDirectoryNotFoundException(string directoryPath) => throw new FileNotFoundException($"Directory Not found '{directoryPath}'.");

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowInvaidFileNameException(string fileName) => throw new IOException($"Invaild file path '{fileName}'.");

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowInvaidDirectoryPathException(string directoryPath) => throw new IOException($"Invaild directory path '{directoryPath}'.");
}
