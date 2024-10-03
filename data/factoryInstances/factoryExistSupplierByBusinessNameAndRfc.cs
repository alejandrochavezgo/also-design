namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryExistSupplierByBusinessNameAndRfc: baseMethod<factoryExistSupplierByBusinessNameAndRfc, supplierModel>
{
    private log _logger = new log();

    protected override supplierModel _getEntity(IDataReader dr)
    {
        try
        {
            return new supplierModel
            {
                id = conversionManager.toInt(dr["IDSUPPLIER"]),
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