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

public class repositorySupplier : baseRepository
{
    private log _logger;

    public repositorySupplier()
    {
        _logger = new log();
    }

    public List<supplierModel> getSuppliers()
    {
        try
        {
            return factoryGetSuppliers.getList((DbDataReader)_providerDB.GetDataReader("sp_getSuppliers", new DbParameter[] {}));
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

    public supplierModel getSupplierById(int id)
    {
        try
        {
            return factoryGetSupplierById.get((DbDataReader)_providerDB.GetDataReader("sp_getSupplierById", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB,"@supplierId", DbType.Int32, id),
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

    public List<string> getContactNamesBySupplierId(int id)
    {
        try
        {
            return factoryGetSupplierContactNamesBySupplierId.getList((DbDataReader)_providerDB.GetDataReader("sp_getContactNamesBySupplierId", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB,"@supplierId", DbType.Int32, id),
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

    public List<string> getContactEmailsBySupplierId(int id)
    {
        try
        {
            return factoryGetSupplierContactEmailsBySupplierId.getList((DbDataReader)_providerDB.GetDataReader("sp_getContactEmailsBySupplierId", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB,"@supplierId", DbType.Int32, id),
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

    public List<string> getContactPhonesBySupplierId(int id)
    {
        try
        {
            return factoryGetSupplierContactPhonesBySupplierId.getList((DbDataReader)_providerDB.GetDataReader("sp_getContactPhonesBySupplierId", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB,"@supplierId", DbType.Int32, id),
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

    public List<supplierModel> getSuppliersByTerm(string businessName)
    {
        try
        {
            return factoryGetSuppliersByTerm.getList((DbDataReader)_providerDB.GetDataReader("sp_getSuppliersByTerm", new DbParameter[]
            {
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

    public int addSupplier(supplierModel supplier)
    {
        try
        {
            var supplierIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@supplierIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);

            base._providerDB.ExecuteNonQuery("sp_addSupplier", new DbParameter[] {
                supplierIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@businessName", DbType.String, supplier.businessName!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@rfc", DbType.String, supplier.rfc!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@address", DbType.String, supplier.address!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@zipcode", DbType.String, supplier.zipCode!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@city", DbType.String, supplier.city!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@state", DbType.String, supplier.state!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@country", DbType.String, supplier.country!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@creationDate", DbType.DateTime, supplier.creationDate),
                dataFactory.getObjParameter(configurationManager.providerDB,"@status", DbType.Int32, supplier.status)
            });

            return Convert.ToInt32(supplierIdAdded.Value);
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

            base._providerDB.ExecuteNonQuery("sp_addSupplierContactName", new DbParameter[] {
                contactNameIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@supplierId", DbType.Int32, id),
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

            base._providerDB.ExecuteNonQuery("sp_addSupplierContactEmail", new DbParameter[] {
                contactEmailIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@supplierId", DbType.Int32, id),
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

            base._providerDB.ExecuteNonQuery("sp_addSupplierContactPhone", new DbParameter[] {
                contactPhoneIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@supplierId", DbType.Int32, id),
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

    public int updateSupplier(supplierModel supplier)
    {
        try
        {
            var supplierIdUpdated = dataFactory.getObjParameter(configurationManager.providerDB, "@supplierIdUpdated", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);

            base._providerDB.ExecuteNonQuery("sp_updateSupplier", new DbParameter[] {
                supplierIdUpdated,
                dataFactory.getObjParameter(configurationManager.providerDB,"@supplierId", DbType.String, supplier.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@businessName", DbType.String, supplier.businessName!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@rfc", DbType.String, supplier.rfc!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@address", DbType.String, supplier.address!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@zipcode", DbType.String, supplier.zipCode!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@city", DbType.String, supplier.city!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@state", DbType.String, supplier.state!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@country", DbType.String, supplier.country!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@status", DbType.Int32, supplier.status),
                dataFactory.getObjParameter(configurationManager.providerDB,"@modificationDate", DbType.DateTime, supplier.modificationDate!.Value)
            });

            return Convert.ToInt32(supplierIdUpdated.Value);
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

    public int removeContactNamesEmailsAndPhonesBySupplierId(int id)
    {
        try
        {
            return base._providerDB.ExecuteNonQuery("sp_removeContactNamesEmailsAndPhonesBySupplierId", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@supplierId", DbType.Int32, id)
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