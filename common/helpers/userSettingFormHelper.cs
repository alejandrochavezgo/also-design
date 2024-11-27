namespace common.helpers;

using entities.models;

public class userSettingFormHelper
{
    public bool isUpdateFormValid(userModel user)
    {
        try
        {
            if (string.IsNullOrEmpty(user.email) || string.IsNullOrEmpty(user.firstname) ||
                string.IsNullOrEmpty(user.lastname) || user.status <= 0 || user.id <= 0 || 
                user.employee!.id <= 0 || user.userRole <= 0 || user.userAccess!.Count == 0)
                return false;

            if (!string.IsNullOrEmpty(user.password))
                if (string.IsNullOrEmpty(user.newPassword) || string.IsNullOrEmpty(user.confirmNewPassword) || user.newPassword != user.confirmNewPassword ||
                    string.IsNullOrEmpty(user.newPasswordHash))
                    return false;

            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
}