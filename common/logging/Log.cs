using common.configurations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace common.logging
{
    public class Log
    {
        private readonly ILogger? _loggerT;                                         
        private readonly NLog.Logger _loggerF;                                     

        public Log(ILogger<Log>? logT = null)
        {
            _loggerT = logT;
            _loggerF = NLog.LogManager.GetCurrentClassLogger();
        }
        
        public void logInfo(string message)
        {
            try
            {
                message = $"{ConfigurationManager.AppSettings["system:version"]} :: {message}";

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
                message = $"{ConfigurationManager.AppSettings["system:version"]} :: {message}";

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
}