using Microsoft.Extensions.DependencyInjection;

namespace Lantern;

public static class LanternAppServiceCollectionExtensions
{
    public static LanternAppHostBuilder AddLanternApp(this IServiceCollection services) => LanternAppHostBuilder.Create(services);
}
