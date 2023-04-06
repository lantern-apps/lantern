using Robos.Aus;


var manifest = AusManifest.LoadFromFile(".manifest");
var patch = AusManifest.LoadFromFile(".patch");
var a = manifest.CheckForUpdates(patch);

Console.WriteLine("start");

using (AusUpdateManager manager = new(new AusUpdateOptions
{
    ServerAddress = "https://ytwaimao.com:44322/",
    PackageName = "UT",
    UpdaterName = "UT-更新程序-AutoUpdater.exe",
}))
{
    //await manager.CheckPerformUpdateAsync(default);
    if (manager.HasUpdatePrepared(out Version version))
    {
        manager.LaunchUpdater(version!);
    }

    var result = await manager.CheckForUpdateAsync();
    if (result.CanUpdate)
    {
        await manager.PrepareUpdateAsync(result);
        //manager.LaunchUpdater(result.LastVersion, true);
    }
}

Console.WriteLine("hello");
Console.ReadLine();
