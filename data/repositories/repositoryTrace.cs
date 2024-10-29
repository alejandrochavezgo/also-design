namespace data.repositories;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using common.configurations;
using common.logging;
using data.factoryInstances;
using data.providerData;
using entities.models;
using Newtonsoft.Json;

public class repositoryTrace : baseRepository
{
    private log _logger;

    public repositoryTrace()
    {
        _logger = new log();
    }

    public int addTrace(traceModel trace)
    {
        try
        {
            var traceIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@traceIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_addTrace", new DbParameter[] {
                traceIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@traceType", DbType.Int32, (int)trace!.traceType),
                dataFactory.getObjParameter(configurationManager.providerDB,"@entityType", DbType.Int32, (int)trace!.entityType),
                dataFactory.getObjParameter(configurationManager.providerDB,"@userId", DbType.Int32, trace!.userId),
                dataFactory.getObjParameter(configurationManager.providerDB,"@comments", DbType.String, trace!.comments!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@beforeChange", DbType.String, trace!.beforeChange!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@afterChange", DbType.String, trace!.afterChange!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@entityId", DbType.Int32, (int)trace!.entityId)
            });
            return Convert.ToInt32(traceIdAdded.Value);
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