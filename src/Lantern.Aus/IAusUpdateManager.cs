namespace Lantern.Aus;

/// <summary>
/// Automatic Update Manager
/// </summary>
public interface IAusUpdateManager : IDisposable
{
    /// <summary>
    /// Mark sure manifest file is prepared
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task EnsurePrepareManifestAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Check update
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AusUpdateResult> CheckForUpdateAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Prepare update files
    /// </summary>
    /// <param name="result"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PrepareUpdateAsync(AusUpdateResult result, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check and perform update if can update
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CheckPerformUpdateAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Has update file is prepared
    /// </summary>
    /// <param name="version"></param>
    /// <returns></returns>
    bool HasUpdatePrepared(out Version version);

    /// <summary>
    /// Is specify version update file prepared
    /// </summary>
    /// <param name="version"></param>
    /// <returns></returns>
    bool IsUpdatePrepared(Version version);

    /// <summary>
    /// Launch updater program
    /// </summary>
    /// <param name="version">Version</param>
    /// <param name="options">Launch options</param>
    void LaunchUpdater(Version? version, LaunchUpdaterOptions? options = null);

    /// <summary>
    /// Launch updater program
    /// </summary>
    /// <param name="options">Launch options</param>
    void LaunchUpdater(LaunchUpdaterOptions? options = null) => LaunchUpdater(null, options);

}
