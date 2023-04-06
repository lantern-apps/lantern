using Microsoft.Extensions.DependencyInjection;

namespace Lantern;

public interface ILanternHostBuilder
{
    IServiceCollection Services { get; }
    ILanternHost Build();
}
