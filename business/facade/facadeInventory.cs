namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;

public class facadeInventory
{
    private log _logger;
    private repositoryInventory _repositoryInventory;

    public facadeInventory()
    {
        _logger = new log();
        _repositoryInventory = new repositoryInventory();
    }

    public List<inventoryModel> getItemByTerm(string description)
    {
        try
        {
            return _repositoryInventory.getItemByTerm(description);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}