namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetUserRolesByUserId: baseMethod<factoryGetUserRolesByUserId, roleModel>
{
    private log _logger = new log();

    protected override roleModel _getEntity(IDataReader dr)
    {
        try
        {
            return new roleModel
            {
                id = conversionManager.toInt(dr["IDROLE"]),
                description = conversionManager.toString(dr["DESCRIPTION"])
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}