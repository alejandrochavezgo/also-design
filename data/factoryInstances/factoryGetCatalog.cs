namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetCatalog: baseMethod<factoryGetCatalog, catalogModel>
{
    private log _logger = new log();

    protected override catalogModel _getEntity(IDataReader dr)
    {
        try
        {
            return new catalogModel
            {
                id = conversionManager.toInt(dr["IDCATALOG"]),
                description = conversionManager.toString(dr["DESCRIPTION"]),
                status = conversionManager.toInt(dr["IDSTATUS"])
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}