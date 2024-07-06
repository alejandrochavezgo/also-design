namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;

public class facadeRole
{
    private log _logger;
    private repositoryRole _repositoryRole;

    public facadeRole()
    {
        _logger = new log();
        _repositoryRole = new repositoryRole();
    }

    public List<roleModel> getUserRolesByUserId(int idUser)
    {
        try
        {
            return _repositoryRole.getUserRolesByUserId(idUser);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}