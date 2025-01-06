namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetUsersByTerm: baseMethod<factoryGetUsersByTerm, userModel>
{
    private log _logger = new log();
    
    protected override userModel _getEntity(IDataReader dr)
    {
        try
        {
            return new userModel
            {
                id = conversionManager.toInt(dr["IDUSER"]),
                username = conversionManager.toString(dr["USERNAME"])
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}