namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetWorkOrderItemsByWorkOrderId: baseMethod<factoryGetWorkOrderItemsByWorkOrderId, workOrderItemModel>
{
    private log _logger = new log();

    protected override workOrderItemModel _getEntity(IDataReader dr)
    {
        try
        {
            return new workOrderItemModel
            {
                id = conversionManager.toInt(dr["WORKORDERSITEMS.IDWORKORDEROITEM"]),
                workOrderId = conversionManager.toInt(dr["WORKORDERSITEMS.IDWORKORDER"]),
                toolNumber = conversionManager.toString(dr["WORKORDERSITEMS.TOOLNUMBER"]),
                inventoryItemId = conversionManager.toInt(dr["INVETORY.IDINVENTORY"]),
                inventoryItemDescription = conversionManager.toString(dr["INVETORY.DESCRIPTION"]),
                quantityInStock = conversionManager.toInt(dr["INVETORY.QUANTITY"]),
                quantity = conversionManager.toInt(dr["WORKORDERSITEMS.QUANTITY"]),
                routes = JsonConvert.DeserializeObject<List<string>>(conversionManager.toString(dr["WORKORDERSITEMS.ROUTES"])),
                comments = conversionManager.toString(dr["WORKORDERSITEMS.COMMENTS"])
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}