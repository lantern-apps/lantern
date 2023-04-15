using Microsoft.Extensions.DependencyInjection;

namespace Lantern.Aus;

/// <inheritdoc/>
public static class AusUpdateServiceCollectionExtensions
{
    /// <summary>
    /// Add automatic update background service
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddAusBackgroundService(this IServiceCollection services, Action<AusBackgroundServiceOptions> optionsAction)
    {
        if (optionsAction == null)
            throw new ArgumentNullException(nameof(optionsAction));

        AusBackgroundServiceOptions options = new();
        optionsAction(options);

        services.AddAusUpdateManager(options);

        services.AddSingleton(options);
        services.AddHostedService<AusBackgroundService>();

        return services;
    }

    /// <summary>
    /// Add automatic update manager
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddAusUpdateManager(this IServiceCollection services, Action<AusUpdateOptions> optionsAction)
    {
        if (optionsAction == null)
            throw new ArgumentNullException(nameof(optionsAction));

        AusUpdateOptions options = new();
        optionsAction(options);

        return services.AddAusUpdateManager(options);
    }

    /// <summary>
    /// Add automatic update manager
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddAusUpdateManager(this IServiceCollection services, AusUpdateOptions options)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        options.Validate();

        services.AddSingleton(options);
        services.AddSingleton<IAusUpdateManager, AusUpdateManager>();

        return services;
    }
}
