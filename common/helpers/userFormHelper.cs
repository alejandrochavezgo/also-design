namespace common.helpers;

using common.logging;
using entities.enums;
using entities.models;
using Newtonsoft.Json;

public class userFormHelper
{
    public bool isAddFormValid(userModel user)
    {
        try
        {
            if (string.IsNullOrEmpty(user.email) || string.IsNullOrEmpty(user.firstname) ||
                string.IsNullOrEmpty(user.password) || string.IsNullOrEmpty(user.username) ||
                string.IsNullOrEmpty(user.lastname) || user.status <= 0 || user.userRole <= 0 || user.userAccess!.Count == 0)
                return false;

            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
    
    public bool isUpdateFormValid(userModel user)
    {
        try
        {
            if (string.IsNullOrEmpty(user.email) || string.IsNullOrEmpty(user.firstname) ||
                string.IsNullOrEmpty(user.lastname) || user.status <= 0 || user.id <= 0 || 
                user.employee!.id <= 0 || user.userRole <= 0 || user.userAccess!.Count == 0)
                return false;

            if (!string.IsNullOrEmpty(user.newPassword))
                if (string.IsNullOrEmpty(user.confirmNewPassword) || user.newPassword != user.confirmNewPassword)
                    return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public bool isUpdateFormValid(userModel user, bool isStatusChange)
    {
        try
        {
            if (user == null || user.id <= 0 || user.status != (int)statusType.ACTIVE)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
}
}