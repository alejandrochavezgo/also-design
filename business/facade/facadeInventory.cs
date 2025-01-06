namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;
using entities.enums;
using common.helpers;
using System.Transactions;

public class facadeInventory
{
    private log _logger;
    private repositoryInventory _repositoryInventory;
    private DateTime _dateTime;

    public facadeInventory()
    {
        _logger = new log();
        _repositoryInventory = new repositoryInventory();
        _dateTime = new dateHelper().pstNow();
    }

    public List<inventoryListModel> getItemByTerm(string term)
    {
        try
        {
            return _repositoryInventory.getItemByTerm(term);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<inventoryListModel> getInventoryItems()
    {
        try
        {
            return _repositoryInventory.getInventoryItems();
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<inventoryMovementModel> getInventoryMovementsByInventoryItemId(int inventoryItemId)
    {
        try
        {
            return _repositoryInventory.getInventoryMovementsByInventoryItemId(inventoryItemId);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool addInventoryItem(inventoryItemModel inventoryItem)
    {
        try
        {
            var today = _dateTime;
            inventoryItem.quantity = 0;
            inventoryItem.creationDate = today;
            inventoryItem.itemImagePath = string.IsNullOrEmpty(inventoryItem.itemImagePath) ? string.Empty : inventoryItem.itemImagePath;
            inventoryItem.bluePrintsPath = string.IsNullOrEmpty(inventoryItem.bluePrintsPath) ? string.Empty : inventoryItem.bluePrintsPath;
            inventoryItem.technicalSpecificationsPath = string.IsNullOrEmpty(inventoryItem.technicalSpecificationsPath) ? string.Empty : inventoryItem.technicalSpecificationsPath;
            return _repositoryInventory.addInventoryItem(inventoryItem);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<List<catalogModel>> getAllInventoryCatalogs()
    {
        try
        {
            var catalogs = new List<List<catalogModel>>();
            catalogs.Add(_repositoryInventory.getStatusTypesCatalog().Take(2).ToList());
            catalogs.Add(_repositoryInventory.getMaterialTypesCatalog());
            catalogs.Add(_repositoryInventory.getFinishTypesCatalog());
            catalogs.Add(_repositoryInventory.getLengthUnitTypesCatalog());
            catalogs.Add(_repositoryInventory.getWeightUnitTypesCatalog());
            catalogs.Add(_repositoryInventory.getToleranceUnitTypesCatalog());
            catalogs.Add(_repositoryInventory.getWarehouseUnitTypesCatalog());
            catalogs.Add(_repositoryInventory.getClassificationTypesCatalog());
            return catalogs;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public inventoryItemModel getItemInventoryById(int id)
    {
        try
        {
            return _repositoryInventory.getItemInventoryById(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<catalogModel> getCatalogByName(string name)
    {
        try
        {
            var catalog = new List<catalogModel>();
            switch (name.ToUpper())
            {
                case "PACKINGUNITTYPES":
                    catalog = _repositoryInventory.getPackingUnitTypesCatalog();
                    break;
                default:
                    catalog = null;
                    break;
            }
            return catalog;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<inventoryMovementModel> getInventoryMovementsByPurchaseOrderIdAndInventoryId(int purchaseOrderItemId, int purchaseOrderId, int inventoryItemId)
    {
        try
        {
            return _repositoryInventory.getInventoryMovementsByPurchaseOrderIdAndInventoryId(purchaseOrderItemId, purchaseOrderId, inventoryItemId);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public int addEntry(int inventoryItemId, double quantity, DateTime entryDateTime)
    {
        try
        {
            return _repositoryInventory.addEntry(inventoryItemId, quantity, entryDateTime);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public int addExit(int inventoryItemId, double quantityToRelease)
    {
        try
        {
            return _repositoryInventory.addExit(inventoryItemId, quantityToRelease, _dateTime);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public int addMovement(int purchaseOrderItemId, int inventoryItemId, inventoryMovementType inventoryMovementType, int purchaseOrderId, int userId, double quantity, int unit, string comments, decimal unitValue, decimal totalValue, DateTime transactionDateTime, int projectId, int receivingUserId)
    {
        try
        {
            return _repositoryInventory.addMovement(purchaseOrderItemId, inventoryItemId, inventoryMovementType, purchaseOrderId, userId, quantity, unit, comments, unitValue, totalValue, transactionDateTime, projectId, receivingUserId);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool updateInventoryItem(inventoryItemModel inventoryItem)
    {
        try
            {
                inventoryItem.modificationDate = _dateTime;
                if(inventoryItem.itemImageHasOriginalImage)
                    inventoryItem.itemImagePath = string.Empty;
                else
                    inventoryItem.itemImagePath = inventoryItem.itemImageHasNewImage ? inventoryItem.itemImagePath : inventoryItem.itemDefaultImagePath;

                if(inventoryItem.bluePrintsHasOriginalDocument)
                    inventoryItem.bluePrintsPath = string.Empty;
                else if (inventoryItem.bluePrintsHasNotDocument)
                    inventoryItem.bluePrintsPath = "EMPTY";

                if(inventoryItem.technicalSpecificationsHasOriginalDocument)
                    inventoryItem.technicalSpecificationsPath = string.Empty;
                else if (inventoryItem.technicalSpecificationsHasNotDocument)
                    inventoryItem.technicalSpecificationsPath = "EMPTY";

                return _repositoryInventory.updateInventoryItem(inventoryItem);
            }
            catch (Exception exception)
            {
                _logger.logError($"{JsonConvert.SerializeObject(exception)}");
                throw exception;
            }
    }

    public bool deleteItemInventoryById(int id)
    {
        try
        {
            var inventoryItem = _repositoryInventory.getItemInventoryById(id);
            if (inventoryItem == null || inventoryItem.quantity <= 0)
                return false;
            return _repositoryInventory.deleteItemInventoryById(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool updateInventoryStockByInventoryItemId(inventoryReleaseModel inventoryRelease)
    {
        using (TransactionScope transactionScope = new TransactionScope())
        {
            try
            {
                var inventoryItem = _repositoryInventory.getItemInventoryById(inventoryRelease.id);
                if (inventoryItem == null || inventoryItem.status != (int)statusType.ACTIVE || inventoryItem.quantity <= 0 || inventoryRelease.quantityToRelease > inventoryItem.quantity)
                {
                    transactionScope.Dispose();
                    return false;
                }

                if (_repositoryInventory.addExit(inventoryItem.id, inventoryRelease.quantityToRelease, _dateTime) > 0 &&
                    _repositoryInventory.addMovement(0, inventoryItem.id, inventoryMovementType.EXIT, 0, inventoryRelease.deliveringUserId, -1 * inventoryRelease.quantityToRelease, 0, inventoryRelease.comments, 0, 0, _dateTime, inventoryRelease.projectId, inventoryRelease.receivingUserId) > 0)
                {
                    transactionScope.Complete();
                    return true;
                }
                else
                {
                    transactionScope.Dispose();
                    return false;
                }
            }
            catch (Exception exception)
            {
                transactionScope.Dispose();
                _logger.logError($"{JsonConvert.SerializeObject(exception)}");
                return false;
            }
        }
    }
}