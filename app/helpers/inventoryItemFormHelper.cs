namespace app.helpers;

using entities.models;

public static class inventoryItemFormHelper
{
    public static bool isAddFormValid(inventoryItemModel inventoryItem)
    {
        try
        {
            if (string.IsNullOrEmpty(inventoryItem.itemCode) || string.IsNullOrEmpty(inventoryItem.itemName) ||
                inventoryItem.status <= 0 || string.IsNullOrEmpty(inventoryItem.description) || inventoryItem.material <= 0 ||
                inventoryItem.finishType <= 0 || inventoryItem.classificationType <= 0 || inventoryItem.unitDiameter <= 0 || inventoryItem.length <= 0 || inventoryItem.unitLength <= 0 ||
                inventoryItem.weight <= 0 || inventoryItem.unitWeight <= 0 || inventoryItem.tolerance <= 0 || inventoryItem.unitTolerance <= 0 ||
                inventoryItem.warehouseLocation <= 0 || inventoryItem.reorderQty <= 0 || string.IsNullOrEmpty(inventoryItem.notes))
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
    
    public static bool isUpdateFormValid(inventoryItemModel inventoryItem)
    {
        try
        {
            if (string.IsNullOrEmpty(inventoryItem.itemCode) || string.IsNullOrEmpty(inventoryItem.itemName) ||
                inventoryItem.status <= 0 || string.IsNullOrEmpty(inventoryItem.description) || inventoryItem.material <= 0 ||
                inventoryItem.finishType <= 0 || inventoryItem.classificationType <= 0 || inventoryItem.unitDiameter <= 0 || inventoryItem.length <= 0 || inventoryItem.unitLength <= 0 ||
                inventoryItem.weight <= 0 || inventoryItem.unitWeight <= 0 || inventoryItem.tolerance <= 0 || inventoryItem.unitTolerance <= 0 ||
                inventoryItem.warehouseLocation <= 0 || inventoryItem.reorderQty <= 0 || string.IsNullOrEmpty(inventoryItem.notes) || inventoryItem.id <= 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }


    public static bool isUpdateFormValid(inventoryItemModel inventoryItem, bool isStatusChange)
    {
        try
        {
            if (inventoryItem == null || inventoryItem.id <= 0 || inventoryItem.quantity > 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
}