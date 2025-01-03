namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetProjectsByTerm: baseMethod<factoryGetProjectsByTerm, projectModel>
{
    private log _logger = new log();
    
    protected override projectModel _getEntity(IDataReader dr)
    {
        try
        {
            return new projectModel
            {
                id = conversionManager.toInt(dr["IDPROJECT"]),
                name = conversionManager.toString(dr["NAME"])
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}