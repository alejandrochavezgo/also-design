namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using entities.enums;
using Newtonsoft.Json;
using System.Transactions;
using common.helpers;
using common.utils;

public class facadePurchaseOrder
{
    private log _logger;
    private userModel _user;
    private facadeTrace _facadeTrace;
    private repositoryPurchaseOrder _repositoryPurchaseOrder;

    public facadePurchaseOrder(userModel user)
    {
        _user = user;
        _logger = new log();
        _facadeTrace = new facadeTrace();
        _repositoryPurchaseOrder = new repositoryPurchaseOrder();
    }

    public List<List<catalogModel>> getAllPurchaseOrderCatalogs()
    {
        try
        {
            var catalogs = new List<List<catalogModel>>();
            catalogs.Add(_repositoryPurchaseOrder.getPaymentTypesCatalog());
            catalogs.Add(_repositoryPurchaseOrder.getCurrencyTypesCatalog());
            return catalogs;
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
            return _repositoryPurchaseOrder.getPurchaseOrders();
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool addPurchaseOrder(purchaseOrderModel purchaseOrder)
    {
        using(var transactionScope = new TransactionScope())
        {
            try
            {
                purchaseOrder.creationDate = DateTime.Now;
                var purchaseOrderIdAdded = _repositoryPurchaseOrder.addPurchaseOrder(purchaseOrder);

                foreach (var item in purchaseOrder.items)
                {
                    item.imagePath = string.IsNullOrEmpty(item.imagePath) ? string.Empty : item.imagePath;
                    if(!(_repositoryPurchaseOrder.addPurchaseOrderItem(item, purchaseOrderIdAdded) > 0))
                        throw new Exception($"Error at saving the purchase order item.");
                }

                var result = purchaseOrderIdAdded > 0;
                var purchaseOrderAfter = getPurchaseOrderById(purchaseOrderIdAdded);
                var purchaseOrderSettings = new JsonSerializerSettings
                {
                    ContractResolver = new ignoringPropertiesContractResolver(new[]
                    { 
                        "creationDate", "modificationDate", "status", "statusColor"
                    })
                };
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.ADD_PURCHASE_ORDER,
                    entityType = entityType.PURCHASE_ORDER,
                    userId = _user.id,
                    comments = "PURCHASE ORDER ADDED.",
                    beforeChange = string.Empty,
                    afterChange = JsonConvert.SerializeObject(purchaseOrderAfter, purchaseOrderSettings),
                    entityId = purchaseOrderIdAdded
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

    public bool updatePurchaseOrder(purchaseOrderModel purchaseOrder)
    {
        using(var transactionScope = new TransactionScope())
        {
            try
            {
                var purchaseOrderBefore = getPurchaseOrderById(purchaseOrder.id);
                if (purchaseOrderBefore != null && purchaseOrderBefore.status != (int)statusType.ACTIVE)
                {
                    transactionScope.Dispose();
                    return false;
                }

                purchaseOrder.modificationDate = DateTime.Now;
                var purchaseOrderIdUpdated = _repositoryPurchaseOrder.updatePurchaseOrder(purchaseOrder);

                foreach (var item in purchaseOrder.items)
                    if(item.id > 0)
                    {
                        item.imagePath = string.IsNullOrEmpty(item.imagePath) ? string.Empty : item.imagePath;
                        if(!(_repositoryPurchaseOrder.updatePurchaseOrderItem(item) > 0))
                            throw new Exception($"Error at updating the purchase order item.");
                    }
                    else
                    {
                        item.imagePath = string.IsNullOrEmpty(item.imagePath) ? string.Empty : item.imagePath;
                        if(!(_repositoryPurchaseOrder.addPurchaseOrderItem(item, purchaseOrderIdUpdated) > 0))
                            throw new Exception($"Error at saving the purchase order item.");
                    }

                var result = purchaseOrderIdUpdated > 0;
                var purchaseOrderAfter = getPurchaseOrderById(purchaseOrder.id);
                var purchaseOrderSettings = new JsonSerializerSettings
                {
                    ContractResolver = new ignoringPropertiesContractResolver(new[]
                    { 
                        "creationDate", "modificationDate", "status", "statusColor"
                    })
                };
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.UPDATE_PURCHASE_ORDER,
                    entityType = entityType.PURCHASE_ORDER,
                    userId = _user.id,
                    comments = "PURCHASE ORDER UPDATED.",
                    beforeChange = JsonConvert.SerializeObject(purchaseOrderBefore, purchaseOrderSettings),
                    afterChange = JsonConvert.SerializeObject(purchaseOrderAfter, purchaseOrderSettings),
                    entityId = purchaseOrder.id
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

    public bool deletePurchaseOrderById(int id)
    {
        using (var transactionScope = new TransactionScope())
        {
            try
            {
                var purchaseOrder = _repositoryPurchaseOrder.getPurchaseOrderById(id);
                if (purchaseOrder == null || purchaseOrder.status != (int)statusType.ACTIVE)
                    return false;

                var result = _repositoryPurchaseOrder.deletePurchaseOrderById(id);
                var trace = _facadeTrace.addTrace(new traceModel
                {
                    traceType = traceType.DELETE_PURCHASE_ORDER,
                    entityType = entityType.PURCHASE_ORDER,
                    userId = _user.id,
                    comments = "PURCHASE ORDER DELETED.",
                    beforeChange = string.Empty,
                    afterChange = string.Empty,
                    entityId = purchaseOrder.id
                });

                if(result && trace > 0)
                    transactionScope.Complete();
                else
                    transactionScope.Dispose();

                return result;
            }
            catch (Exception exception)
            {
                _logger.logError($"{JsonConvert.SerializeObject(exception)}");
                throw exception;
            }
        }
    }

    public bool updateStatusByPurchaseOrderId(changeStatusModel changeStatus)
    {
        using (TransactionScope transactionScope = new TransactionScope())
        {
            try
            {
                var purchaseOrder = _repositoryPurchaseOrder.getPurchaseOrderById(changeStatus.purchaseOrderId);
                if (purchaseOrder == null || (purchaseOrder.status == changeStatus.newStatusId && purchaseOrder.status != (int)statusType.PARTIALLY_FULFILLED))
                {
                    transactionScope.Dispose();
                    return false;
                }

                var trace = 0;
                var today = DateTime.Now;
                var quantityPendingItems = 0;
                var quantityCompletedItems = 0;
                var _facadeTrace = new facadeTrace();
                var _facadeInventory = new facadeInventory();
                if (changeStatus.newStatusId == (int)statusType.PARTIALLY_FULFILLED)
                {
                    var purchaseOrderItems = getPendingPurchaseOrderItemsByPurchaseOrderId(purchaseOrder.id);
                    if (purchaseOrderItems == null || purchaseOrderItems.Count == 0)
                    {
                        transactionScope.Dispose();
                        return false;
                    }

                    quantityPendingItems = purchaseOrderItems.Count;
                    foreach (var purchaseOrderItem in changeStatus.purchaseOrderItems)
                    {
                        var purchaseOrderItemFromDatabase = purchaseOrderItems.FirstOrDefault(x => x.inventoryItemId == purchaseOrderItem.inventoryItemId);
                        var inventoryMovements = _facadeInventory.getInventoryMovementsByPurchaseOrderIdAndInventoryId(purchaseOrder.id, purchaseOrderItem.inventoryItemId);
                        var newQuantity = 0d;
                        var maxQuantityAllowed = 0d;
                        if (purchaseOrderItemFromDatabase != null && inventoryMovements != null)
                        {
                            newQuantity = (inventoryMovements.Sum(x => x.quantity) + purchaseOrderItem.quantity);
                            maxQuantityAllowed = purchaseOrderItemFromDatabase.quantity;
                            if (newQuantity > maxQuantityAllowed)
                            {
                                transactionScope.Dispose();
                                return false;
                            }
                        }

                        var entry = _facadeInventory.addEntry(purchaseOrderItem.inventoryItemId, purchaseOrderItem.quantity, today);
                        var movement = _facadeInventory.addMovement(purchaseOrderItem.inventoryItemId, inventoryMovementType.PARTIAL_RECEIPT, purchaseOrder.id, changeStatus.userId, purchaseOrderItem.quantity, purchaseOrderItem.unit, changeStatus.comments, purchaseOrderItem.unitValue, purchaseOrderItem.totalValue, today);
                        trace = _facadeTrace.addTrace(new traceModel
                        {
                            entityType = entityType.INVENTORY,
                            entityId = purchaseOrderItem.inventoryItemId,
                            traceType = traceType.PARTIAL_MATERIAL_RECEIPT,
                            userId = changeStatus.userId,
                            comments = changeStatus.comments,
                            beforeChange = string.Empty,
                            afterChange = string.Empty
                        });

                        if (newQuantity > 0 && maxQuantityAllowed > 0 && newQuantity == maxQuantityAllowed)
                        {
                            quantityCompletedItems++;
                        }

                        if (entry <= 0|| movement <= 0 || trace <= 0)
                        {
                            transactionScope.Dispose();
                            return false;
                        }
                    }
                }
                else if (changeStatus.newStatusId == (int)statusType.FULFILLED)
                {
                    var purchaseOrderItems = changeStatus.currentStatusId == (int)statusType.PARTIALLY_FULFILLED ? getPendingPurchaseOrderItemsByPurchaseOrderId(purchaseOrder.id) : _repositoryPurchaseOrder.getPurchaseOrderItemsByPurchaseOrderId(purchaseOrder.id);
                    if (purchaseOrderItems == null || purchaseOrderItems.Count == 0)
                    {
                        transactionScope.Dispose();
                        return false;
                    }

                    foreach (var purchaseOrderItem in purchaseOrderItems)
                    {
                        var quantity = 0d;
                        var inventoryMovements = _facadeInventory.getInventoryMovementsByPurchaseOrderIdAndInventoryId(purchaseOrder.id, purchaseOrderItem.inventoryItemId);
                        if (inventoryMovements != null)
                        {
                            quantity = purchaseOrderItem.quantity - inventoryMovements.Sum(x => x.quantity);
                        }

                        var entry = _facadeInventory.addEntry(purchaseOrderItem.inventoryItemId, quantity, today);
                        var movement = _facadeInventory.addMovement(purchaseOrderItem.inventoryItemId, inventoryMovementType.RECEIPT, purchaseOrder.id, changeStatus.userId, quantity, purchaseOrderItem.unit, changeStatus.comments, purchaseOrderItem.unitValue, purchaseOrderItem.totalValue, today);
                        trace = _facadeTrace.addTrace(new traceModel
                        {
                            entityType = entityType.INVENTORY,
                            entityId = purchaseOrderItem.inventoryItemId,
                            traceType = traceType.MATERIAL_RECEIPT,
                            userId = changeStatus.userId,
                            comments = changeStatus.comments,
                            beforeChange = string.Empty,
                            afterChange = string.Empty
                        });

                        if (entry <= 0|| movement <= 0 || trace <= 0)
                        {
                            transactionScope.Dispose();
                            return false;
                        }
                    }
                }

                trace = _facadeTrace.addTrace(new traceModel
                {
                    entityType = entityType.PURCHASE_ORDER,
                    entityId = purchaseOrder.id,
                    traceType = traceType.CHANGE_STATUS,
                    userId = changeStatus.userId,
                    comments = changeStatus.comments,
                    beforeChange = JsonConvert.SerializeObject(purchaseOrder.status),
                    afterChange = JsonConvert.SerializeObject(changeStatus.newStatusId)
                });

                if(changeStatus.newStatusId == (int)statusType.PARTIALLY_FULFILLED && quantityCompletedItems > 0 && quantityPendingItems > 0 && quantityCompletedItems == quantityPendingItems)
                {
                    trace = _facadeTrace.addTrace(new traceModel
                    {
                        entityType = entityType.PURCHASE_ORDER,
                        entityId = purchaseOrder.id,
                        traceType = traceType.CHANGE_STATUS,
                        userId = changeStatus.userId,
                        comments = "AUTO-FULFILLED",
                        beforeChange = $"{changeStatus.newStatusId}",
                        afterChange = $"{(int)statusType.FULFILLED}"
                    });
                    changeStatus.newStatusId = (int)statusType.FULFILLED;
                }

                if (_repositoryPurchaseOrder.updateStatusByPurchaseOrderId(purchaseOrder.id, changeStatus.newStatusId) && trace > 0)
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

    public List<List<catalogModel>> getStatusCatalog()
    {
        try
        {
            var catalogs = new List<List<catalogModel>>();
            var purchaseOrderStatus = new HashSet<int> {1, 6, 7, 9, 10, 8, 12};
            catalogs.Add(new repositoryInventory().getStatusTypesCatalog().Where(x => purchaseOrderStatus.Contains(x.id)).ToList());
            return catalogs;
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
            var purchaseOrder = _repositoryPurchaseOrder.getPurchaseOrderById(id);
            purchaseOrder.items = _repositoryPurchaseOrder.getPurchaseOrderItemsByPurchaseOrderId(id);
            var _repositorySupplier = new repositorySupplier();
            purchaseOrder.supplier.contactNames = _repositorySupplier.getContactNamesBySupplierId(purchaseOrder.supplier.id);
            purchaseOrder.supplier.contactPhones = _repositorySupplier.getContactPhonesBySupplierId(purchaseOrder.supplier.id);
            return purchaseOrder;
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
            return _repositoryPurchaseOrder.getPurchaseOrderItemsByPurchaseOrderId(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<purchaseOrderItemsModel> getPendingPurchaseOrderItemsByPurchaseOrderId(int purchaseOrderId)
    {
        try
        {
            var _facadeInventory = new facadeInventory();
            var purchaseOrderItems = _repositoryPurchaseOrder.getPurchaseOrderItemsByPurchaseOrderId(purchaseOrderId);
            var purchaseOrderItemsToRemove = new List<purchaseOrderItemsModel>();
            if (purchaseOrderItems != null)
                foreach (var purchaseOrderItem in purchaseOrderItems)
                {
                    var inventoryMovements = _facadeInventory.getInventoryMovementsByPurchaseOrderIdAndInventoryId(purchaseOrderId, purchaseOrderItem.inventoryItemId);
                    if (inventoryMovements != null && inventoryMovements.Sum(x => x.quantity) == purchaseOrderItem.quantity)
                        purchaseOrderItemsToRemove.Add(purchaseOrderItem);
                }
            purchaseOrderItems.RemoveAll(purchaseOrderItem => purchaseOrderItemsToRemove.Contains(purchaseOrderItem));
            return purchaseOrderItems;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public List<traceModel> getPurchaseOrderTracesByPurchaseOrderId(int id)
    {
        try
        {
            return _repositoryPurchaseOrder.getPurchaseOrderTracesByPurchaseOrderId(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public traceModel getPurchaseOrderTraceById(int id)
    {
        try
        {
            return _repositoryPurchaseOrder.getPurchaseOrderTraceById(id);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}