namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.enums;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetInventoryMovements: baseMethod<factoryGetInventoryMovements, inventoryMovementModel>
{
    private log _logger = new log();

    protected override inventoryMovementModel _getEntity(IDataReader dr)
    {
        try
        {
            return new inventoryMovementModel
            {
                id = conversionManager.toInt(dr["IDINVENTORYMOVEMENT"]),
                inventoryMovementType = (inventoryMovementType)conversionManager.toInt(dr["IDINVENTORYMOVEMENTTYPE"]),
                purchaseOrderId = conversionManager.toInt(dr["IDPURCHASEORDER"]),
                purchaseOrderItemId = conversionManager.toInt(dr["IDPURCHASEORDERSITEM"]),
                inventoryItemId = conversionManager.toInt(dr["IDINVENTORY"]),
                userId = conversionManager.toInt(dr["IDUSER"]),
                quantity = conversionManager.toDouble(dr["QUANTITY"]),
                packingUnitType = (packingUnitType)conversionManager.toInt(dr["IDPACKINGUNITTYPE"]),
                comments = conversionManager.toString(dr["COMMENTS"]),
                creationDate = conversionManager.toValidDate(dr["CREATIONDATE"]),
                unitValue = conversionManager.toDecimal(dr["UNITVALUE"]),
                totalValue = conversionManager.toDecimal(dr["TOTALVALUE"])
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}