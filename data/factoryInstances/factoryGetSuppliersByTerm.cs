namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetSuppliersByTerm: baseMethod<factoryGetSuppliersByTerm, supplierModel>
{
    private log _logger = new log();
    
    protected override supplierModel _getEntity(IDataReader dr)
    {
        try
        {
            return new supplierModel
            {
                id = conversionManager.toInt(dr["IDSUPPLIER"]),
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