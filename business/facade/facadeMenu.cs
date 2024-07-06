namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;
using Newtonsoft.Json;

public class facadeMenu
{
    private log _logger;
    private repositoryMenu _repositoryMenu;

    public facadeMenu()
    {
        _logger = new log();
        _repositoryMenu = new repositoryMenu();
    }

    public List<menuModel> getUserMenusByUserId(int idUser)
    {
        try
        {
            return _repositoryMenu.getUserMenusByUserId(idUser);
        }
        catch (Exception exception)
        {
            _logger.logError($"{JsonConvert.SerializeObject(exception)}");
            throw exception;
        }
    }
}