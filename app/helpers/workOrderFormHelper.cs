namespace app.helpers;

using entities.enums;
using entities.models;

public static class workOrderFormHelper
{
    public static bool isAddFormValid(workOrderModel workOrder)
    {
        try
        {
            if (workOrder.userId <= 0 || workOrder.quotationId <= 0 || workOrder.priorityId <= 0 || workOrder.deliveryDate!.Date < dateHelper.pstNow().Date ||
                workOrder.items!.Count == 0)
                return false;

            foreach (var item in workOrder.items)
                if (string.IsNullOrEmpty(item.toolNumber) || item.inventoryItemId <= 0 || item.quantity <= 0 || item.quantityInStock <= 0 ||
                    item.quantity > item.quantityInStock || item.routes!.Count == 0)
                    return false;

            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
    
    public static bool isUpdateFormValid(workOrderModel workOrder)
    {
        try
        {
            if (workOrder.id <= 0 || workOrder.userId <= 0 || workOrder.quotationId <= 0 || workOrder.priorityId <= 0 || workOrder.deliveryDate!.Date < dateHelper.pstNow().Date ||
                workOrder.items!.Count == 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public static bool isUpdateFormValid(workOrderModel workOrder, bool isStatusChange)
    {
        try
        {
            if (workOrder == null || workOrder.id <= 0 || workOrder.status != (int)statusType.CANCELLED)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
}