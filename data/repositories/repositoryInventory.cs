namespace data.repositories;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using common.configurations;
using common.logging;
using data.factoryInstances;
using data.providerData;
using entities.models;
using Newtonsoft.Json;

public class repositoryInventory : baseRepository
{
    private log _logger;

    public repositoryInventory()
    {
        _logger = new log();
    }

    public List<inventoryModel> getItemByTerm(string description)
    {
        try
        {
            return factoryGetItemByTerm.getList((DbDataReader)_providerDB.GetDataReader("sp_getInventoryByTerm", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@description", DbType.String, description),
            }));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}