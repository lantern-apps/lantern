namespace Lantern;

public interface ILanternApp
{
    IServiceProvider Services { get; }
    void Stop();
}
