namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using entities.enums;
using Newtonsoft.Json;
using System.Transactions;

public class facadePurchaseOrder
{
    private log _logger;
    private repositoryPurchaseOrder _repositoryPurchaseOrder;

    public facadePurchaseOrder()
    {
        _logger = new log();
        _repositoryPurchaseOrder = new repositoryPurchaseOrder();
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

                transactionScope.Complete();
                return true;
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

                transactionScope.Complete();
                return true;
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
        try
        {
            var purchaseOrder = _repositoryPurchaseOrder.getPurchaseOrderById(id);
            if (purchaseOrder == null || purchaseOrder.status != (int)statusType.ACTIVE)
                return false;
            return _repositoryPurchaseOrder.deletePurchaseOrderById(id);
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
            purchaseOrder.items = _repositoryPurchaseOrder.getPurchaseOrderItemsByIdPurchaseOrder(id);

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
}