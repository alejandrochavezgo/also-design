namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;

public class FacadeUser {

    private Log _logger;
    private string? _logTraceId;
    
    public List<UserModel> getUserByIdUser(int idUser) 
    {
        try 
        {
            return new RepositoryUser().getUserByIdUser(idUser);
        }
        catch (Exception exception) 
        {
            throw exception;
        }
    }
}