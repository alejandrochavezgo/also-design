namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;

public class facadeMenu
{
    private Log _logger;
    
    public List<menuModel> getUserMenusByIdUser(int idUser)
    {
        try
        {
            return new repositoryMenu().getUserMenusByIdUser(idUser);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}