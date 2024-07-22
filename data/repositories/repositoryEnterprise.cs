namespace data.repositories;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using common.configurations;
using common.logging;
using data.factoryInstances;
using data.providerData;
using entities.enums;
using entities.models;
using Newtonsoft.Json;

public class repositoryEnterprise : baseRepository
{
    private log _logger;

    public repositoryEnterprise()
    {
        _logger = new log();
    }

    public List<enterpriseModel> getEnterpriseFullInformationByIdAndConfigType(int id, configType configType)
    {
        try
        {
            return factoryGetEnterpriseFullInformationByIdAndConfigType.getList((DbDataReader)_providerDB.GetDataReader("sp_getEnterpriseFullInformationByIdAndConfigType", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@enterpriseId", DbType.Int32, id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@configType", DbType.Int32, (int)configType)
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