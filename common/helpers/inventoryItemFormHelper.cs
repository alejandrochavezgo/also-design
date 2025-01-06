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
                inventoryItem.status <= 0 || string.IsNullOrEmpty(inventoryItem.description))
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
                inventoryItem.status <= 0 || string.IsNullOrEmpty(inventoryItem.description) || inventoryItem.id <= 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public bool isUpdateFormValid(inventoryItemModel inventoryItem, bool isStatusChange)
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

    public bool isUpdateFormValid(inventoryReleaseModel inventoryRelease, bool isStatusChange, bool isStockValid)
    {
        try
        {
            if (inventoryRelease == null || inventoryRelease.id <= 0 || inventoryRelease.quantityToRelease <= 0 || inventoryRelease.stock <= 0 ||
                inventoryRelease.quantityToRelease > inventoryRelease.stock || inventoryRelease.projectId <= 0 || inventoryRelease.deliveringUserId <=0 ||
                inventoryRelease.receivingUserId <= 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
}