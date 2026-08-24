using multimedia_simulator.Interfaces;
using proxy_simulator.Config;
using proxy_simulator.Interfaces;
using proxy_simulator.Services;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

AppConfig.Configuration = builder.Configuration;

//============== Services Configuration ==============
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IDBService, SQLiteService>();

var app = builder.Build();


//===========App services===================================
IDBService dbService = app.Services.GetRequiredService<IDBService>();
//==========================================================


//============== Application Lifecycle Events ==============
var lifetime = app.Lifetime;
//==================== On Started ====================
lifetime.ApplicationStarted.Register(async () =>
{
    app.Logger.LogInformation("--> [Lifecycle] Application Started: initializing DB...");
    await dbService.CreateConnectionAndInitialize();
});
//==================== On Stopping-Graceful Shutdown ====================
lifetime.ApplicationStopping.Register(async () =>
{
    app.Logger.LogInformation("--> [Lifecycle] Graceful Shutdown: Server is shutting down, cleaning up...");
});
//==================== On Stopped ====================
lifetime.ApplicationStopped.Register(() =>
{
    app.Logger.LogInformation("--> [Lifecycle] Application Stopped: Server is completely closed.");
});
//============== END-Application Lifecycle Events-END ==============


//============== Middleware Pipeline ==============
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();