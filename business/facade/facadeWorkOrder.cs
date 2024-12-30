namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;
using entities.enums;

public class facadeWorkOrder
{
    private log _logger;
    private repositoryWorkOrder _repositoryWorkOrder;

    public facadeWorkOrder()
    {
        _logger = new log();
        _repositoryWorkOrder = new repositoryWorkOrder();
    }

    public List<List<catalogModel>> getAllWorkOrderCatalogs()
    {
        try
        {
            var catalogs = new List<List<catalogModel>>();
            catalogs.Add(_repositoryWorkOrder.getPriorityTypesCatalog());
            return catalogs;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}