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

public class repositoryUser : baseRepository
{
    private log _logger;

    public repositoryUser()
    {
        _logger = new log();
    }

    public List<userModel> getUsers()
    {
        try
        {
            return factoryGetUsers.getList((DbDataReader)_providerDB.GetDataReader("sp_getUsers", new DbParameter[] {}));
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

    public bool updateUser(userModel user)
    {
        try
        {
            var userIdModified = dataFactory.getObjParameter(configurationManager.providerDB, "@userIdModified", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);

            base._providerDB.ExecuteNonQuery("sp_updateUser", new DbParameter[] {
                userIdModified,
                dataFactory.getObjParameter(configurationManager.providerDB,"@userId",DbType.Int32, user.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@email",DbType.String, user.email!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@firstname",DbType.String,user.firstname!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@lastname",DbType.String,user.lastname!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@isActive",DbType.Boolean, user.isActive)
            });

            return Convert.ToInt32(userIdModified.Value) > 0;
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