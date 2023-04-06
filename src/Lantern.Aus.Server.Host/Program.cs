using Lantern.Aus.Server;
using Lantern.Aus.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAusPackageService();
builder.Services.AddAusPackageFileWatcher(options =>
{
    builder.Configuration.Bind(nameof(AusPackageFileWatchOptions), options);
});

builder.Services.AddResponseCompression();
var app = builder.Build();

app.UseResponseCompression();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapAusEndpoints().WithOpenApi();

app.Run();