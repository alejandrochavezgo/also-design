namespace app.Controllers;

using providerData;
using entities.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using providerData.helpers;
using System.Text;
using Newtonsoft.Json;
using common.configurations;
using providerData.entitiesData;
using entities.enums;

public class loginController : Controller
{
    private readonly ILogger<loginController> _logger;
    private readonly UserManager<applicationUser> _userManager;
    private readonly SignInManager<applicationUser> _signInManager;
    private readonly IHttpClientFactory _clientFactory;

    public loginController(ILogger<loginController> logger, UserManager<applicationUser> userManager, SignInManager<applicationUser> signInManager, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _clientFactory = clientFactory;
    }

    [HttpGet]
    public IActionResult login()
    {
        Response.Cookies.Delete("userCookie");
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> login([FromBody] loginModel login)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    isSuccess = false,
                    url = string.Empty,
                    message = "The information is not valid, please refresh the page and try again."
                });
            }

            var userIdentity = await _signInManager.UserManager.FindByNameAsync(login.username!);
            if (userIdentity is null || string.IsNullOrEmpty(userIdentity.NormalizedUserName))
            {
                return Json(new
                {
                    isSuccess = false,
                    url = string.Empty,
                    message = "I can't find this username. Check it and try again please."
                });
            }

            if (userIdentity.StatusId == (int)statusType.INACTIVE)
            {
                return Json(new
                {
                    isSuccess = false,
                    url = string.Empty,
                    message = "This user is deactivated."
                });
            }

            if (userIdentity.StatusId == (int)statusType.LOCKED)
            {
                return Json(new
                {
                    isSuccess = false,
                    url = string.Empty,
                    message = "This user is locked."
                });
            }

            if (userIdentity.StatusId != (int)statusType.ACTIVE)
            {
                return Json(new
                {
                    isSuccess = false,
                    url = string.Empty,
                    message = $"This user has {userIdentity.StatusId} as status id."
                });
            }

            userIdentity.PasswordHash = userSecurityHelper.evaluateHash(_userManager, userIdentity, login.password!);
            if (string.IsNullOrEmpty(userIdentity.PasswordHash))
            {
                return Json(new
                {
                    isSuccess = false,
                    url = string.Empty,
                    message = "Password incorrect, check it and try again please."
                });
            }

            var paswordHash = _userManager.PasswordHasher.VerifyHashedPassword(userIdentity, userIdentity.PasswordHash, login.password!);
            if (paswordHash != PasswordVerificationResult.Success)
            {
                return Json(new
                {
                    isSuccess = false,
                    url = string.Empty,
                    message = "Password incorrect, check it and try again please."
                });
            }

            var expirationDate = DateTime.Now.AddDays(7);
            var client = _clientFactory.CreateClient();
            var responsePost = await client.PostAsync(configurationManager.appSettings["api:routes:login:authenticate"], new StringContent(JsonConvert.SerializeObject(new
            {
                id = userIdentity.NormalizedId,
                username = userIdentity.NormalizedUserName,
                email = userIdentity.NormalizedEmail,
                expirationDate,
            }), Encoding.UTF8, "application/json"));

            if (!responsePost.IsSuccessStatusCode)
            {
                return Json(new
                {
                    isSuccess = false,
                    url = string.Empty,
                    message = responsePost.RequestMessage
                });
            }

            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(responsePostAsJson);
            user!.firstname = userIdentity.Firstname;
            user.lastname = userIdentity.Lastname;
            client.Dispose();

            Response.Cookies.Append("userCookie",
            JsonConvert.SerializeObject(new
            {
                id = user.id,
                username = user.username,
                firstname = user.firstname,
                lastname = user.lastname,
                roles = user.roles,
                menus = user.menus,
                token = user.token
            }), 
            new CookieOptions {
                Expires = expirationDate,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Json(new
            {
                isSuccess = true,
                url = "/dashboard/dashboard",
                message = "Ok."
            });
        }
        catch (Exception e)
        {
            return Json(new
            {
                isSuccess = false,
                url = string.Empty,
                message = $"Unexpected error has ocurred: {e.Message}.\nPlease refresh the page and try again."
            });
        }
    }

    [HttpPost]
    public JsonResult logout()
    {
        try
        {
            Response.Cookies.Delete("userCookie");
            return Json(new
            {
                isSuccess = true,
                url = "/login/login",
                message = "Ok."
            });
        }
        catch (Exception e)
        {
            return Json(new
            {
                isSuccess = false,
                url = string.Empty,
                message = $"Unexpected error has ocurred: {e.Message}.\nPlease refresh the page and try again."
            });
        }
    }

    [HttpGet]
    public IActionResult passwordReset()
    {
        return View();
    }
}
