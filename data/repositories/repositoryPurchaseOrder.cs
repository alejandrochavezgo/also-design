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

public class repositoryPurchaseOrder : baseRepository
{
    private log _logger;

    public repositoryPurchaseOrder()
    {
        _logger = new log();
    }

    public List<catalogModel> getCurrencyTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getCurrencyTypesCatalog", new DbParameter[]{}));
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

    public List<catalogModel> getPaymentTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getPaymentTypesCatalog", new DbParameter[]{}));
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

    public List<purchaseOrderModel> getPurchaseOrders()
    {
        try
        {
            return factoryGetPurchaseOrders.getList((DbDataReader)_providerDB.GetDataReader("sp_getPurchaseOrders", new DbParameter[] {}));
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

    public purchaseOrderModel getPurchaseOrderById(int id)
    {
        try
        {
            return factoryGetPurchaseOrderById.get((DbDataReader)_providerDB.GetDataReader("sp_getPurchaseOrderById", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB,"@purchaseOrderId", DbType.Int32, id),
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

    public List<purchaseOrderItemsModel> getPurchaseOrderItemsByPurchaseOrderId(int id)
    {
        try
        {
            return factoryGetPurchaseOrderItemsByIdPurchaseOrder.getList((DbDataReader)_providerDB.GetDataReader("sp_getPurchaseOrderItemsByPurchaseOrderId", new DbParameter[]
            {
                dataFactory.getObjParameter(configurationManager.providerDB,"@purchaseOrderId", DbType.Int32, id),
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

    public int addPurchaseOrder(purchaseOrderModel purchaseOrder)
    {
        try
        {
            var purchaseOrderIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@purchaseOrderIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_addPurchaseOrder", new DbParameter[] {
                purchaseOrderIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@supplierId", DbType.Int32, purchaseOrder.supplier!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@userId", DbType.Int32, purchaseOrder.user!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@status", DbType.Int32, purchaseOrder.status),
                dataFactory.getObjParameter(configurationManager.providerDB,"@paymentId", DbType.Int32, purchaseOrder.payment!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@currencyId", DbType.Int32, purchaseOrder.currency!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@subtotal", DbType.Decimal, purchaseOrder.subtotal!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@taxRate", DbType.Decimal, purchaseOrder.taxRate!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@taxAmount", DbType.Decimal, purchaseOrder.taxAmount!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@totalAmount", DbType.Decimal, purchaseOrder.totalAmount!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@creationDate", DbType.DateTime, purchaseOrder.creationDate),
                dataFactory.getObjParameter(configurationManager.providerDB,"@userMainContactPhone", DbType.String, purchaseOrder.user.employee!.mainContactPhone!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@supplierMainContactName", DbType.String, purchaseOrder.supplier!.mainContactName!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@supplierMainContactPhone", DbType.String, purchaseOrder.supplier!.mainContactPhone!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@generalNotes", DbType.String, purchaseOrder.generalNotes!)
            });
            return Convert.ToInt32(purchaseOrderIdAdded.Value);
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

    public int updatePurchaseOrder(purchaseOrderModel purchaseOrder)
    {
        try
        {
            var purchaseOrderIdUpdated = dataFactory.getObjParameter(configurationManager.providerDB, "@purchaseOrderIdUpdated", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);

            base._providerDB.ExecuteNonQuery("sp_updatePurchaseOrder", new DbParameter[] {
                purchaseOrderIdUpdated,
                dataFactory.getObjParameter(configurationManager.providerDB,"@purchaseOrderId", DbType.Int32, purchaseOrder!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@supplierId", DbType.Int32, purchaseOrder.supplier!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@userId", DbType.Int32, purchaseOrder.user!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@status", DbType.Int32, purchaseOrder.status),
                dataFactory.getObjParameter(configurationManager.providerDB,"@paymentId", DbType.Int32, purchaseOrder.payment!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@currencyId", DbType.Int32, purchaseOrder.currency!.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@subtotal", DbType.Decimal, purchaseOrder.subtotal!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@taxRate", DbType.Decimal, purchaseOrder.taxRate!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@taxAmount", DbType.Decimal, purchaseOrder.taxAmount!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@totalAmount", DbType.Decimal, purchaseOrder.totalAmount!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@modificationDate", DbType.DateTime, purchaseOrder.modificationDate!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@userMainContactPhone", DbType.String, purchaseOrder.user.employee!.mainContactPhone!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@supplierMainContactName", DbType.String, purchaseOrder.supplier!.mainContactName!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@supplierMainContactPhone", DbType.String, purchaseOrder.supplier!.mainContactPhone!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@generalNotes", DbType.String, purchaseOrder.generalNotes!)
            });

            return Convert.ToInt32(purchaseOrderIdUpdated.Value);
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

    public int addPurchaseOrderItem(purchaseOrderItemsModel purchaseOrderItem, int purchaseOrderId)
    {
        try
        {
            var purchaseOrderItemIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@purchaseOrderItemIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);

            base._providerDB.ExecuteNonQuery("sp_addPurchaseOrderItem", new DbParameter[] {
                purchaseOrderItemIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@purchaseOrderId", DbType.Int32, purchaseOrderId),
                dataFactory.getObjParameter(configurationManager.providerDB,"@quantity", DbType.Int32, purchaseOrderItem.quantity),
                dataFactory.getObjParameter(configurationManager.providerDB,"@unit", DbType.Int32, purchaseOrderItem.unit),
                dataFactory.getObjParameter(configurationManager.providerDB,"@unitValue", DbType.Decimal, purchaseOrderItem.unitValue),
                dataFactory.getObjParameter(configurationManager.providerDB,"@totalValue", DbType.Decimal, purchaseOrderItem.totalValue),
                dataFactory.getObjParameter(configurationManager.providerDB,"@description", DbType.String, purchaseOrderItem.description!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@material", DbType.String, purchaseOrderItem.material!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@details", DbType.String, purchaseOrderItem.details!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@imagePath", DbType.String, purchaseOrderItem.imagePath!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@notes", DbType.String, purchaseOrderItem.notes!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@inventoryItemId", DbType.Int32, purchaseOrderItem.inventoryItemId)
            });

            return Convert.ToInt32(purchaseOrderItemIdAdded.Value);
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

    public int updatePurchaseOrderItem(purchaseOrderItemsModel purchaseOrderItem)
    {
        try
        {
            var purchaseOrderItemIdUpdated = dataFactory.getObjParameter(configurationManager.providerDB, "@purchaseOrderItemIdUpdated", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);

            base._providerDB.ExecuteNonQuery("sp_updatePurchaseOrderItem", new DbParameter[] {
                purchaseOrderItemIdUpdated,
                dataFactory.getObjParameter(configurationManager.providerDB,"@purchaseOrderItemId", DbType.Int32, purchaseOrderItem.id),
                dataFactory.getObjParameter(configurationManager.providerDB,"@quantity", DbType.Int32, purchaseOrderItem.quantity),
                dataFactory.getObjParameter(configurationManager.providerDB,"@unit", DbType.Int32, purchaseOrderItem.unit),
                dataFactory.getObjParameter(configurationManager.providerDB,"@unitValue", DbType.Decimal, purchaseOrderItem.unitValue),
                dataFactory.getObjParameter(configurationManager.providerDB,"@totalValue", DbType.Decimal, purchaseOrderItem.totalValue),
                dataFactory.getObjParameter(configurationManager.providerDB,"@description", DbType.String, purchaseOrderItem.description!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@material", DbType.String, purchaseOrderItem.material!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@details", DbType.String, purchaseOrderItem.details!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@imagePath", DbType.String, purchaseOrderItem.imagePath!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@notes", DbType.String, purchaseOrderItem.notes!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@inventoryItemId", DbType.Int32, purchaseOrderItem.inventoryItemId)
            });

            return Convert.ToInt32(purchaseOrderItemIdUpdated.Value);
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

    public bool deletePurchaseOrderById(int id)
    {
        try
        {
            base._providerDB.ExecuteNonQuery("sp_deletePurchaseOrderById", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@purchaseOrderId", DbType.Int32, id),
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

    public bool updateStatusByPurchaseOrderId(int purchaseOrderId, int status)
    {
        try
        {
            var idPurchaseOrderUpdated = dataFactory.getObjParameter(configurationManager.providerDB, "@idPurchaseOrderUpdated", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_updateStatusByPurchaseOrderId", new DbParameter[] {
                idPurchaseOrderUpdated,
                dataFactory.getObjParameter(configurationManager.providerDB,"@purchaseOrderId", DbType.Int32, purchaseOrderId),
                dataFactory.getObjParameter(configurationManager.providerDB,"@newStatus", DbType.Int32, status)
            });

            return Convert.ToInt32(idPurchaseOrderUpdated.Value) > 0 ? true : false;
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