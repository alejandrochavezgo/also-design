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

public class repositoryMenu : baseRepository
{
    private log _logger;

    public repositoryMenu()
    {
        _logger = new log();
    }

    public List<menuModel> getUserMenusByUserId(int idUser)
    {
        try
        {
            return factoryGetUserMenusByUserId.getList((DbDataReader)_providerDB.GetDataReader("sp_getUserMenusByUserId", new DbParameter[] 
            {
                dataFactory.getObjParameter(configurationManager.providerDB, "@idUser", DbType.Int32, idUser)
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