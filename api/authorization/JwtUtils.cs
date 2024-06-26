namespace api.authorization;

using providerData.helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using providerData.entitiesData;
using Newtonsoft.Json;

public interface IJwtUtils
{
    public string generateJwtToken(userModel user);
    public userModel? validateJwtToken(string? token);
}

public class jwtUtils : IJwtUtils
{
    private readonly appSettingsHelper _appSettings;

    public jwtUtils(IOptions<appSettingsHelper> appSettings)
    {
        _appSettings = appSettings.Value;

        if (string.IsNullOrEmpty(_appSettings.Secret))
            throw new Exception("JWT secret not configured");
    }

    public string generateJwtToken(userModel user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim("id", $"{user.id}"),
                new Claim("username", $"{user.username}"),
                new Claim("firstname", $"{user.firstname}"),
                new Claim("lastname", $"{user.lastname}"),
            }),
            Expires = user.expirationDate,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public userModel? validateJwtToken(string? token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var user = new userModel {
                id = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value)
            };
            
            return user;
        }
        catch
        {
            return null;
        }
    }
}