namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using Newtonsoft.Json;

internal class factoryGetSupplierContactPhonesBySupplierId: baseMethod<factoryGetSupplierContactPhonesBySupplierId, string>
{
    private log _logger = new log();

    protected override string _getEntity(IDataReader dr)
    {
        try
        {
            return conversionManager.toString(dr["PHONE"]);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}