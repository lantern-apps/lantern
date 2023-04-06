namespace Lantern.Aus.Server.Services;

public class InMemoryAusManifestCache : IAusManifestCache
{
    private readonly StringComparison _comparison;
    private readonly List<AusManifest> _packages = new();
    private readonly object _lock = new();

    public InMemoryAusManifestCache(AusUpdateServiceOptions options)
    {
        _comparison = options.IgnoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
    }

    public IList<AusManifest> GetManifests() => _packages;

    public void Create(AusManifest package)
    {
        lock (_lock)
        {
            var manifest = _packages.FirstOrDefault(x => string.Equals(x.Name, package.Name, _comparison) && x.Version == package.Version);
            if (manifest != null)
            {
                _packages.Remove(manifest);
            }

            _packages.Add(package);
        }
    }

    public AusManifest? GetManifest(string package)
    {
        lock (_lock)
        {
            return _packages.Where(x => string.Equals(x.Name, package, _comparison)).OrderByDescending(x => x.Version).FirstOrDefault();
        }
    }

    public AusManifest? GetManifest(string package, Version version)
    {
        lock (_lock)
        {
            return _packages.FirstOrDefault(x => string.Equals(x.Name, package, _comparison) && x.Version == version);
        }
    }

    public void Delete(string package, Version version)
    {
        lock (_lock)
        {
            var manifest = _packages.FirstOrDefault(x => string.Equals(x.Name, package, _comparison) && x.Version == version);

            if (manifest != null)
            {
                _packages.Remove(manifest);
            }
        }
    }

    public AusManifest[] GetNewestManifests(string package, Version version)
    {
        lock (_lock)
        {
            var list = new List<AusManifest>();
            foreach(var p in _packages.Where(x => string.Equals(x.Name, package, _comparison) && x.Version > version).OrderBy(x => x.Version))
            {
                if (p.RequireVersion != null && p.RequireVersion > version)
                    break;

                list.Add(p);
            }
            return list.ToArray();
        }
    }
}