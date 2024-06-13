namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;

public class facadeRole
{
    private Log _logger;
    
    public List<roleModel> getUserRolesByIdUser(int idUser)
    {
        try
        {
            return new repositoryRole().getUserRolesByIdUser(idUser);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}