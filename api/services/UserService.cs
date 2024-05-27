namespace api.services;

using authorization;
using providerData.entities;
using business.facade;
using entities.models;

public interface IUserService
{
    AuthenticateResponse? authenticate(AuthenticateRequest model);
}

public class UserService : IUserService
{
    private readonly IJwtUtils _jwtUtils;

    public UserService(IJwtUtils jwtUtils)
    {
        _jwtUtils = jwtUtils;
    }

    public AuthenticateResponse? authenticate(AuthenticateRequest model)
    {
        var user = new FacadeUser("${Guid.NewGuid()}").getUserByIdUser(model.id);
        if (user == null)
            return null;

        var token = _jwtUtils.generateJwtToken(new UserModel {
            id = user.id,
            username = user.username,
            firstname = user.firstname,
            lastname = user.lastname,
            expirationDate = model.expirationDate
        });

        return new AuthenticateResponse(user, token);
    }
}