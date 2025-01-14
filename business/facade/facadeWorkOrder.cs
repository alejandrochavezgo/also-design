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

    public List<List<catalogModel>> getAllWorkOrderCatalogs()
    {
        try
        {
            var catalogs = new List<List<catalogModel>>();
            catalogs.Add(_repositoryWorkOrder.getPriorityTypesCatalog());
            return catalogs;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}