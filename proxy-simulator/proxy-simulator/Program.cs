using proxy_simulator.Interfaces;
using proxy_simulator.Config;
using proxy_simulator.Services;
using proxy_simulator.Constants;


var builder = WebApplication.CreateBuilder(args);

AppConfig.Configuration = builder.Configuration;

//============== Services Configuration ==============
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IDBService, SQLiteService>();
builder.Services.AddSingleton<IDeviceService, DeviceService>();
builder.Services.AddHttpClient<IMultimediaServiceAPI, MultimediaServiceAPI>( client =>
{
    client.BaseAddress = new Uri("http://localhost:5002/");   
});

var app = builder.Build();


//===========App services===================================
IDBService dbService = app.Services.GetRequiredService<IDBService>();
//==========================================================


//============== Application Lifecycle Events ==============
var lifetime = app.Lifetime;
//==================== On Started ====================
lifetime.ApplicationStarted.Register(async () =>
{
    app.Logger.LogInformation(ProgramConstants.Logs.LifeCycle.INIT_LOG);
    app.Logger.LogInformation(ProgramConstants.Logs.LifeCycle.STARTING_LOG);
    await dbService.CreateConnectionAndInitialize();
});
//==================== On Stopping-Graceful Shutdown ====================
lifetime.ApplicationStopping.Register(() =>
{
    app.Logger.LogInformation(ProgramConstants.Logs.LifeCycle.STOPING_LOG);
});
//==================== On Stopped ====================
lifetime.ApplicationStopped.Register(() =>
{
    app.Logger.LogInformation(ProgramConstants.Logs.LifeCycle.STOPED_LOG);
});
//============== END-Application Lifecycle Events-END ==============


//============== Middleware Pipeline ==============
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();