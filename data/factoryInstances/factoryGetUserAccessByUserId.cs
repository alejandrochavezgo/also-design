namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using Newtonsoft.Json;

internal class factoryGetUserAccessByUserId: baseMethod<factoryGetUserAccessByUserId, string>
{
    private log _logger = new log();

    protected override string _getEntity(IDataReader dr)
    {
        try
        {
            return conversionManager.toString(dr["MENUS.DESCRIPTION"]);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}