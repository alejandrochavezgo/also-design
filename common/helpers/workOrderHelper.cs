namespace common.helpers;

using common.logging;
using entities.enums;
using entities.models;
using Newtonsoft.Json;

public class workOrderFormHelper
{
    private log _logger;
    private DateTime _dateTime;

    public workOrderFormHelper ()
    {
        _logger = new log();
        _dateTime = new dateHelper().pstNow();
    }

    public bool isAddFormValid(workOrderModel workOrder)
    {
        try
        {
            if (workOrder.userId <= 0 || workOrder.quotationId <= 0 || workOrder.priorityId <= 0 || workOrder.deliveryDate!.Date < _dateTime.Date ||
                workOrder.items!.Count == 0)
                return false;

            foreach (var item in workOrder.items)
            {
                if (string.IsNullOrEmpty(item.toolNumber) || item.inventoryItemId <= 0 || item.quantity <= 0 || item.quantityInStock <= 0 ||
                    item.quantity > item.quantityInStock || item.routes!.Count == 0)
                    return false;
            }

            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public bool isUpdateFormValid(workOrderModel workOrder)
    {
        try
        {
            if (workOrder.id <= 0 || workOrder.userId <= 0 || workOrder.quotationId <= 0 || workOrder.priorityId <= 0 || workOrder.deliveryDate!.Date < _dateTime.Date ||
                workOrder.items!.Count == 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public bool isUpdateFormValid(workOrderModel workOrder, bool isStatusChange)
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