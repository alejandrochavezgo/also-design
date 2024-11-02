namespace app.helpers;

using entities.enums;
using entities.models;

public static class supplierFormHelper
{
    public static bool isAddFormValid(supplierModel supplier)
    {
        try
        {
            if (string.IsNullOrEmpty(supplier.businessName) || string.IsNullOrEmpty(supplier.rfc) ||
                string.IsNullOrEmpty(supplier.address) || string.IsNullOrEmpty(supplier.zipCode) ||
                string.IsNullOrEmpty(supplier.city) || string.IsNullOrEmpty(supplier.state) ||
                string.IsNullOrEmpty(supplier.country) || supplier.status <= 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
    
    public static bool isUpdateFormValid(supplierModel supplier)
    {
        try
        {
            if (string.IsNullOrEmpty(supplier.businessName) || string.IsNullOrEmpty(supplier.rfc) ||
                string.IsNullOrEmpty(supplier.address) || string.IsNullOrEmpty(supplier.zipCode) ||
                string.IsNullOrEmpty(supplier.city) || string.IsNullOrEmpty(supplier.state) ||
                string.IsNullOrEmpty(supplier.country) || supplier.status <= 0 || supplier.id <= 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public static bool isUpdateFormValid(supplierModel supplier, bool isStatusChange)
    {
        try
        {
            if (supplier == null || supplier.id <= 0 || supplier.status != (int)statusType.ACTIVE)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
}