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

public class repositoryQuotation : baseRepository
{
    private log _logger;

    public repositoryQuotation()
    {
        _logger = new log();
    }

    public List<quotationModel> getQuotations()
    {
        try
        {
            return factoryGetQuotations.getList((DbDataReader)_providerDB.GetDataReader("sp_getQuotations", new DbParameter[] {}));
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