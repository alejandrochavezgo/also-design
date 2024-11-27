namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.enums;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetPurchaseOrderTraceById: baseMethod<factoryGetPurchaseOrderTraceById, traceModel>
{
    private log _logger = new log();
    
    protected override traceModel _getEntity(IDataReader dr)
    {
        try
        {
            return new traceModel
            {
                id = conversionManager.toInt(dr["IDTRACE"]),
                beforeChange = !string.IsNullOrEmpty(conversionManager.toString(dr["BEFORECHANGE"])) ? conversionManager.toString(dr["BEFORECHANGE"]) : "-",
                afterChange = !string.IsNullOrEmpty(conversionManager.toString(dr["AFTERCHANGE"])) ? conversionManager.toString(dr["AFTERCHANGE"]) : "-",
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}