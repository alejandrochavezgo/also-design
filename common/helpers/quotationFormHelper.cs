namespace common.helpers;

using common.logging;
using entities.models;
using Newtonsoft.Json;

public class quotationFormHelper
{
    private log _logger;

    public quotationFormHelper ()
    {
        _logger = new log();
    }

    public bool isAddFormValid(quotationModel quotation)
    {
        try
        {
            if (quotation.client!.id <= 0 || string.IsNullOrEmpty(quotation.client!.mainContactName) || string.IsNullOrEmpty(quotation.client!.mainContactPhone) ||
                quotation.payment!.id <= 0 || quotation.user!.id <= 0 || string.IsNullOrEmpty(quotation.user.employee!.mainContactPhone) || quotation.currency!.id <= 0 || quotation.items!.Count == 0)
                return false;

            foreach (var item in quotation.items!)
                    if(string.IsNullOrEmpty(item.description) || string.IsNullOrEmpty(item.material) || string.IsNullOrEmpty(item.unit))
                        return false;

            return true;
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool isUpdateFormValid(quotationModel quotation)
    {
        try
        {
            if (quotation.client!.id <= 0 || string.IsNullOrEmpty(quotation.client!.mainContactName) || string.IsNullOrEmpty(quotation.client!.mainContactPhone) ||
                quotation.payment!.id <= 0 || quotation.user!.id <= 0 || string.IsNullOrEmpty(quotation.user.employee!.mainContactPhone) || quotation.currency!.id <= 0 || quotation.items!.Count == 0 || quotation.id <= 0)
                return false;

            foreach (var item in quotation.items!)
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