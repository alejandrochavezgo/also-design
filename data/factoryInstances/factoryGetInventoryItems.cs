namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetInventoryItems: baseMethod<factoryGetInventoryItems, inventoryListModel>
{
    private log _logger = new log();

    protected override inventoryListModel _getEntity(IDataReader dr)
    {
        try
        {
            var lastRestockDate = conversionManager.toValidDate(dr["LASTRESTOCKDATE"]) <= DateTime.MinValue ? "-" : conversionManager.toValidDate(dr["LASTRESTOCKDATE"]).ToString("yyyy-MM-dd hh:mm:ss");
            return new inventoryListModel
            {
                itemName = conversionManager.toString(dr["NAME"]),
                itemCode = conversionManager.toString(dr["CODE"]),
                itemImagePath = conversionManager.toString(dr["ITEMIMAGEPATH"]),
                idQuantityLastRestockDate = $"{conversionManager.toInt(dr["IDINVENTORY"])}&{conversionManager.toString(dr["QUANTITY"])}&{lastRestockDate}"
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}