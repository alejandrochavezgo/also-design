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

public class repositoryRole : baseRepository
{
    private log _logger;

    public repositoryRole()
    {
        _logger = new log();
    }

    public List<roleModel> getUserRolesByUserId(int idUser)
    {
        try
        {
            return factoryGetUserRolesByUserId.getList((DbDataReader)_providerDB.GetDataReader("sp_getUserRolesByUserId", new DbParameter[] 
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