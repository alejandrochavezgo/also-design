namespace common.helpers;

using common.logging;
using entities.enums;
using entities.models;
using Newtonsoft.Json;

public class purchaseOrderFormHelper
{
    private log _logger;

    public purchaseOrderFormHelper ()
    {
        _logger = new log();
    }

    public bool isAddFormValid(purchaseOrderModel purchaseOrder)
    {
        try
        {
            if (purchaseOrder.projectId <= 0 || purchaseOrder.status != (int)statusType.ACTIVE || purchaseOrder.supplier!.id <= 0 || string.IsNullOrEmpty(purchaseOrder.supplier!.mainContactName) || string.IsNullOrEmpty(purchaseOrder.supplier!.mainContactPhone) ||
                purchaseOrder.payment!.id <= 0 || purchaseOrder.user!.id <= 0 || string.IsNullOrEmpty(purchaseOrder.user.employee!.mainContactPhone) || purchaseOrder.currency!.id <= 0 || purchaseOrder.items!.Count == 0)
                return false;

            foreach (var item in purchaseOrder.items!)
                    if(string.IsNullOrEmpty(item.description) || string.IsNullOrEmpty(item.material) || item.unit <= 0 || item.inventoryItemId <= 0)
                        return false;

            return true;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool isUpdateFormValid(purchaseOrderModel purchaseOrder)
    {
        try
        {
            if (purchaseOrder.projectId <= 0 || purchaseOrder.supplier!.id <= 0 || string.IsNullOrEmpty(purchaseOrder.supplier!.mainContactName) || string.IsNullOrEmpty(purchaseOrder.supplier!.mainContactPhone) ||
                purchaseOrder.payment!.id <= 0 || purchaseOrder.user!.id <= 0 || string.IsNullOrEmpty(purchaseOrder.user.employee!.mainContactPhone) || purchaseOrder.currency!.id <= 0 || purchaseOrder.items!.Count == 0 || purchaseOrder.id <= 0)
                return false;

            foreach (var item in purchaseOrder.items!)
                if(string.IsNullOrEmpty(item.description) || string.IsNullOrEmpty(item.material) || item.unit <= 0 || item.inventoryItemId <= 0)
                    return false;

            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public bool isUpdateFormValid(changeStatusModel changeStatus)
    {
        try
        {
            if (changeStatus == null || changeStatus.purchaseOrderId <= 0 || changeStatus.currentStatusId <= 0 || changeStatus.newStatusId <= 0 || changeStatus.userId <= 0 || string.IsNullOrEmpty(changeStatus.comments))
                return false;

            if (changeStatus.newStatusId == changeStatus.currentStatusId && !(changeStatus.newStatusId == (int)statusType.PARTIALLY_FULFILLED && changeStatus.currentStatusId == (int)statusType.PARTIALLY_FULFILLED) || string.IsNullOrEmpty(changeStatus.comments))
                return false;

            if (changeStatus.newStatusId == (int)statusType.PARTIALLY_FULFILLED && !arePurchaseOrderItemsValid(changeStatus.purchaseOrderItems!))
                return false;

            switch ((statusType)changeStatus.currentStatusId)
            {
                case statusType.ACTIVE:
                    if (changeStatus.newStatusId != (int)statusType.PENDING)
                        return false;
                    break;
                case statusType.PENDING:
                    if (changeStatus.newStatusId != (int)statusType.ACTIVE && changeStatus.newStatusId != (int)statusType.APPROVED && changeStatus.newStatusId != (int)statusType.REJECTED)
                        return false;
                    break;
                case statusType.APPROVED:
                    if (changeStatus.newStatusId != (int)statusType.CANCELLED && changeStatus.newStatusId != (int)statusType.PARTIALLY_FULFILLED && changeStatus.newStatusId != (int)statusType.FULFILLED)
                        return false;
                    break;
                case statusType.PARTIALLY_FULFILLED:
                    if (changeStatus.newStatusId != (int)statusType.FULFILLED && changeStatus.newStatusId != (int)statusType.CANCELLED && changeStatus.newStatusId != (int)statusType.PARTIALLY_FULFILLED)
                        return false;
                    break;
                case statusType.CANCELLED:
                case statusType.FULFILLED:
                case statusType.REJECTED:
                    if (changeStatus.newStatusId != (int)statusType.CLOSED)
                        return false;
                    break;
                case statusType.CLOSED:
                    return false;
                default:
                    return false;
            }

            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    private static bool arePurchaseOrderItemsValid(List<purchaseOrderItemsModel> purchaseOrderItems)
    {
        try
        {
            if (purchaseOrderItems == null || purchaseOrderItems.Count == 0)
                return false;

            return !purchaseOrderItems.Any(item => item.inventoryItemId <= 0 || item.unit <= 0 || item.quantity <= 0 || item.unitValue <= 0 || item.totalValue <= 0);
        }
        catch (Exception exception)
        {
            return false;
        }
    }
}