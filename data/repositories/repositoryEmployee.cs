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

public class repositoryEmployee : baseRepository
{
    private log _logger;

    public repositoryEmployee()
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

    public int addEmployee(employeeModel employee)
    {
        try
        {
            var employeeIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@employeeIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);

            base._providerDB.ExecuteNonQuery("sp_addEmployee", new DbParameter[] {
                employeeIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@userId", DbType.Int32, employee.userId),
                dataFactory.getObjParameter(configurationManager.providerDB,"@gender", DbType.Int32, employee.gender!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@address", DbType.String, employee.address!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@city", DbType.String,employee.city!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@state", DbType.String, employee.state!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@zipcode", DbType.String, employee.zipcode!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@jobposition", DbType.String, employee.jobPosition!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@profession", DbType.String, employee.profession!)
            });

            return Convert.ToInt32(employeeIdAdded.Value);
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

    public employeeModel getEmployeeById(int id)
    {
        try
        {
            return factoryGetEmployeeById.get((DbDataReader)_providerDB.GetDataReader("sp_getEmployeeById", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB,"@employeeId", DbType.Int32, id)
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

    public List<string> getContactPhonesByEmployeeId(int id)
    {
        try
        {
            return factoryGetEmployeeContactPhonesByEmployeeId.getList((DbDataReader)_providerDB.GetDataReader("sp_getContactPhonesByEmployeeId", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@employeeId", DbType.Int32, id),
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

            base._providerDB.ExecuteNonQuery("sp_addEmployeeContactPhone", new DbParameter[] {
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

    public bool updateEmployee(employeeModel employee)
    {
        try
        {
            var employeeIdUpdated = dataFactory.getObjParameter(configurationManager.providerDB, "@employeeIdUpdated", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);

            base._providerDB.ExecuteNonQuery("sp_updateEmployee", new DbParameter[] {
                employeeIdUpdated,
                dataFactory.getObjParameter(configurationManager.providerDB,"@employeeId",DbType.Int32, employee.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@gender",DbType.Int32, employee.gender!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@address",DbType.String, employee.address!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@zipcode",DbType.String, employee.zipcode!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@city",DbType.String, employee.city!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@state",DbType.String, employee.state!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@profession",DbType.String, employee.profession!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@jobPosition",DbType.String, employee.jobPosition!)
            });

            return Convert.ToInt32(employeeIdUpdated.Value) > 0;
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

    public bool deleteEmployeeById(int id)
    {
        try
        {
            base._providerDB.ExecuteNonQuery("sp_deleteEmployeeById", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB,"@employeeId", DbType.Int32, id),
            });
            return true;
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

    public List<traceModel> getEmployeeTracesByEmployeeId(int id)
    {
        try
        {
            return factoryGetEmployeeTracesByEmployeeId.getList((DbDataReader)_providerDB.GetDataReader("sp_getLastEmployeeTracesByEmployeeId", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB, "@employeeId", DbType.Int32, id)
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

    public traceModel getEmployeeTraceById(int id)
    {
        try
        {
            return factoryGetEmployeeTraceById.get((DbDataReader)_providerDB.GetDataReader("sp_getEmployeeTraceById", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB, "@traceId", DbType.Int32, id)
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