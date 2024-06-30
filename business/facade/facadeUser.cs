namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;

public class facadeUser
{
    private log _logger = new log();

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
            user.email = user.email.Trim().ToUpper();
            user.firstname = user.firstname.Trim().ToUpper();
            user.lastname = user.lastname.Trim().ToUpper();
            user.modificationDate = DateTime.Now;
            return new repositoryUser().updateUser(user);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }

    public bool addUser(userModel user)
    {
        try
        {
            user.username = user.username.Trim().ToUpper();
            user.firstname = user.firstname.Trim().ToUpper();
            user.lastname = user.lastname.Trim().ToUpper();
            user.creationDate = DateTime.Now;
            user.email = user.email.Trim().ToUpper();
            user.isLocked = false;
            user.failCount = 0;
            return new repositoryUser().addUser(user);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}