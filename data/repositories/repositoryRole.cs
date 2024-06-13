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

public class repositoryRole : BaseRepository
{
    private Log _logger;

    public repositoryRole()
    {
        _logger = new Log();
    }

    public List<roleModel> getUserRolesByIdUser(int idUser)
    {
        try
        {
            return factoryGetUserRolesByIdUser.GetList((DbDataReader)_ProviderDB.GetDataReader("sp_getUserRolesByIdUser", new DbParameter[] 
            {
                DataFactory.GetObjParameter(ConfigurationManager.ProviderDB, "@idUser", DbType.Int32, idUser)
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