using System.Diagnostics.CodeAnalysis;

namespace Lantern.AsService;

internal class ThrowHelper
{
    [DoesNotReturn]
    public static void ThrowInvaildUrlOrPredicate() => throw new();
}
