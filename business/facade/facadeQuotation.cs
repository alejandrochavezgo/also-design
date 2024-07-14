namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;

public class facadeQuotation
{
    private log _logger;
    private repositoryQuotation _repositoryQuotation;

    public facadeQuotation()
    {
        _logger = new log();
        _repositoryQuotation = new repositoryQuotation();
    }

    public List<quotationModel> getQuotations()
    {
        try
        {
            return  _repositoryQuotation.getQuotations();
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}