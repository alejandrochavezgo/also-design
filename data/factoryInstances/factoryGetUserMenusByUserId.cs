namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetUserMenusByUserId: baseMethod<factoryGetUserMenusByUserId, menuModel>
{
    private log _logger = new log();
    
    protected override menuModel _getEntity(IDataReader dr)
    {
        try
        {
            return new menuModel
            {
                id = conversionManager.toInt(dr["IDMENU"]),
                description = conversionManager.toString(dr["DESCRIPTION"]),
                path = conversionManager.toString(dr["PATH"])
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}