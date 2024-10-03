namespace common.helpers;

using common.logging;
using entities.models;
using Newtonsoft.Json;

public class clientFormHelper
{
    private log _logger;

    public clientFormHelper ()
    {
        _logger = new log();
    }

    public bool isAddFormValid(clientModel client)
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
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool isUpdateFormValid(clientModel client)
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