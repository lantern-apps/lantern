using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Lantern.Aus.Server.Endpoints;

namespace Lantern.Aus.Server;

public static class AusEndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder MapAusEndpoints(this IEndpointRouteBuilder builder)
    {
        var builders = new RouteHandlerBuilder[]
        {
            builder.MapGet("/packages/{package}/manifest/latest", (
                [FromServices] IAusPackageEndpoint endpoint,
                [FromRoute] string package) => endpoint.GetLatestManifest(package))
                .WithName("GetLatestManifest")
                .Produces<AusManifest>(200)
                .ProducesProblem(404)
                .ProducesProblem(400),

            builder.MapGet("/packages/{package}/manifest", (
                [FromServices] IAusPackageEndpoint endpoint,
                [FromRoute] string package,
                [FromQuery] string version) => endpoint.GetManifest(package, version))
                .WithName("GetManifest")
                .Produces<AusManifest>(200)
                .ProducesProblem(404)
                .ProducesProblem(400),

            builder.MapGet("/packages/{package}/file", (
                [FromServices] IAusPackageEndpoint endpoint,
                [FromRoute] string package,
                [FromQuery] string version,
                [FromQuery] string name) => endpoint.GetPackageFile(package, version, name))
                .WithName("GetPackageFile")
                .Produces<byte[]>(200)
                .ProducesProblem(404)
                .ProducesProblem(400),

            builder.MapGet("/packages/{package}/update", (
                [FromServices] IAusPackageEndpoint endpoint,
                [FromRoute] string package,
                [FromQuery] string version) => endpoint.GetUpdate(package, version))
                .WithName("GetUpdate")
                .Produces<AusManifest>(200)
                .Produces(204)
                .ProducesProblem(404)
                .ProducesProblem(400),
        };

        return new RouteHandlerBuilder(builders).WithTags("Aus");
    }
}
