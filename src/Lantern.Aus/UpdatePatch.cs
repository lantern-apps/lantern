namespace AutoUpdates;

public class UpdatePatch
{
    public static readonly UpdatePatch Empty = new(null, null, false, null);

    public UpdatePatch(AusManifest? manifest, IReadOnlyList<AusFile>? files, bool isPrepared, bool? mapFileExtensions)
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
