namespace Lantern.Aus;

public class AusUpdateResult
{
    public AusUpdateResult(AusManifest manifest, AusManifest? patch, IList<AusFile>? files, bool isPrepared)
    {
        Manifest = manifest;
        Patch = patch!;

        UpdateFiles = new List<AusFile>();
        if (patch == null || files == null || files.Count == 0)
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
    }

    /// <summary>
    /// Update patch
    /// </summary>
    public AusManifest Patch { get; }

    /// <summary>
    /// Current file manifest
    /// </summary>
    public AusManifest Manifest { get; }

    public bool CanUpdate { get; }
    public bool IsPrepared { get; }

    public IReadOnlyList<AusFile> UpdateFiles { get; }
}
