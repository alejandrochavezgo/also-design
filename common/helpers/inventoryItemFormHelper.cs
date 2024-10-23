namespace common.helpers;

using common.logging;
using entities.models;
using Newtonsoft.Json;

public class inventoryItemFormHelper
{
    private log _logger;

    public inventoryItemFormHelper ()
    {
        _logger = new log();
    }

    public bool isAddFormValid(inventoryItemModel inventoryItem)
    {
        try
        {
            if (string.IsNullOrEmpty(inventoryItem.itemCode) || string.IsNullOrEmpty(inventoryItem.itemName) ||
                inventoryItem.status <= 0 || string.IsNullOrEmpty(inventoryItem.description) || inventoryItem.material <= 0 ||
                inventoryItem.finishType <= 0 || inventoryItem.unitDiameter <= 0 || inventoryItem.length <= 0 || inventoryItem.unitLength <= 0 ||
                inventoryItem.weight <= 0 || inventoryItem.unitWeight <= 0 || inventoryItem.tolerance <= 0 || inventoryItem.unitTolerance <= 0 ||
                inventoryItem.warehouseLocation <= 0 || inventoryItem.quantity <= 0 || inventoryItem.reorderQty <= 0 || inventoryItem.unit <= 0 ||
                inventoryItem.currency <= 0 || inventoryItem.unitValue <= 0 || inventoryItem.totalValue <= 0 || string.IsNullOrEmpty(inventoryItem.notes))
                return false;
            return true;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool isUpdateFormValid(inventoryItemModel inventoryItem)
    {
        try
        {
            if (string.IsNullOrEmpty(inventoryItem.itemCode) || string.IsNullOrEmpty(inventoryItem.itemName) ||
                inventoryItem.status <= 0 || string.IsNullOrEmpty(inventoryItem.description) || inventoryItem.material <= 0 ||
                inventoryItem.finishType <= 0 || inventoryItem.unitDiameter <= 0 || inventoryItem.length <= 0 || inventoryItem.unitLength <= 0 ||
                inventoryItem.weight <= 0 || inventoryItem.unitWeight <= 0 || inventoryItem.tolerance <= 0 || inventoryItem.unitTolerance <= 0 ||
                inventoryItem.warehouseLocation <= 0 || inventoryItem.quantity <= 0 || inventoryItem.reorderQty <= 0 || inventoryItem.unit <= 0 ||
                inventoryItem.currency <= 0 || inventoryItem.unitValue <= 0 || inventoryItem.totalValue <= 0 || string.IsNullOrEmpty(inventoryItem.notes) || inventoryItem.id <= 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
}