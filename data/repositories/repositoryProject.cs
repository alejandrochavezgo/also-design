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

public class repositoryProject : baseRepository
{
    private log _logger;

    public repositoryProject()
    {
        _logger = new log();
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


    public int addProject(projectModel project)
    {
        try
        {
            var projectIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@projectIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_addProject", new DbParameter[] {
                projectIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@name", DbType.String, project.name!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientId", DbType.Int32, project.client!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@description", DbType.String, project.description!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@startDate", DbType.DateTime, project.startDate!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@endDate", DbType.DateTime, project.endDate!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@creationDate", DbType.DateTime, project.creationDate!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@status", DbType.Int32, project.status)
            });
            return Convert.ToInt32(projectIdAdded.Value);
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

    public projectModel getProjectById(int id)
    {
        try
        {
            return factoryGetProjectById.get((DbDataReader)_providerDB.GetDataReader("sp_getProjectById", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@projectId", DbType.Int32, id)
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

    public List<projectModel> getProjects()
    {
        try
        {
            return factoryGetProjects.getList((DbDataReader)_providerDB.GetDataReader("sp_getProjects", new DbParameter[] {}));
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

    public List<traceModel> getProjectTracesByProjectId(int id)
    {
        try
        {
            return factoryGetUserTracesByUserId.getList((DbDataReader)_providerDB.GetDataReader("sp_getLastProjectTracesByProjectId", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB, "@projectId", DbType.Int32, id)
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

    public traceModel getProjectTraceById(int id)
    {
        try
        {
            return factoryGetProjectTraceById.get((DbDataReader)_providerDB.GetDataReader("sp_getProjectTraceById", new DbParameter[]
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

    public int updateProject(projectModel project)
    {
        try
        {
            var projectIdUpdated = dataFactory.getObjParameter(configurationManager.providerDB, "@projectIdUpdated", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_updateProject", new DbParameter[] {
                projectIdUpdated,
                dataFactory.getObjParameter(configurationManager.providerDB,"@projectId", DbType.Int32, project.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@name", DbType.String, project.name!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientId", DbType.Int32, project.client!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@description", DbType.String, project.description!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@startDate", DbType.DateTime, project.startDate!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@endDate", DbType.DateTime, project.endDate!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@status", DbType.Int32, project.status),
                dataFactory.getObjParameter(configurationManager.providerDB,"@modificationDate", DbType.DateTime, project.modificationDate!.Value)
            });
            return Convert.ToInt32(projectIdUpdated.Value);
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

    public bool deleteProjectById(int id)
    {
        try
        {
            base._providerDB.ExecuteNonQuery("sp_deleteProjectById", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@projectId", DbType.Int32, id)
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

    public List<projectModel> getProjectsByTerm(string name)
    {
        try
        {
            return factoryGetProjectsByTerm.getList((DbDataReader)_providerDB.GetDataReader("sp_getProjectsByTerm", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@name", DbType.String, name)
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