namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;
using System.Transactions;
using entities.enums;

public class facadeEnterprise
{
    private log _logger;
    private repositoryEnterprise _repositoryEnterprise;

    public facadeEnterprise()
    {
        _logger = new log();
        _repositoryEnterprise = new repositoryEnterprise();
    }

    public List<enterpriseModel> getEnterpriseFullInformationByIdAndConfigType(int id, configType configType)
    {
        try
        {
            return _repositoryEnterprise.getEnterpriseFullInformationByIdAndConfigType(id, configType);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}