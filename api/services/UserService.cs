namespace api.services;

using authorization;
using providerData.entities;
using business.facade;

public interface IUserService
{
    AuthenticateResponse? authenticate(AuthenticateRequest model);
    IEnumerable<User> getAll();
    User? getById(int id);
}

public class UserService : IUserService
{
    private List<User> _users = new List<User>
    {
        new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
    };

    private readonly IJwtUtils _jwtUtils;

    public UserService(IJwtUtils jwtUtils)
    {
        _jwtUtils = jwtUtils;
    }

    public AuthenticateResponse? authenticate(AuthenticateRequest model)
    {
        var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

        if (user == null) return null;

        var token = _jwtUtils.generateJwtToken(user);


        var temp = new FacadeUser().getUserByIdUser(1);

        return new AuthenticateResponse(user, token);
    }

    public IEnumerable<User> getAll()
    {
        return _users;
    }

    public User? getById(int id)
    {
        return _users.FirstOrDefault(x => x.Id == id);
    }
}