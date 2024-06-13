using entities.models;

namespace providerData.entities;

public class AuthenticateResponse
{
    public int id { get; set; }
    public string? username { get; set; }
    public string? email { get; set; }
    public DateTime? expirationDate { get; set; }
    public string? token { get; set; }
    public List<roleModel> roles { get; set; }
    public List<menuModel> menus { get; set; }


    public AuthenticateResponse(UserModel user)
    {
        id = user.id;
        username = user.username;
        email = user.email;
        token = user.token;
        roles = user.roles;
        menus = user.menus;
    }
}