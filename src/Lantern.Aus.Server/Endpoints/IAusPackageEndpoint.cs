using Microsoft.AspNetCore.Http;

namespace Lantern.Aus.Server.Endpoints;

public interface IAusPackageEndpoint
{
    IResult GetUpdate(string package,string version);
    IResult GetLatestManifest(string package);
    IResult GetManifest(string package, string version);
    IResult GetPackageFile(string package, string version, string name);
}
