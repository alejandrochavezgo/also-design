using api.authorization;
using api.services;
using common.logging;
using providerData.helpers;
using Microsoft.Extensions.Logging.Console;
using NLog;
using NLog.Web;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(i => {
        i.ColorBehavior = LoggerColorBehavior.Enabled;
        i.IncludeScopes = true;
        i.SingleLine = true;
        i.TimestampFormat = "yyyy-MM-dd hh:mm:ss ";
        i.UseUtcTimestamp = true;
    }).SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
});
var _logT = loggerFactory.CreateLogger<log>();
var _logF = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

var log = new log(_logT, _logF);
var trackId = Guid.NewGuid().ToString();
log.logDebug("[ALSO DESIGN SYSTEM] Starting up the Also Design System...");

try
{
    log.logDebug("[ALSO DESIGN SYSTEM] Creating builder...");
    var builder = WebApplication.CreateBuilder(args);
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    log.logDebug("[ALSO DESIGN SYSTEM] Adding services...");
    log.logDebug("\t[+] MVC services...");
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    //Add services to DI container.
    {
        var services = builder.Services;
        services.AddCors();
        services.AddControllers();

        //Configure strongly typed settings object.
        services.Configure<appSettingsHelper>(builder.Configuration.GetSection("jwt"));

        //Configure DI for application services.
        services.AddScoped<IJwtUtils, jwtUtils>();
        services.AddScoped<IUserService, userService>();
        
        #region ::services - provider for sql server
        log.logDebug("\t[+] SQL Server provider service...");
        System.Data.Common.DbProviderFactories.RegisterFactory(common.configurations.configurationManager.appSettings["providers:alsoProviderName"]!, System.Data.SqlClient.SqlClientFactory.Instance);
        #endregion
    }

    var app = builder.Build();

    //Configure HTTP request pipeline.
    {
        //Global cors policy.
        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        //Custom jwt auth middleware.
        app.UseMiddleware<jwtMiddleware>();
        app.MapControllers();
    }

    app.Run();
}
catch (Exception e)
{
    log.logError($"[ALSO DESIGN SYSTEM] Error in the Also Design System project:");
    throw;
}
finally 
{
    log.logDebug("[ALSO DESIGN SYSTEM] Flushing and stoping internal timers/threads before application-exit...");
    NLog.LogManager.Shutdown();
    log.logDebug("[ALSO DESIGN SYSTEM] Shutting down the Also Design System...");
}