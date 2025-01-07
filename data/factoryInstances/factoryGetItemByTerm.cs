namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetItemByTerm: baseMethod<factoryGetItemByTerm, inventoryListModel>
{
    private log _logger = new log();
    
    protected override inventoryListModel _getEntity(IDataReader dr)
    {
        try
        {
            return new inventoryListModel
            {
                id = conversionManager.toInt(dr["IDINVENTORY"]),
                itemCode = conversionManager.toString(dr["CODE"]),
                itemName = conversionManager.toString(dr["NAME"]),
                quantity = conversionManager.toDecimal(dr["QUANTITY"]),
                itemDescription = conversionManager.toString(dr["DESCRIPTION"])
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}