namespace app.helpers;

using entities.models;

public static class workOrderFormHelper
{
    public static bool isAddFormValid(workOrderModel workOrder)
    {
        try
        {
            if (workOrder.quotationId <= 0 || workOrder.priorityId <= 0 || workOrder.deliveryDate!.Value.Date < dateHelper.pstNow().Date ||
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
    
    // public static bool isUpdateFormValid(clientModel client)
    // {
    //     try
    //     {
    //         if (string.IsNullOrEmpty(client.businessName) || string.IsNullOrEmpty(client.rfc) ||
    //             string.IsNullOrEmpty(client.address) || string.IsNullOrEmpty(client.zipCode) ||
    //             string.IsNullOrEmpty(client.city) || string.IsNullOrEmpty(client.state) ||
    //             string.IsNullOrEmpty(client.country) || client.status <= 0 || client.id <= 0)
    //             return false;
    //         return true;
    //     }
    //     catch (Exception exception)
    //     {
    //         throw exception;
    //     }
    // }

    // public static bool isUpdateFormValid(clientModel client, bool isStatusChange)
    // {
    //     try
    //     {
    //         if (client == null || client.id <= 0 || client.status != (int)statusType.ACTIVE)
    //             return false;
    //         return true;
    //     }
    //     catch (Exception exception)
    //     {
    //         throw exception;
    //     }
    // }
}