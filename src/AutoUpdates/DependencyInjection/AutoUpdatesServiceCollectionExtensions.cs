using Microsoft.Extensions.DependencyInjection;

namespace AutoUpdates;

/// <inheritdoc/>
public static class AutoUpdatesServiceCollectionExtensions
{
    /// <summary>
    /// Add automatic update background service
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddAutoUpdateBackgroundService(this IServiceCollection services, Action<AutoUpdateBackgroundServiceOptions> optionsAction)
    {
        if (optionsAction == null)
            throw new ArgumentNullException(nameof(optionsAction));

        AutoUpdateBackgroundServiceOptions options = new();
        optionsAction(options);

        services.AddAutoUpdateManager(options);

        services.AddSingleton(options);
        services.AddHostedService<AutoUpdateBackgroundService>();

        return services;
    }

    /// <summary>
    /// Add automatic update manager
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddAutoUpdateManager(this IServiceCollection services, Action<UpdateOptions> optionsAction)
    {
        if (optionsAction == null)
            throw new ArgumentNullException(nameof(optionsAction));

        UpdateOptions options = new();
        optionsAction(options);

        return services.AddAutoUpdateManager(options);
    }

    /// <summary>
    /// Add automatic update manager
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddAutoUpdateManager(this IServiceCollection services, UpdateOptions options)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        options.Validate();

        services.AddSingleton(options);
        services.AddSingleton<IUpdateManager, UpdateManager>();

        return services;
    }
}
