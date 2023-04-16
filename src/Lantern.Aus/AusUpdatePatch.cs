namespace Lantern.Aus;

public class AusUpdatePatch
{
    public static readonly AusUpdatePatch Empty = new(null, null, false, null);

    public AusUpdatePatch(AusManifest? manifest, IReadOnlyList<AusFile>? files, bool isPrepared, bool? mapFileExtensions)
    {
        Manifest = manifest!;

        UpdateFiles = new List<AusFile>();
        if (manifest == null || files == null || files.Count == 0)
        {
            UpdateFiles = new List<AusFile>();
            CanUpdate = false;
        }
        else
        {
            UpdateFiles = new List<AusFile>(files);
            CanUpdate = true;
        }

        IsPrepared = isPrepared;
        MapFileExtensions = mapFileExtensions;
    }

    /// <summary>
    /// Update patch
    /// </summary>
    public AusManifest Manifest { get; }

    public bool? MapFileExtensions { get; }

    public bool CanUpdate { get; }
    public bool IsPrepared { get; internal set; }

    public IReadOnlyList<AusFile> UpdateFiles { get; }
}
