namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetPurchaseOrderItemsByIdPurchaseOrder: baseMethod<factoryGetPurchaseOrderItemsByIdPurchaseOrder, purchaseOrderItemsModel>
{
    private log _logger = new log();

    protected override purchaseOrderItemsModel _getEntity(IDataReader dr)
    {
        try
        {
            return new purchaseOrderItemsModel
            {
                id = conversionManager.toInt(dr["IDPURCHASEORDERSITEM"]),
                purchaseOrderId = conversionManager.toInt(dr["IDPURCHASEORDER"]),
                description = conversionManager.toString(dr["DESCRIPTION"]),
                material = conversionManager.toString(dr["MATERIAL"]),
                details = conversionManager.toString(dr["DETAILS"]),
                imagePath = conversionManager.toString(dr["IMAGEPATH"]),
                notes = conversionManager.toString(dr["NOTES"]),
                quantity = conversionManager.toDouble(dr["QUANTITY"]),
                unit = conversionManager.toInt(dr["IDPACKINGUNITTYPE"]),
                unitDescription = conversionManager.toString(dr["PACKINGUNITTYPES.DESCRIPTION"]),
                unitValue = conversionManager.toDecimal(dr["UNITVALUE"]),
                totalValue = conversionManager.toDecimal(dr["TOTALVALUE"]),
                inventoryItemId = conversionManager.toInt(dr["IDINVENTORY"])
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    private string getStatusColor(int statusId)
    {
        try
        {
            var status = string.Empty;
            switch (statusId)
            {
                case 1:
                    status = "success";
                    break;
                case 2:
                    status = "danger";
                    break;
                default:
                    status = "dark";
                    break;
            }
            return status;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    private string getStatusName(int statusId)
    {
        try
        {
            var status = string.Empty;
            switch (statusId)
            {
                case 1:
                    status = "active";
                    break;
                case 2:
                    status = "inactive";
                    break;
                default:
                    status = "undefined";
                    break;
            }
            return status;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}