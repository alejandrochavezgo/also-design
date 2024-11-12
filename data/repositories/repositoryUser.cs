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

    public List<catalogModel> getGenderTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getGenderTypesCatalog", new DbParameter[] {}));
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

    public List<catalogModel> getStatusTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getStatusTypesCatalog", new DbParameter[] {}));
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

    public List<userModel> getNonUsers()
    {
        try
        {
            return factoryGetNonUsers.getList((DbDataReader)_providerDB.GetDataReader("sp_getNonUsers", new DbParameter[] {}));
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

    public userModel getUserById(int id)
    {
        try
        {
            return factoryGetUserById.get((DbDataReader)_providerDB.GetDataReader("sp_getUserById", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@userId", DbType.Int32, id),
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

    public userModel getNonUserById(int id)
    {
        try
        {
            return factoryGetNonUserById.get((DbDataReader)_providerDB.GetDataReader("sp_getNonUserById", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@userId", DbType.Int32, id),
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

    public int addContactPhone(int id, string phone)
    {
        try
        {
            var contactPhoneIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@contactPhoneIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_addUserContactPhone", new DbParameter[] {
                contactPhoneIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@employeeId", DbType.Int32, id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@phone", DbType.String, phone)
            });
            return Convert.ToInt32(contactPhoneIdAdded.Value);
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
            var userIdUpdated = dataFactory.getObjParameter(configurationManager.providerDB, "@userIdUpdated", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_updateUser", new DbParameter[] {
                userIdUpdated,
                dataFactory.getObjParameter(configurationManager.providerDB,"@userId",DbType.Int32, user.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@status",DbType.Int32, user.status),
                dataFactory.getObjParameter(configurationManager.providerDB,"@email",DbType.String, user.email!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@firstname",DbType.String,user.firstname!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@lastname",DbType.String,user.lastname!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@modificationDate",DbType.DateTime, user.modificationDate)
            });
            return Convert.ToInt32(userIdUpdated.Value) > 0;
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

    public bool addUserToEmployee(userModel user)
    {
        try
        {
            var userIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@userIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_addUserToEmployee", new DbParameter[] {
                userIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB, "@userId", DbType.Int32, user.id),
                dataFactory.getObjParameter(configurationManager.providerDB, "@username", DbType.String, user.username!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@password", DbType.String, user.passwordHash!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@creationDate", DbType.DateTime, user.creationDate!)
            });
            return Convert.ToInt32(userIdAdded.Value) > 0;
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

    public int addUser(userModel user)
    {
        try
        {
            var userIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@userIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_addUser", new DbParameter[] {
                userIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@status", DbType.Int32, user.status),
                dataFactory.getObjParameter(configurationManager.providerDB,"@username", DbType.String, user.username!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@password", DbType.String, user.passwordHash!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@firstname", DbType.String,user.firstname!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@lastname", DbType.String, user.lastname!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@creationDate", DbType.DateTime, user.creationDate),
                dataFactory.getObjParameter(configurationManager.providerDB,"@email", DbType.String, user.email!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@failCount", DbType.Int32, user.failCount)
            });
            return Convert.ToInt32(userIdAdded.Value);
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

    public bool updateUserPassword(int userId, string newPasswordHash)
    {
        try
        {
            var userIdUpdated = dataFactory.getObjParameter(configurationManager.providerDB, "@userIdUpdated", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_updateUserPassword", new DbParameter[]
            {
                userIdUpdated,
                dataFactory.getObjParameter(configurationManager.providerDB,"@userId",DbType.Int32, userId),
                dataFactory.getObjParameter(configurationManager.providerDB,"@newPasswordHash",DbType.String, newPasswordHash)
            });
            return Convert.ToInt32(userIdUpdated.Value) > 0;
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

    public int addNonUser(userModel user)
    {
        try
        {
            var userIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@userIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);

            base._providerDB.ExecuteNonQuery("sp_addNonUser", new DbParameter[] {
                userIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@status", DbType.Int32, user.status),
                dataFactory.getObjParameter(configurationManager.providerDB,"@firstname", DbType.String,user.firstname!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@lastname", DbType.String, user.lastname!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@creationDate", DbType.DateTime, user.creationDate),
                dataFactory.getObjParameter(configurationManager.providerDB,"@email", DbType.String, user.email!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@failCount", DbType.Int32, user.failCount)
            });

            return Convert.ToInt32(userIdAdded.Value);
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

    public int removeContactPhonesByEmployeeId(int id)
    {
        try
        {
            return base._providerDB.ExecuteNonQuery("sp_removeContactPhonesByEmployeeId", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@employeeId", DbType.Int32, id)
            });
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