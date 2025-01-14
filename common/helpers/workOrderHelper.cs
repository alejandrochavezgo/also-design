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
            if (workOrder.quotationId <= 0 || workOrder.priorityId <= 0 || workOrder.deliveryDate!.Value.Date < _dateTime.Date ||
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

    // public bool isUpdateFormValid(projectModel project)
    // {
    //     try
    //     {
    //         if (string.IsNullOrEmpty(project.name) || project.client == null || project.client.id <= 0 ||
    //             string.IsNullOrEmpty(project.description) || project.startDate == null || project.startDate.Value.Date < _dateTime.Date ||
    //             project.endDate == null || project.endDate.Value.Date < _dateTime.Date ||
    //             project.endDate.Value.Date < project.startDate.Value.Date || project.status <= 0 || project.id <= 0)
    //             return false;
    //         return true;
    //     }
    //     catch (Exception exception)
    //     {
    //         throw exception;
    //     }
    // }

    // public bool isUpdateFormValid(projectModel project, bool isStatusChange)
    // {
    //     try
    //     {
    //         if (project == null || project.id <= 0 || project.status != (int)statusType.ACTIVE)
    //             return false;
    //         return true;
    //     }
    //     catch (Exception exception)
    //     {
    //         throw exception;
    //     }
    // }
}