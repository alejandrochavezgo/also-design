namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryExistClientByBusinessNameAndRfc: baseMethod<factoryExistClientByBusinessNameAndRfc, clientModel>
{
    private log _logger = new log();

    protected override clientModel _getEntity(IDataReader dr)
    {
        try
        {
            return new clientModel
            {
                id = conversionManager.toInt(dr["IDCLIENT"]),
                status = conversionManager.toInt(dr["IDSTATUS"]),
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