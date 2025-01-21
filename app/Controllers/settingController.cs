namespace app.controllers;

using providerData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using entities.models;
using authorization;
using System.Net.Http.Headers;
using common.configurations;
using System.Text;
using app.helpers;
using providerData.helpers;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using entities.enums;

[authorization]
public class settingController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<settingController> _logger;
    private readonly UserManager<applicationUser> _userManager;
    private readonly SignInManager<applicationUser> _signInManager;

    public settingController(ILogger<settingController> logger, UserManager<applicationUser> userManager, SignInManager<applicationUser> signInManager, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _clientFactory = clientFactory;
    }

    [HttpGet("setting/user")]
    public async Task<IActionResult> user()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var user = new userModel { id = userCookie.id };
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:setting:getUserById"]}", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));

            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePost.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePost.ReasonPhrase : errorMessage;
                return RedirectToAction("errorWithParams", "error", new { errorCode = 0, errorMessage = message });
            }

            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<entities.models.userModel>(responsePostAsJson);
            clientHttp.Dispose();

            ViewData["user.id"] = result!.id;
            ViewData["user.email"] = result!.email;
            ViewData["user.username"] = result!.username;
            ViewData["user.firstname"] = result!.firstname;
            ViewData["user.lastname"] = result!.lastname;
            ViewData["user.userRole"] = result!.userRole;
            ViewData["user.userAccess"] = result!.userAccess;
            ViewData["user.status"] = result!.status;
            ViewData["employee.id"] = result.employee!.id;
            ViewData["employee.gender"] = result.employee!.gender;
            ViewData["employee.address"] = result.employee!.address;
            ViewData["employee.city"] = result.employee!.city;
            ViewData["employee.state"] = result.employee!.state;
            ViewData["employee.zipcode"] = result.employee!.zipcode;
            ViewData["employee.profession"] = result.employee!.profession;
            ViewData["employee.jobPosition"] = result.employee!.jobPosition;
            ViewData["employee.contactPhones"] = result.employee!.contactPhones;
            return View();
        }
        catch(Exception exception)
        {
            return Json(new
            {
                isSuccess = false,
                message = $"{exception.Message}"
            });
        }
    }

    [HttpPost("setting/updateUser")]
    public async Task<JsonResult> updateUser([FromBody] userModel user)
    {
        try
        {
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            if (!ModelState.IsValid || !userSettingFormHelper.isUpdateFormValid(user) || userCookie == null || string.IsNullOrEmpty(userCookie.username))
                return Json(new
                { 
                    isSuccess = false,
                    message = "Invalid data."
                });

            user.username = userCookie!.username;
            if (!string.IsNullOrEmpty(user.password))
            {
                var userIdentity = await _signInManager.UserManager.FindByNameAsync(user.username!);
                if (userIdentity is null || string.IsNullOrEmpty(userIdentity.NormalizedUserName))
                {
                    return Json(new
                    {
                        isSuccess = false,
                        message = "I can't find this username. Check it and try again please."
                    });
                }

                userIdentity.PasswordHash = userSecurityHelper.evaluateHash(_userManager, userIdentity, user.password!);
                if (string.IsNullOrEmpty(userIdentity.PasswordHash))
                {
                    return Json(new
                    {
                        isSuccess = false,
                        url = string.Empty,
                        message = "Password incorrect, check it and try again please."
                    });
                }

                var paswordHash = _userManager.PasswordHasher.VerifyHashedPassword(userIdentity, userIdentity.PasswordHash, user.password!);
                if (paswordHash != PasswordVerificationResult.Success)
                {
                    return Json(new
                    {
                        isSuccess = false,
                        url = string.Empty,
                        message = "Password incorrect, check it and try again please."
                    });
                }

                user.newPasswordHash = userSecurityHelper.generateHash(_userManager,
                new applicationUser()
                {
                    UserName = user.username,
                    NormalizedUserName = user.username,
                    Password = user.newPassword,
                    Email = user.email,
                    NormalizedEmail = user.email
                },
                user.newPassword!);
            }

            var clientHttp = _clientFactory.CreateClient();
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync(configurationManager.appSettings["api:routes:setting:updateUser"], new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));

            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePost.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePost.ReasonPhrase : errorMessage;
                return Json(new
                {
                    isSuccess = false,
                    message = $"{message}"
                });
            }
            clientHttp.Dispose();

            return Json(new
            { 
                isSuccess = true,
                message = "User settings updated successfully."
            });
        }
        catch (Exception exception)
        {
            return Json(new
            {
                isSuccess = false,
                message = $"{exception.Message}"
            });
        }
    }

    [HttpGet("setting/getCatalogs")]
    public async Task<IActionResult> getCatalogs()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:setting:getAllUserCatalogs"]}");
            if(!responseGet.IsSuccessStatusCode)
            {
                var errorMessage = await responseGet.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responseGet.ReasonPhrase : errorMessage;
                return Json(new
                {
                    isSuccess = false,
                    message = $"{message}"
                });
            }

            var responseGetAsJson = await responseGet.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<IEnumerable<IEnumerable<entities.models.catalogModel>>>(responseGetAsJson);
            clientHttp.Dispose();

            return Json(new
            {
                isSuccess = true,
                message = "Ok.",
                results
            });
        }
        catch (Exception exception)
        {
            return Json(new
            {
                isSuccess = false,
                message = $"{exception.Message}"
            });
        }
    }
}