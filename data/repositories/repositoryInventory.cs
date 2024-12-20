namespace data.repositories;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using common.configurations;
using common.logging;
using data.factoryInstances;
using data.providerData;
using entities.enums;
using entities.models;
using Newtonsoft.Json;

public class repositoryInventory : baseRepository
{
    private log _logger;

    public repositoryInventory()
    {
        _logger = new log();
    }

    public List<inventoryListModel> getItemByTerm(string term)
    {
        try
        {
            return factoryGetItemByTerm.getList((DbDataReader)_providerDB.GetDataReader("sp_getInventoryByTerm", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@term", DbType.String, term),
            }));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<catalogModel> getStatusTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getStatusTypesCatalog", new DbParameter[] {}));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<catalogModel> getMaterialTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getMaterialTypesCatalog", new DbParameter[] {}));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<catalogModel> getFinishTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getFinishTypesCatalog", new DbParameter[] {}));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<catalogModel> getLengthUnitTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getLengthUnitTypesCatalog", new DbParameter[] {}));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<catalogModel> getWeightUnitTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getWeightUnitTypesCatalog", new DbParameter[] {}));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<catalogModel> getToleranceUnitTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getToleranceUnitTypesCatalog", new DbParameter[] {}));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<catalogModel> getWarehouseUnitTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getWarehouseUnitTypesCatalog", new DbParameter[] {}));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<catalogModel> getClassificationTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getClassificationTypesCatalog", new DbParameter[] {}));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<catalogModel> getCurrencyTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getCurrencyTypesCatalog", new DbParameter[] {}));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
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
            return factoryGetInventoryItems.getList((DbDataReader)_providerDB.GetDataReader("sp_getInventoryItems", new DbParameter[] {}));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<inventoryMovementModel> getInventoryMovementsByPurchaseOrderIdAndInventoryItemId(int inventoryItemId)
    {
        try
        {
            return factoryGetInventoryMovementsByPurchaseOrderIdAndInventoryItemId.getList((DbDataReader)_providerDB.GetDataReader("sp_getLastInventoryMovementsByPurchaseorderIdAndInventoryItemId", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB, "@inventoryItemId", DbType.Int32, inventoryItemId)
            }));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
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
            var inventoryItemIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@inventoryItemIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_addInventoryItem", new DbParameter[] {
                inventoryItemIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB, "@status", DbType.Int32, inventoryItem.status!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@itemCode", DbType.String, inventoryItem.itemCode!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@itemName", DbType.String, inventoryItem.itemName!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@description", DbType.String, inventoryItem.description!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@material", DbType.String, inventoryItem.material!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@finishType", DbType.Int32, inventoryItem.finishType!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@classificationType", DbType.Int32, inventoryItem.classificationType!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@diameter", DbType.Double, inventoryItem.diameter!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitDiameter", DbType.Int32, inventoryItem.unitDiameter!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@length", DbType.Double, inventoryItem.length!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitLength", DbType.Int32, inventoryItem.unitLength!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@weight", DbType.Double, inventoryItem.weight!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitWeight", DbType.Int32, inventoryItem.unitWeight!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@tolerance", DbType.Double, inventoryItem.tolerance!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitTolerance", DbType.Int32, inventoryItem.unitTolerance!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@warehouseLocation", DbType.Int32, inventoryItem.warehouseLocation!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@reorderQty", DbType.Int32, inventoryItem.reorderQty!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@creationDate", DbType.DateTime, inventoryItem.creationDate!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@notes", DbType.String, inventoryItem.notes!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@itemImagePath", DbType.String, inventoryItem.itemImagePath!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@bluePrintsPath", DbType.String, inventoryItem.bluePrintsPath!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@technicalSpecificationsPath", DbType.String, inventoryItem.technicalSpecificationsPath!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@quantity", DbType.Double, inventoryItem.quantity!)
            });
            return Convert.ToInt32(inventoryItemIdAdded.Value) > 0 ? true : false;
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
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
            return factoryGetItemInventoryById.get((DbDataReader)_providerDB.GetDataReader("sp_getInventoryItemById", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB,"@inventoryItemId", DbType.Int32, id)
            }));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<catalogModel> getPackingUnitTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getPackingUnitTypesCatalog", new DbParameter[]{}));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<inventoryMovementModel> getInventoryMovementsByPurchaseOrderIdAndInventoryId(int purchaseOrderId, int inventoryItemId)
    {
        try
        {
            return factoryGetInventoryMovements.getList((DbDataReader)_providerDB.GetDataReader("sp_getInventoryMovementsByPurchaseOrderIdAndInventoryId", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB,"@purchaseOrderId", DbType.Int32, purchaseOrderId),
                dataFactory.getObjParameter(configurationManager.providerDB,"@inventoryItemId", DbType.Int32, inventoryItemId)
            }));
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
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
            var inventoryItemIdUpdated = dataFactory.getObjParameter(configurationManager.providerDB, "@inventoryItemIdUpdated", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_updateInventoryItem", new DbParameter[] {
                inventoryItemIdUpdated,
                dataFactory.getObjParameter(configurationManager.providerDB, "@inventoryItemId", DbType.Int32, inventoryItem.id),
                dataFactory.getObjParameter(configurationManager.providerDB, "@status", DbType.Int32, inventoryItem.status!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@itemCode", DbType.String, inventoryItem.itemCode!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@itemName", DbType.String, inventoryItem.itemName!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@description", DbType.String, inventoryItem.description!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@material", DbType.String, inventoryItem.material!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@finishType", DbType.Int32, inventoryItem.finishType!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@classificationType", DbType.Int32, inventoryItem.classificationType!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@diameter", DbType.Double, inventoryItem.diameter!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitDiameter", DbType.Int32, inventoryItem.unitDiameter!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@length", DbType.Double, inventoryItem.length!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitLength", DbType.Int32, inventoryItem.unitLength!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@weight", DbType.Double, inventoryItem.weight!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitWeight", DbType.Int32, inventoryItem.unitWeight!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@tolerance", DbType.Double, inventoryItem.tolerance!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitTolerance", DbType.Int32, inventoryItem.unitTolerance!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@warehouseLocation", DbType.Int32, inventoryItem.warehouseLocation!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@reorderQty", DbType.Int32, inventoryItem.reorderQty!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@modificationDate", DbType.DateTime, inventoryItem.modificationDate!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@notes", DbType.String, inventoryItem.notes!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@itemImagePath", DbType.String, inventoryItem.itemImagePath!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@bluePrintsPath", DbType.String, inventoryItem.bluePrintsPath!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@technicalSpecificationsPath", DbType.String, inventoryItem.technicalSpecificationsPath!)
            });
            return Convert.ToInt32(inventoryItemIdUpdated.Value) > 0 ? true : false;
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
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
            var entryIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@entryIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_addInventoryEntry", new DbParameter[] {
                entryIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB, "@inventoryItemId", DbType.Int32, inventoryItemId),
                dataFactory.getObjParameter(configurationManager.providerDB, "@quantity", DbType.Double, quantity),
                dataFactory.getObjParameter(configurationManager.providerDB, "@entryDateTime", DbType.DateTime, entryDateTime)
            });
            return Convert.ToInt32(entryIdAdded.Value);
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public int addMovement(int inventoryItemId, inventoryMovementType inventoryMovementType, int purchaseOrderId, int userId, double quantity, int unit, string comments, decimal unitValue, decimal totalValue, DateTime entryDateTime)
    {
        try
        {
            var movementIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@movementIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_addInventoryMovement", new DbParameter[] {
                movementIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB, "@inventoryItemId", DbType.Int32, inventoryItemId),
                dataFactory.getObjParameter(configurationManager.providerDB, "@inventoryMovementType", DbType.Int32, (int)inventoryMovementType),
                dataFactory.getObjParameter(configurationManager.providerDB, "@purchaseOrderId", DbType.Int32, purchaseOrderId),
                dataFactory.getObjParameter(configurationManager.providerDB, "@userId", DbType.Int32, userId),
                dataFactory.getObjParameter(configurationManager.providerDB, "@quantity", DbType.Double, quantity),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unit", DbType.Int32, unit),
                dataFactory.getObjParameter(configurationManager.providerDB, "@comments", DbType.String, comments),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitValue", DbType.Decimal, unitValue),
                dataFactory.getObjParameter(configurationManager.providerDB, "@totalValue", DbType.Decimal, totalValue),
                dataFactory.getObjParameter(configurationManager.providerDB, "@entryDateTime", DbType.DateTime, entryDateTime)
            });
            return Convert.ToInt32(movementIdAdded.Value);
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
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
            base._providerDB.ExecuteNonQuery("sp_deleteItemInventoryById", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@inventoryItemId", DbType.Int32, id),
            });
            return true;
        }
        catch (SqlException SqlException)
        {
            _logger.logError($"{JsonConvert.SerializeObject(SqlException)}");
            throw SqlException;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}