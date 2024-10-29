namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;

public class facadeTrace
{
    private log _logger;
    private repositoryTrace _repositoryTrace;

    public facadeTrace()
    {
        _logger = new log();
        _repositoryTrace = new repositoryTrace();
    }

    public int addTrace(traceModel trace)
    {
        try
        {
            return _repositoryTrace.addTrace(trace);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}