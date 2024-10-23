namespace data.repositories;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using common.configurations;
using common.logging;
using data.factoryInstances;
using data.providerData;
using entities.models;
using Newtonsoft.Json;

public class repositoryInventory : baseRepository
{
    private log _logger;

    public repositoryInventory()
    {
        _logger = new log();
    }

    public List<inventoryModel> getItemByTerm(string description)
    {
        try
        {
            return factoryGetItemByTerm.getList((DbDataReader)_providerDB.GetDataReader("sp_getInventoryByTerm", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@description", DbType.String, description),
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

    public List<catalogModel> getPackingUnitTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getPackingUnitTypesCatalog", new DbParameter[] {}));
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
                dataFactory.getObjParameter(configurationManager.providerDB, "@diameter", DbType.Double, inventoryItem.diameter!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitDiameter", DbType.Int32, inventoryItem.unitDiameter!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@length", DbType.Double, inventoryItem.length!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitLength", DbType.Int32, inventoryItem.unitLength!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@weight", DbType.Double, inventoryItem.weight!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitWeight", DbType.Int32, inventoryItem.unitWeight!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@tolerance", DbType.Double, inventoryItem.tolerance!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitTolerance", DbType.Int32, inventoryItem.unitTolerance!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@warehouseLocation", DbType.Int32, inventoryItem.warehouseLocation!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@quantity", DbType.Int32, inventoryItem.quantity!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@reorderQty", DbType.Int32, inventoryItem.reorderQty!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unit", DbType.Int32, inventoryItem.unit!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@currency", DbType.Int32, inventoryItem.currency!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitValue", DbType.Decimal, inventoryItem.unitValue!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@totalValue", DbType.Decimal, inventoryItem.totalValue!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@creationDate", DbType.DateTime, inventoryItem.creationDate!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@lastRestockDate", DbType.DateTime, inventoryItem.lastRestockDate!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@notes", DbType.String, inventoryItem.notes!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@itemImagePath", DbType.String, inventoryItem.itemImagePath!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@bluePrintsPath", DbType.String, inventoryItem.bluePrintsPath!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@technicalSpecificationsPath", DbType.String, inventoryItem.technicalSpecificationsPath!)
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
                dataFactory.getObjParameter(configurationManager.providerDB, "@diameter", DbType.Double, inventoryItem.diameter!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitDiameter", DbType.Int32, inventoryItem.unitDiameter!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@length", DbType.Double, inventoryItem.length!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitLength", DbType.Int32, inventoryItem.unitLength!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@weight", DbType.Double, inventoryItem.weight!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitWeight", DbType.Int32, inventoryItem.unitWeight!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@tolerance", DbType.Double, inventoryItem.tolerance!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitTolerance", DbType.Int32, inventoryItem.unitTolerance!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@warehouseLocation", DbType.Int32, inventoryItem.warehouseLocation!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@quantity", DbType.Int32, inventoryItem.quantity!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@reorderQty", DbType.Int32, inventoryItem.reorderQty!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unit", DbType.Int32, inventoryItem.unit!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@currency", DbType.Int32, inventoryItem.currency!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@unitValue", DbType.Decimal, inventoryItem.unitValue!),
                dataFactory.getObjParameter(configurationManager.providerDB, "@totalValue", DbType.Decimal, inventoryItem.totalValue!),
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
}