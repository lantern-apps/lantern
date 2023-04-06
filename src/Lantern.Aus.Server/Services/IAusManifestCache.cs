namespace Lantern.Aus.Server.Services;

public interface IAusManifestCache
{
    IList<AusManifest> GetManifests();
    void Create(AusManifest package);
    void Delete(string package, Version version);

    AusManifest? GetManifest(string package);
    AusManifest? GetManifest(string package, Version version);
    AusManifest[] GetNewestManifests(string package, Version version);
}