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

public class repositoryClient : baseRepository
{
    private log _logger;

    public repositoryClient()
    {
        _logger = new log();
    }

    public List<clientModel> getClients()
    {
        try
        {
            return factoryGetClients.getList((DbDataReader)_providerDB.GetDataReader("sp_getClients", new DbParameter[] {}));
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

    public clientModel getClientById(int id)
    {
        try
        {
            return factoryGetClientById.get((DbDataReader)_providerDB.GetDataReader("sp_getClientById", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientId", DbType.Int32, id),
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

    public List<string> getContactNamesByclientId(int id)
    {
        try
        {
            return factoryGetClientContactNamesByClientId.getList((DbDataReader)_providerDB.GetDataReader("sp_getContactNamesByClientId", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientId", DbType.Int32, id),
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

    public List<string> getContactEmailsByclientId(int id)
    {
        try
        {
            return factoryGetClientContactEmailsByClientId.getList((DbDataReader)_providerDB.GetDataReader("sp_getContactEmailsByClientId", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientId", DbType.Int32, id),
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

    public List<string> getContactPhonesByClientId(int id)
    {
        try
        {
            return factoryGetClientContactPhonesByClientId.getList((DbDataReader)_providerDB.GetDataReader("sp_getContactPhonesByClientId", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientId", DbType.Int32, id),
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

    public List<clientModel> getClientsByTerm(string businessName)
    {
        try
        {
            return factoryGetClientsByTerm.getList((DbDataReader)_providerDB.GetDataReader("sp_getClientsByTerm", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@businessName", DbType.String, businessName),
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

    public int addClient(clientModel client)
    {
        try
        {
            var clientIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@clientIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);

            base._providerDB.ExecuteNonQuery("sp_addClient", new DbParameter[] {
                clientIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@businessName", DbType.String, client.businessName!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@rfc", DbType.String, client.rfc!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@address", DbType.String, client.address!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@zipcode", DbType.String, client.zipCode!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@city", DbType.String, client.city!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@state", DbType.String, client.state!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@country", DbType.String, client.country!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@creationDate", DbType.DateTime, client.creationDate),
                dataFactory.getObjParameter(configurationManager.providerDB,"@status", DbType.Int32, client.status)
            });

            return Convert.ToInt32(clientIdAdded.Value);
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

    public int addContactName(int id, string name)
    {
        try
        {
            var contactNameIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@contactNameIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);

            base._providerDB.ExecuteNonQuery("sp_addClientContactName", new DbParameter[] {
                contactNameIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientId", DbType.Int32, id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@name", DbType.String, name)
            });

            return Convert.ToInt32(contactNameIdAdded.Value);
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

    public int addContactEmail(int id, string email)
    {
        try
        {
            var contactEmailIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@contactEmailIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);

            base._providerDB.ExecuteNonQuery("sp_addClientContactEmail", new DbParameter[] {
                contactEmailIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientId", DbType.Int32, id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@email", DbType.String, email)
            });

            return Convert.ToInt32(contactEmailIdAdded.Value);
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

            base._providerDB.ExecuteNonQuery("sp_addClientContactPhone", new DbParameter[] {
                contactPhoneIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientId", DbType.Int32, id),
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

    public int updateClient(clientModel client)
    {
        try
        {
            var clientIdUpdated = dataFactory.getObjParameter(configurationManager.providerDB, "@clientIdUpdated", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);

            base._providerDB.ExecuteNonQuery("sp_updateClient", new DbParameter[] {
                clientIdUpdated,
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientId", DbType.String, client.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@businessName", DbType.String, client.businessName!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@rfc", DbType.String, client.rfc!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@address", DbType.String, client.address!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@zipcode", DbType.String, client.zipCode!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@city", DbType.String, client.city!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@state", DbType.String, client.state!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@country", DbType.String, client.country!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@status", DbType.Int32, client.status),
                dataFactory.getObjParameter(configurationManager.providerDB,"@modificationDate", DbType.DateTime, client.modificationDate!.Value)
            });

            return Convert.ToInt32(clientIdUpdated.Value);
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

    public int removeContactNamesEmailsAndPhonesByClientId(int id)
    {
        try
        {
            return base._providerDB.ExecuteNonQuery("sp_removeContactNamesEmailsAndPhonesByClientId", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientId", DbType.Int32, id)
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