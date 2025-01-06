namespace data.factoryInstances;

using System.Data;
using common.conversions;
using common.logging;
using entities.models;
using Newtonsoft.Json;

internal class factoryGetInventoryMovementsByInventoryItemId: baseMethod<factoryGetInventoryMovementsByInventoryItemId, inventoryMovementModel>
{
    private log _logger = new log();
    
    protected override inventoryMovementModel _getEntity(IDataReader dr)
    {
        try
        {
            return new inventoryMovementModel
            {
                id = conversionManager.toInt(dr["IDINVENTORYMOVEMENT"]),
                quantity = conversionManager.toDouble(dr["QUANTITY"]),
                comments = conversionManager.toString(dr["COMMENTS"]),
                creationDate = conversionManager.toValidDate(dr["CREATIONDATE"]),
                unitValue = conversionManager.toDecimal(dr["UNITVALUE"]),
                totalValue = conversionManager.toDecimal(dr["TOTALVALUE"]),
                approvedDeliveredUsername = conversionManager.toString(dr["APPROVEDDELIVEREDUSERNAME"]),
                receivedUsername = conversionManager.toString(dr["RECEIVEDUSERNAME"]),
                projectName = conversionManager.toString(dr["PROJECTNAME"]),
                packingUnitTypeDescription = conversionManager.toString(dr["PACKINGUNITTYPES.DESCRIPTION"]),
                inventoryMovementTypeDescription = conversionManager.toString(dr["INVENTORYMOVEMENTS.DESCRIPTION"])
            };
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}