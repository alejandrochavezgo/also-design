namespace app.Controllers;

using providerData;
using entities.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using providerData.helpers;
using System.Text;
using Newtonsoft.Json;
using System.Globalization;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IHttpClientFactory _clientFactory;

    public LoginController(ILogger<LoginController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _clientFactory = clientFactory;
    }

    [HttpGet]
    public IActionResult login()
    {
        return View();
    }

    
    [HttpPost]
    public async Task<JsonResult> login([FromBody] LoginModel login)
    {
        try
        {
            if (!ModelState.IsValid)
                return Json(new LoginResultModel
                {
                    isSuccess = false,
                    message = "The information is not valid, please refresh the page and try again."
                });

            var userIdentity = await _signInManager.UserManager.FindByNameAsync(login.username);
            if (userIdentity is null || string.IsNullOrEmpty(userIdentity.NormalizedUserName) || userIdentity.IsPermanentlyDeleted)
            {
                return Json(new LoginResultModel
                {
                    isSuccess = false,
                    message = "I can't find this username. Check it and try again please."
                });
            }

            if (!userIdentity.IsActive)
            {
                return Json(new LoginResultModel
                {
                    isSuccess = false,
                    message = "This user is deactivated."
                });
            }

            if (userIdentity.IsLocked)
            {
                return Json(new LoginResultModel
                {
                    isSuccess = false,
                    message = "This user is locked."
                });
            }

            userIdentity.PasswordHash = UserSecurityHelper.evaluateHash(_userManager, userIdentity, login.password);
            if (string.IsNullOrEmpty(userIdentity.PasswordHash))
            {
                return Json(new LoginResultModel
                {
                    isSuccess = false,
                    message = "Password incorrect, check it and try again please."
                });
            }

            var paswordHash = _userManager.PasswordHasher.VerifyHashedPassword(userIdentity, userIdentity.PasswordHash, login.password);
            if (paswordHash != PasswordVerificationResult.Success)
            {
                return Json(new LoginResultModel
                {
                    isSuccess = false,
                    message = "Password incorrect, check it and try again please."
                });
            }

            //get roles to pass into json

            var expirationDate = DateTime.Now.AddDays(7);
            var json = JsonConvert.SerializeObject(new
            {
                id = userIdentity.NormalizedId,
                username = userIdentity.NormalizedUserName,
                email = userIdentity.NormalizedEmail,
                expirationDate,
                roles = ""
            });

            //var payload = new StringContent(json, Encoding.UTF8, "application/json");
            //var client = _clientFactory.CreateClient();
            //var responsePost = await client.PostAsync("http://localhost:4000/users/authenticate", payload);
            //var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            //var responsePostAsModel = JsonConvert.DeserializeObject<UserTestModel>(responsePostAsJson);
            //client.Dispose();

            ////Validating token...
            //client = _clientFactory.CreateClient();
            //client.DefaultRequestHeaders.Add("Authorization", $"{responsePostAsModel.Token}");
            //var response = await client.GetAsync("http://localhost:4000/users/");
            //var responseJson = await response.Content.ReadAsStringAsync();
            //client.Dispose();
            ////////////////

            //Response.Cookies.Append("token", responsePostAsModel.Token);

            return Json(new LoginResultModel
            {
                isSuccess = true,
                message = "Ok."
            });
        }
        catch (Exception e)
        {
            return Json(new ExceptionResultModel
            {
                isSuccess = false,
                message = $"Unexpected error has ocurred: {e.Message}.\nPlease refresh the page and try again."
            });
        }
    }
}
