namespace business.facade;

using common.logging;
using data.repositories;
using entities.models;

public class FacadeUser {

    private Log _logger;
    public string? _logTraceId;

    public FacadeUser(string? logTraceId)
    {
        _logTraceId = logTraceId;
    }

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