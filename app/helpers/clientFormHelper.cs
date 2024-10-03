namespace app.helpers;

using entities.models;

public static class clientFormHelper
{
    public static bool isAddFormValid(clientModel client)
    {
        try
        {
            if (string.IsNullOrEmpty(client.businessName) || string.IsNullOrEmpty(client.rfc) ||
                string.IsNullOrEmpty(client.address) || string.IsNullOrEmpty(client.zipCode) ||
                string.IsNullOrEmpty(client.city) || string.IsNullOrEmpty(client.state) ||
                string.IsNullOrEmpty(client.country) || client.status <= 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
    
    public static bool isUpdateFormValid(clientModel client)
    {
        try
        {
            if (string.IsNullOrEmpty(client.businessName) || string.IsNullOrEmpty(client.rfc) ||
                string.IsNullOrEmpty(client.address) || string.IsNullOrEmpty(client.zipCode) ||
                string.IsNullOrEmpty(client.city) || string.IsNullOrEmpty(client.state) ||
                string.IsNullOrEmpty(client.country) || client.status <= 0 || client.id <= 0)
                return false;
            return true;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
}