namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;

public class facadeInventory
{
    private log _logger;
    private repositoryInventory _repositoryInventory;

    public facadeInventory()
    {
        _logger = new log();
        _repositoryInventory = new repositoryInventory();
    }

    public List<inventoryModel> getItemByTerm(string description)
    {
        try
        {
            return _repositoryInventory.getItemByTerm(description);
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

    public bool addInventoryItem(inventoryItemModel inventoryItem)
    {
        try
        {
            inventoryItem.creationDate = DateTime.Now;
            inventoryItem.lastRestockDate = DateTime.Now;
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
            catalogs.Add(_repositoryInventory.getPackingUnitTypesCatalog());
            catalogs.Add(_repositoryInventory.getCurrencyTypesCatalog());
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

    public bool updateInventoryItem(inventoryItemModel inventoryItem)
    {
        try
            {
                inventoryItem.modificationDate = DateTime.Now;
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
}