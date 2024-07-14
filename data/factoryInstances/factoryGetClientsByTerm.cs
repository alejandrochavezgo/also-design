namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetClientsByTerm: baseMethod<factoryGetClientsByTerm, clientModel>
{
    private log _logger = new log();
    
    protected override clientModel _getEntity(IDataReader dr)
    {
        try
        {
            return new clientModel
            {
                id = conversionManager.toInt(dr["IDCLIENT"]),
                businessName = conversionManager.toString(dr["BUSINESSNAME"]),
                city = conversionManager.toString(dr["CITY"]),
                address = conversionManager.toString(dr["ADDRESS"]),
                rfc = conversionManager.toString(dr["RFC"])
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}