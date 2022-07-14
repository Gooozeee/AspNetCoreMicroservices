using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.AddConfiguration(builder.Configuration);
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle()); ;

var app = builder.Build();

builder.Configuration.AddJsonFile($"ocelot.{app.Environment.EnvironmentName}.json", true, true);

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();
