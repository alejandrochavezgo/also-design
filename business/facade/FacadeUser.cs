namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;

public class FacadeUser {

    private Log _logger;
    
    public UserModel getUserByIdUser(int idUser)
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