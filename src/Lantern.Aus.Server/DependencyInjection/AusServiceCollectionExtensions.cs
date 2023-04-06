using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Lantern.Aus.Server.Endpoints;
using Lantern.Aus.Server.Services;
using System.Text;

namespace Lantern.Aus.Server;

public static class AusServiceCollectionExtensions
{
    public static IServiceCollection AddAusPackageService(this IServiceCollection services, Action<AusUpdateServiceOptions>? optionsAction = null)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        AusUpdateServiceOptions options = new();
        optionsAction?.Invoke(options);

        services.AddSingleton(options);
        services.AddScoped<IAusPackageEndpoint, AusPackageEndpoint>();
        services.TryAddSingleton<IAusManifestCache, InMemoryAusManifestCache>();

        return services;
    }

    public static IServiceCollection AddAusPackageFileWatcher(this IServiceCollection services, Action<AusPackageFileWatchOptions> optionsAction)
    {
        if(optionsAction == null)
            throw new ArgumentNullException(nameof(optionsAction));

        AusPackageFileWatchOptions options = new();
        optionsAction(options);

        if(string.IsNullOrWhiteSpace(options.PackagesDirectory))
            throw new ArgumentNullException(nameof(options.PackagesDirectory));

        Directory.CreateDirectory(options.PackagesDirectory);

        services.AddSingleton(options);
        services.AddHostedService<AusPackageFileWatchBackgroundService>();

        services.TryAddSingleton(new AusUpdateServiceOptions());
        services.TryAddSingleton<IAusManifestCache, InMemoryAusManifestCache>();

        return services;
    }

}
