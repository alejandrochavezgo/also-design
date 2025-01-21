namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;
using entities.enums;
using System.Transactions;
using common.utils;
using common.helpers;

public class facadeWorkOrder
{
    private log _logger;
    private userModel _user;
    private repositoryWorkOrder _repositoryWorkOrder;
    private facadeTrace _facadeTrace;
    private DateTime _dateTime;

    public facadeWorkOrder(userModel user)
    {
        _user = user;
        _logger = new log();
        _facadeTrace = new facadeTrace();
        _repositoryWorkOrder = new repositoryWorkOrder();
        _dateTime = new dateHelper().pstNow();
    }

    public bool addWorkOrder(workOrderModel workOrder)
    {
        using(var transactionScope = new TransactionScope())
        {
            try
            {
                workOrder.creationDate = _dateTime;
                workOrder.status = (int)statusType.ACTIVE;
                var workOrderIdAdded = _repositoryWorkOrder.addWorkOrder(workOrder);
                foreach (var item in workOrder.items)
                    if(!(_repositoryWorkOrder.addWorkOrderItem(item, workOrderIdAdded) > 0))
                        throw new Exception($"Error at saving the work order item.");

                var workOrderAfter = getWorkOrderById(workOrderIdAdded);
                var workOrderSettings = new JsonSerializerSettings
                {
                    ContractResolver = new ignoringPropertiesContractResolver(new[]
                    { 
                        "creationDate", "modificationDate", "status", "statusColor"
                    })
                };
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.ADD_WORKORDER,
                    entityType = entityType.WORK_ORDER,
                    userId = _user.id,
                    comments = "WORK ORDER ADDED.",
                    beforeChange = string.Empty,
                    afterChange = JsonConvert.SerializeObject(workOrderAfter, workOrderSettings),
                    entityId = workOrderIdAdded
                });

                var result = workOrderIdAdded > 0 && trace > 0;
                if(result)
                    transactionScope.Complete();
                else
                    transactionScope.Dispose();

                return result;
            }
            catch (Exception exception)
            {
                transactionScope.Dispose();
                _logger.logError($"{JsonConvert.SerializeObject(exception)}");
                throw exception;
            }
        }
    }

    public workOrderModel getWorkOrderById(int id)
    {
        try
        {
            return _repositoryWorkOrder.getWorkOrderById(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public workOrderModel getFullWorkOrderById(int id)
    {
        try
        {
            var workOrder = _repositoryWorkOrder.getWorkOrderById(id);
            workOrder.items = _repositoryWorkOrder.getWorkOrderItemsByWorkOrderId(workOrder.id);
            workOrder.quotationItems = new repositoryQuotation().getQuotationItemsByIdQuotation(workOrder.quotationId);
            return workOrder;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<List<catalogModel>> getAllWorkOrderCatalogs()
    {
        try
        {
            var catalogs = new List<List<catalogModel>>();
            catalogs.Add(_repositoryWorkOrder.getStatusTypesCatalog().Where(x => x.id == (int)statusType.ACTIVE ||
                                                                            x.id == (int)statusType.INACTIVE ||
                                                                            x.id == (int)statusType.LOCKED ||
                                                                            x.id == (int)statusType.CANCELLED ||
                                                                            x.id == (int)statusType.INPROGRESS ||
                                                                            x.id == (int)statusType.CLOSED).ToList());
            catalogs.Add(_repositoryWorkOrder.getPriorityTypesCatalog());
            return catalogs;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<workOrderModel> getWorkOrders()
    {
        try
        {
            return _repositoryWorkOrder.getWorkOrders();
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool deleteWorkOrderById(int id)
    {
        using (var transactionScope = new TransactionScope())
        {
            try
            {
                var workOrder = _repositoryWorkOrder.getWorkOrderById(id);
                if (workOrder == null || workOrder.status != (int)statusType.CANCELLED)
                    return false;

                var result = _repositoryWorkOrder.deleteWorkOrderById(id);
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.DELETE_WORKORDER,
                    entityType = entityType.WORK_ORDER,
                    userId = _user.id,
                    comments = "WORKORDER DELETED.",
                    beforeChange = string.Empty,
                    afterChange = string.Empty,
                    entityId = workOrder.id
                });

                if(result && trace > 0)
                    transactionScope.Complete();
                else
                    transactionScope.Dispose();

                return result;
            }
            catch (Exception exception)
            {
                transactionScope.Dispose();
                _logger.logError($"{JsonConvert.SerializeObject(exception)}");
                throw exception;
            }
        }
    }

    public bool updateWorkOrder(workOrderModel workOrder)
    {
        using(var transactionScope = new TransactionScope())
        {
            try
            {
                var workOrderBefore = getFullWorkOrderById(workOrder.id);
                if (workOrderBefore != null && workOrderBefore.status != (int)statusType.ACTIVE)
                {
                    transactionScope.Dispose();
                    return false;
                }

                workOrder.modificationDate = _dateTime;
                var workOrderIdUpdated = _repositoryWorkOrder.updateWorkOrder(workOrder);

                foreach (var item in workOrder.items)
                    if (item.id > 0)
                    {
                        if(!(_repositoryWorkOrder.updateWorkOrderItem(item) > 0))
                            throw new Exception($"Error at updating the work order item.");
                    }
                    else
                    {
                        if(!(_repositoryWorkOrder.addWorkOrderItem(item, workOrderIdUpdated) > 0))
                            throw new Exception($"Error at saving the new work order item.");
                    }

                var result = workOrderIdUpdated > 0;
                var workOrderAfter = getFullWorkOrderById(workOrder.id);
                var workOrderSettings = new JsonSerializerSettings
                {
                    ContractResolver = new ignoringPropertiesContractResolver(new[]
                    { 
                        "creationDate", "modificationDate", "status", "statusColor"
                    })
                };
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.UPDATE_WORKORDER,
                    entityType = entityType.WORK_ORDER,
                    userId = _user.id,
                    comments = "WORK ORDER UPDATED.",
                    beforeChange = JsonConvert.SerializeObject(workOrderBefore, workOrderSettings),
                    afterChange = JsonConvert.SerializeObject(workOrderAfter, workOrderSettings),
                    entityId = workOrder.id
                });

                if(result && trace > 0)
                    transactionScope.Complete();
                else
                    transactionScope.Dispose();

                return result;
            }
            catch (Exception exception)
            {
                transactionScope.Dispose();
                _logger.logError($"{JsonConvert.SerializeObject(exception)}");
                throw exception;
            }
        }
    }

    public List<traceModel> getWorkOrderTracesByWorkOrderId(int id)
    {
        try
        {
            return _repositoryWorkOrder.getWorkOrderTracesByWorkOrderId(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public traceModel getWorkOrderTraceById(int id)
    {
        try
        {
            return _repositoryWorkOrder.getWorkOrderTraceById(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}