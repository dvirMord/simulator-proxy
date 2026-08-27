using proxy_simulator.Interfaces;
using proxy_simulator.Config;
using proxy_simulator.Services;
using proxy_simulator.Constants;


var builder = WebApplication.CreateBuilder(args);

AppConfig.Configuration = builder.Configuration;

//============== Services Configuration ==============
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

//======================DI's======================================
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddSingleton<IDBService, SQLiteService>();
builder.Services.AddSingleton<IDeviceService, DeviceService>();
//=====================Http client's==============================
builder.Services.AddHttpClient<IMultimediaServiceAPI, MultimediaServiceAPI>
(MultimediaSimulator =>
{
    MultimediaSimulator.BaseAddress = new Uri("http://localhost:5002/");   
});
builder.Services.AddHttpClient<ITelemetryServiceAPI, TelemetryServiceAPI>
(TelemetrySimulator =>
{
    TelemetrySimulator.BaseAddress = new Uri("http://localhost:8000/");
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


//============== Middleware and swagger Pipeline ==============
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    { 
        c.SwaggerEndpoint(ServicesConstants.Program.Swagger.SWAGGER_URL, ServicesConstants.Program.Swagger.SWAGGER_NAME); 
    });
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();