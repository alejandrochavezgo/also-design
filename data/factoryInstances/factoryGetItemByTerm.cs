namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetItemByTerm: baseMethod<factoryGetItemByTerm, inventoryModel>
{
    private log _logger = new log();
    
    protected override inventoryModel _getEntity(IDataReader dr)
    {
        try
        {
            return new inventoryModel
            {
                id = conversionManager.toInt(dr["IDINVENTORY"]),
                description = conversionManager.toString(dr["DESCRIPTION"]),
                code = conversionManager.toString(dr["CODE"]),
                unit = conversionManager.toString(dr["UNIT"]),
                unitValue = conversionManager.toDecimal(dr["UNITVALUE"])
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}