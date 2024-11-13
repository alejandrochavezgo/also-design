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
                string.IsNullOrEmpty(user.lastname) || user.status <= 0 || user.employee!.gender <= 0 ||
                string.IsNullOrEmpty(user.employee!.address) || string.IsNullOrEmpty(user.employee!.city)||
                string.IsNullOrEmpty(user.employee!.state) || string.IsNullOrEmpty(user.employee!.zipcode) ||
                string.IsNullOrEmpty(user.employee!.jobPosition) || string.IsNullOrEmpty(user.employee!.profession) ||
                user.employee!.contactPhones!.Count == 0 || user.status <= 0 || user.userRole <= 0 || user.userAccess!.Count == 0)
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
                string.IsNullOrEmpty(user.lastname) || user.status <= 0 || user.employee!.gender <= 0 ||
                string.IsNullOrEmpty(user.employee!.address) || string.IsNullOrEmpty(user.employee!.city)||
                string.IsNullOrEmpty(user.employee!.state) || string.IsNullOrEmpty(user.employee!.zipcode) ||
                string.IsNullOrEmpty(user.employee!.jobPosition) || string.IsNullOrEmpty(user.employee!.profession) ||
                user.employee!.contactPhones!.Count == 0 || user.status <= 0 || user.id <= 0 || user.employee!.id <= 0 ||
                user.userRole <= 0 || user.userAccess!.Count == 0)
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