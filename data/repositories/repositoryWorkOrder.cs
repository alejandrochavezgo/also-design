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

public class repositoryWorkOrder : baseRepository
{
    private log _logger;

    public repositoryWorkOrder()
    {
        _logger = new log();
    }

    public int addWorkOrder(workOrderModel workOrder)
    {
        try
        {
            var workOrderIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@workOrderIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_addWorkOrder", new DbParameter[] {
                workOrderIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@quotationId", DbType.Int32, workOrder.quotationId!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@priorityId", DbType.Int32, workOrder.priorityId!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@rfq", DbType.String, workOrder.rfq!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@endDate", DbType.DateTime, workOrder.deliveryDate!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@creationDate", DbType.DateTime, workOrder.creationDate!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@status", DbType.Int32, workOrder.status!)
            });
            return Convert.ToInt32(workOrderIdAdded.Value);
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

    public int addWorkOrderItem(workOrderItemModel workOrderItem, int workOrderIdAdded)
    {
        try
        {
            var workOrderItemIdAdded = dataFactory.getObjParameter(configurationManager.providerDB, "@workOrderItemIdAdded", DbType.Int32, DBNull.Value, -1, ParameterDirection.Output);
            base._providerDB.ExecuteNonQuery("sp_addWorkOrderItem", new DbParameter[] {
                workOrderItemIdAdded,
                dataFactory.getObjParameter(configurationManager.providerDB,"@workOrderId", DbType.Int32, workOrderIdAdded),
                dataFactory.getObjParameter(configurationManager.providerDB,"@toolNumber", DbType.String, workOrderItem.toolNumber!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@inventoryId", DbType.Int32, workOrderItem.inventoryItemId!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@quantity", DbType.Decimal, workOrderItem.quantity!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@routes", DbType.String, workOrderItem.routes!),
                dataFactory.getObjParameter(configurationManager.providerDB,"@comments", DbType.String, workOrderItem.comments!)
            });
            return Convert.ToInt32(workOrderItemIdAdded.Value);
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

    public workOrderModel getWorkOrderById(int id)
    {
        try
        {
            return factoryGetWorkOrderById.get((DbDataReader)_providerDB.GetDataReader("sp_getWorkOrderById", new DbParameter[] {
                dataFactory.getObjParameter(configurationManager.providerDB,"@clientId", DbType.Int32, id)
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

    public List<catalogModel> getPriorityTypesCatalog()
    {
        try
        {
            return factoryGetCatalog.getList((DbDataReader)_providerDB.GetDataReader("sp_getPriorityTypesCatalog", new DbParameter[] {}));
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