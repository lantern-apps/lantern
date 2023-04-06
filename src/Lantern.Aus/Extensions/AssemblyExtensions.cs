using System.Reflection;

namespace Lantern.Aus;

internal static class AssemblyExtensions
{
    public static async Task ExtractManifestResourceAsync(
        this Assembly assembly,
        string resourceName,
        string destFilePath)
    {
        var input = assembly.GetManifestResourceStream(resourceName) ?? throw new Exception($"Could not find resource '{resourceName}'.");
        using var output = File.Create(destFilePath);
        await input.CopyToAsync(output);
    }
}

