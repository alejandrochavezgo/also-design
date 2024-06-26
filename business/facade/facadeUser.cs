namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;

public class facadeUser
{
    private log _logger;
    
    public List<userModel> getUsers()
    {
        try
        {
            return new repositoryUser().getUsers();
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool updateUser(userModel user)
    {
        try
        {
            return new repositoryUser().updateUser(user);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}