namespace app.helpers;

using entities.enums;
using entities.models;

public static class quotationFormHelper
{
    public static bool isAddFormValid(quotationModel quotation)
    {
        try
        {
            if (quotation.client!.id <= 0 || string.IsNullOrEmpty(quotation.client!.mainContactName) || string.IsNullOrEmpty(quotation.client!.mainContactPhone) ||
                quotation.payment!.id <= 0 || quotation.user!.id <= 0 || string.IsNullOrEmpty(quotation.user.employee!.mainContactPhone) || quotation.currency!.id <= 0 || quotation.items!.Count == 0)
                return false;

            foreach (var item in quotation.items!)
                if(string.IsNullOrEmpty(item.description) || string.IsNullOrEmpty(item.material) || item.unit <= 0)
                    return false;

            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
    
    public static bool isUpdateFormValid(quotationModel quotation)
    {
        try
        {
            if (quotation.status != (int)statusType.ACTIVE || quotation.client!.id <= 0 || string.IsNullOrEmpty(quotation.client!.mainContactName) || string.IsNullOrEmpty(quotation.client!.mainContactPhone) ||
                quotation.payment!.id <= 0 || quotation.user!.id <= 0 || string.IsNullOrEmpty(quotation.user.employee!.mainContactPhone) || quotation.currency!.id <= 0 || quotation.items!.Count == 0 || quotation.id <= 0)
                return false;

            foreach (var item in quotation.items!)
                if(string.IsNullOrEmpty(item.description) || string.IsNullOrEmpty(item.material) || item.unit <= 0)
                    return false;

            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public static bool isUpdateFormValid(changeStatusModel changeStatus)
    {
        try
        {
            if (changeStatus == null || changeStatus.quotationId <= 0 || changeStatus.currentStatusId <= 0 || changeStatus.newStatusId <= 0 || string.IsNullOrEmpty(changeStatus.comments))
                return false;

            if (changeStatus.newStatusId == changeStatus.currentStatusId || string.IsNullOrEmpty(changeStatus.comments))
                return false;

            switch ((statusType)changeStatus.currentStatusId)
            {
                case statusType.ACTIVE:
                    if (changeStatus.newStatusId != (int)statusType.PENDING)
                        return false;
                    break;
                case statusType.PENDING:
                    if (changeStatus.newStatusId != (int)statusType.APPROVED && changeStatus.newStatusId != (int)statusType.REJECTED && changeStatus.newStatusId != (int)statusType.EXPIRED && changeStatus.newStatusId != (int)statusType.CANCELLED)
                        return false;
                    break;
                case statusType.APPROVED:
                    if (changeStatus.newStatusId != (int)statusType.LINKED && changeStatus.newStatusId != (int)statusType.CANCELLED && changeStatus.newStatusId != (int)statusType.CLOSED)
                        return false;
                    break;
                case statusType.REJECTED:
                case statusType.EXPIRED:
                case statusType.CANCELLED:
                    if (changeStatus.newStatusId != (int)statusType.CLOSED)
                        return false;
                    break;
                case statusType.CLOSED:
                case statusType.LINKED:
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
}