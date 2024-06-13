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
        var roles = new facadeRole().getUserRolesByIdUser(model.id);
        var menus = new facadeMenu().getUserMenusByIdUser(model.id);
        var user = new UserModel()
        {
            id = model.id,
            username = model.username,
            expirationDate = model.expirationDate,
            email = model.email,
            roles = roles,
            menus = menus,
        };
        user.token = _jwtUtils.generateJwtToken(user);
        return new AuthenticateResponse(user);
    }
}