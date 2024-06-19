namespace common.logging;

using common.configurations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog.Fluent;

public class log
{
    private readonly ILogger? _loggerT;
    private readonly NLog.Logger _loggerF;

    public log(ILogger<log>? logT = null)
    {
        _loggerT = logT;
        _loggerF = NLog.LogManager.GetCurrentClassLogger();
    }
    public log(ILogger<log> logT, NLog.Logger logF)
    {
        _loggerT = logT;
        _loggerF = logF;
    }

    public void logDebug(string message)
    {
        try
        {
            message = $"{configurationManager.appSettings["system:version"]} :: {message}";

            if (_loggerT is not null)
                _loggerT.LogDebug($"{message}");
            else
                System.Diagnostics.Debug.WriteLine($"{message}");
            _loggerF.Debug($"{message}");
        }
        catch (Exception e)
        {
            var exception = $"{JsonConvert.SerializeObject(e, Formatting.Indented)}";
            logFatal($"General exception: At manage the service request.\nData: {message}\nException caught:\n{exception}");
        }
    }

    public void logInfo(string message)
    {
        try
        {
            message = $"{configurationManager.appSettings["system:version"]} :: {message}";

            if (_loggerT is not null)
                _loggerT.LogInformation($"{message}");
            else
                System.Diagnostics.Debug.WriteLine($"{message}");
            _loggerF.Info($"{message}");
        }
        catch (Exception e)
        {
            var exception = $"{JsonConvert.SerializeObject(e, Formatting.Indented)}";
            logFatal($"General exception: At manage the service request.\nData: {message}\nException caught:\n{exception}");
        }
    }

    public void logError(string message)
    {
        try
        {
            message = $"{configurationManager.appSettings["system:version"]} :: {message}";

            if (_loggerT is not null)
                _loggerT.LogError($"{message}");
            else
                System.Diagnostics.Debug.WriteLine($"{message}");
            _loggerF.Error($"{message}");
        }
        catch (Exception e)
        {
            var exception = $"{JsonConvert.SerializeObject(e, Formatting.Indented)}";
            logFatal($"General exception: At manage the service request.\nData: {message}\nException caught:\n{exception}");
        }
    }

    public void logFatal(string message)
    {
        _loggerF.Fatal(message);
    }
}