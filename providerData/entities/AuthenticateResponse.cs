using entities.models;

namespace providerData.entities;

public class AuthenticateResponse
{
    public int id { get; set; }
    public string? username { get; set; }
    public string? firstName { get; set; }
    public string? lastName { get; set; }
    public string? email { get; set; }
    public string? token { get; set; }


    public AuthenticateResponse(UserModel user, string token)
    {
        id = user.id;
        username = user.username;
        firstName = user.firstname;
        lastName = user.lastname;
        email = user.email;
        this.token = token;
    }
}