using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Lantern.Aus.Server.Services;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Lantern.Aus.Server.Endpoints;

public class AusPackageEndpoint : IAusPackageEndpoint
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    };

    private readonly IAusManifestCache _store;
    private readonly AusPackageFileWatchOptions _watchOptions;
    private readonly AusUpdateServiceOptions _serviceOptions;
    private readonly StringComparison comparison;

    public AusPackageEndpoint(
        AusPackageFileWatchOptions watchOptions,
        AusUpdateServiceOptions serviceOptions,
        IAusManifestCache store)
    {
        _watchOptions = watchOptions;
        _serviceOptions = serviceOptions;
        _store = store;

        comparison = _serviceOptions.IgnoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
    }

    public IResult GetLatestManifest(string package)
    {
        var manifest = _store.GetManifest(package);
        if (manifest == null)
        {
            return NotFound(package);
        }

        return Results.Json(manifest, SerializerOptions);
    }

    public IResult GetManifest(string package, string version)
    {
        if (string.IsNullOrEmpty(version))
        {
            return GetLatestManifest(package);
        }

        if (!Version.TryParse(version, out var v))
        {
            return InvalidVersion(package);
        }

        var manifest = _store.GetManifest(package, v);
        if (manifest == null)
        {
            return NotFound(package, v);
        }

        return Results.Json(manifest, SerializerOptions);
    }

    public IResult GetPackageFile(string package, string version, string name)
    {
        if (!Version.TryParse(version, out var v))
        {
            return InvalidVersion(package);
        }

        var manifest = _store.GetManifest(package, v);
        if (manifest == null)
        {
            return NotFound(package, v);
        }

        var file = manifest.Files.FirstOrDefault(x => string.Equals(x.Name, name, comparison));
        if (file == null)
        {
            return NotFound(package, v, name);
        }

        var stream = File.Open(Path.Combine(_watchOptions.PackagesDirectory, manifest.Name, manifest.Version.ToString(), file.Name), FileMode.Open, FileAccess.Read, FileShare.Read);
        return Results.File(stream, null, Path.GetFileName(file.Name));
    }

    public IResult GetUpdate(string package, string version)
    {
        if (!Version.TryParse(version, out var v))
        {
            return InvalidVersion(package);
        }

        var patches = _store.GetNewestManifests(package, v);
        if (patches.Length == 0)
        {
            return Results.NoContent();
        }

        AusManifest? manifest = null;

        foreach (var patch in patches)
        {
            if (manifest == null)
                manifest = patch;
            else
                manifest = manifest.ApplyPatch(patch);
        }

        manifest!.ClearFileVersion();

        return Results.Json(manifest, SerializerOptions);
    }

    private static IResult InvalidVersion(string package)
    {
        return Results.BadRequest(new ProblemDetails
        {
            Title = AusErrorCodes.InvalidPackageVersion,
            Detail = $"包{package}版本号格式错误",
        });
    }

    private static IResult NotFound(string package)
    {
        return Results.NotFound(new ProblemDetails
        {
            Title = AusErrorCodes.NotFoundPackage,
            Detail = $"未找到包{package}",
        });
    }

    private static IResult NotFound(string package, Version version)
    {
        return Results.NotFound(new ProblemDetails
        {
            Title = AusErrorCodes.NotFoundPackageVersion,
            Detail = $"未找到包{package}-v{version}",
        });
    }

    private static IResult NotFound(string package, Version version, string filename)
    {
        return Results.NotFound(new ProblemDetails
        {
            Title = AusErrorCodes.NotFoundPackageFile,
            Detail = $"未找到包{package}-v{version}-{filename}",
        });
    }

}