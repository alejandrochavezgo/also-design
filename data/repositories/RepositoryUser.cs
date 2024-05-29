namespace data.repositories;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using common.configurations;
using data.factoryInstances;
using data.providerData;
using entities.models;

public class RepositoryUser : BaseRepository 
{
    public UserModel getUserByIdUser(int idUser)
    {
        try 
        {
            return FactoryGetUserByIdUser.Get((DbDataReader)_ProviderDB.GetDataReader("sp_getUserByIdUser", new DbParameter[] {
                DataFactory.GetObjParameter(ConfigurationManager.ProviderDB, "@idUser", DbType.Int32, idUser)
            }));
        }
        catch (SqlException SqlException)
        {
            throw SqlException;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
}