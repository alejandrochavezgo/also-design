namespace api.services;

using authorization;
using providerData.entitiesData;
using business.facade;

public interface IUserService
{
    authenticateResponse? authenticate(authenticateRequest model);
}

public class userService : IUserService
{
    private readonly IJwtUtils _jwtUtils;

    public userService(IJwtUtils jwtUtils)
    {
        _jwtUtils = jwtUtils;
    }

    public authenticateResponse? authenticate(authenticateRequest model)
    {
        var roles = new facadeRole().getUserRolesByIdUser(model.id);
        var menus = new facadeMenu().getUserMenusByIdUser(model.id);
        var user = new userModel()
        {
            id = model.id,
            username = model.username,
            expirationDate = model.expirationDate,
            email = model.email,
            roles = roles,
            menus = menus,
        };
        user.token = _jwtUtils.generateJwtToken(user);
        return new authenticateResponse(user);
    }
}