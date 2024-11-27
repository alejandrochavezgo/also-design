namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.enums;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetClientTracesByClientId: baseMethod<factoryGetClientTracesByClientId, traceModel>
{
    private log _logger = new log();
    
    protected override traceModel _getEntity(IDataReader dr)
    {
        try
        {
            return new traceModel
            {
                id = conversionManager.toInt(dr["IDTRACE"]),
                entityType = (entityType)conversionManager.toInt(dr["IDENTITYTYPE"]),
                entityTypeDescription = conversionManager.toString(dr["entityTypes.DESCRIPTION"]),
                traceType = (traceType) conversionManager.toInt(dr["IDTRACETYPE"]),
                traceTypeDescription = conversionManager.toString(dr["traceTypes.DESCRIPTION"]),
                entityId = conversionManager.toInt(dr["IDENTITY"]),
                username = conversionManager.toString(dr["USERNAME"]),
                userId = conversionManager.toInt(dr["IDUSER"]),
                comments = conversionManager.toString(dr["COMMENTS"]),
                beforeChange = !string.IsNullOrEmpty(conversionManager.toString(dr["BEFORECHANGE"])) ? conversionManager.toString(dr["BEFORECHANGE"]) : "-",
                afterChange = !string.IsNullOrEmpty(conversionManager.toString(dr["AFTERCHANGE"])) ? conversionManager.toString(dr["AFTERCHANGE"]) : "-",
                creationDate = conversionManager.toValidDate(dr["CREATIONDATE"]),
                creationDateAsString = conversionManager.toValidDate(dr["CREATIONDATE"]) > DateTime.MinValue ? conversionManager.toValidDate(dr["CREATIONDATE"]).ToString("yyyy-MM-dd hh:mm:ss") : "-",
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}