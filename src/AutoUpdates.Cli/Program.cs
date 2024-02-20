using AutoUpdates;
using System.Text.Json;
using System.Text.Json.Serialization;

if (args.Length != 2)
    throw new ArgumentException("缺少参数");

var name = args[0].Trim();
var directory = args[1].Trim();

var appsettings = (AppSettings?)JsonSerializer.Deserialize(File.ReadAllText(Path.Combine(directory, "appsettings.json")), typeof(AppSettings), AppSettingsJsonContext.Default);

AusManifest.Load(name, 
    appsettings?.Version ?? new Version(0, 0, 1), 
    directory, ["appsettings.json"])
    .SaveAs(Path.Combine(directory, "manifest.json"));

Console.WriteLine("生成成功");

public class AppSettings
{
    public Version? Version { get; set; }
}

[JsonSerializable(typeof(AppSettings))]
public partial class AppSettingsJsonContext : JsonSerializerContext
{

}