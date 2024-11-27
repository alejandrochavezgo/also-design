namespace app.helpers;

using entities.enums;
using entities.models;
using NuGet.Common;

public static class employeeFormHelper
{
    public static bool isAddFormValid(userModel nonUser)
    {
        try
        {
            if (string.IsNullOrEmpty(nonUser.firstname) || string.IsNullOrEmpty(nonUser.lastname) || nonUser.status <= 0 || nonUser.employee!.gender <= 0 || 
                string.IsNullOrEmpty(nonUser.employee!.address) || string.IsNullOrEmpty(nonUser.employee!.city) || string.IsNullOrEmpty(nonUser.employee!.state) ||
                string.IsNullOrEmpty(nonUser.employee!.zipcode) || string.IsNullOrEmpty(nonUser.employee.jobPosition) || string.IsNullOrEmpty(nonUser.employee!.profession) ||
                nonUser.employee.contactPhones!.Count == 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public static bool isUpdateFormValid(userModel nonUser)
    {
        try
        {
            if (string.IsNullOrEmpty(nonUser.firstname) || string.IsNullOrEmpty(nonUser.lastname) || nonUser.status <= 0 || nonUser.employee!.gender <= 0 || 
                string.IsNullOrEmpty(nonUser.employee!.address) || string.IsNullOrEmpty(nonUser.employee!.city) || string.IsNullOrEmpty(nonUser.employee!.state) ||
                string.IsNullOrEmpty(nonUser.employee!.zipcode) || string.IsNullOrEmpty(nonUser.employee.jobPosition) || string.IsNullOrEmpty(nonUser.employee!.profession) ||
                nonUser.employee.contactPhones!.Count == 0 || nonUser.id <= 0 || nonUser.employee.id <= 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public static bool isUpdateFormValid(employeeModel employee, bool isStatusChange)
    {
        try
        {
            if (employee == null || employee.id <= 0 || employee.status != (int)statusType.ACTIVE)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
}