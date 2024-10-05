namespace app.helpers;

using entities.models;

public static class purchaseOrderFormHelper
{
    public static bool isAddFormValid(purchaseOrderModel purchaseOrder)
    {
        try
        {
            if (purchaseOrder.supplier!.id <= 0 || string.IsNullOrEmpty(purchaseOrder.supplier!.mainContactName) || string.IsNullOrEmpty(purchaseOrder.supplier!.mainContactPhone) ||
                purchaseOrder.payment!.id <= 0 || purchaseOrder.user!.id <= 0 || string.IsNullOrEmpty(purchaseOrder.user.employee!.mainContactPhone) || purchaseOrder.currency!.id <= 0 || purchaseOrder.items!.Count == 0)
                return false;

            foreach (var item in purchaseOrder.items!)
                if(string.IsNullOrEmpty(item.description) || string.IsNullOrEmpty(item.material) || string.IsNullOrEmpty(item.unit))
                    return false;

            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
    
    public static bool isUpdateFormValid(purchaseOrderModel purchaseOrder)
    {
        try
        {
            if (purchaseOrder.supplier!.id <= 0 || string.IsNullOrEmpty(purchaseOrder.supplier!.mainContactName) || string.IsNullOrEmpty(purchaseOrder.supplier!.mainContactPhone) ||
                purchaseOrder.payment!.id <= 0 || purchaseOrder.user!.id <= 0 || string.IsNullOrEmpty(purchaseOrder.user.employee!.mainContactPhone) || purchaseOrder.currency!.id <= 0 || purchaseOrder.items!.Count == 0 || purchaseOrder.id <= 0)
                return false;

            foreach (var item in purchaseOrder.items!)
                if(string.IsNullOrEmpty(item.description) || string.IsNullOrEmpty(item.material) || string.IsNullOrEmpty(item.unit))
                    return false;

            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
}