namespace common.helpers;

using common.logging;
using entities.models;
using Newtonsoft.Json;

public class supplierFormHelper
{
    private log _logger;

    public supplierFormHelper ()
    {
        _logger = new log();
    }

    public bool isAddFormValid(supplierModel supplier)
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
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool isUpdateFormValid(supplierModel supplier)
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
}