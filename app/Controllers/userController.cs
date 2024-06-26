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

[authorization]
public class userController : Controller
{
    private readonly ILogger<userController> _logger;
    private readonly UserManager<applicationUser> _userManager;
    private readonly SignInManager<applicationUser> _signInManager;
    private readonly IHttpClientFactory _clientFactory;

    public userController(ILogger<userController> logger, UserManager<applicationUser> userManager, SignInManager<applicationUser> signInManager, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _clientFactory = clientFactory;
    }

    [HttpGet("list")]
    public IActionResult list()
    {
        return View();
    }

    [HttpGet("updateUserPartial")]
    public IActionResult updateUserPartial(int userId, string email, string firstname, string lastname, bool isActive)
    {
        ViewData["userId"] = userId;
        ViewData["email"] = email;
        ViewData["firstname"] = firstname;
        ViewData["lastname"] = lastname;
        ViewData["isActive"] = isActive;

        return PartialView("_updateUserPartial");
    }

    [HttpPost("updateUser")]
    public async Task<JsonResult> updateUser([FromBody] userModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            { 
                isSuccess = true,
                message = "Invalid data."
            });
        }

        var client = _clientFactory.CreateClient();
        var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
        var responsePost = await client.PostAsync(configurationManager.appSettings["api:routes:user:updateUser"], new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

        if(!responsePost.IsSuccessStatusCode)
        {
            return Json(new
            {
                isSuccess = false,
                message = $"{responsePost.ReasonPhrase}"
            });
        }
        client.Dispose();

        return Json(new 
        { 
            isSuccess = true,
            message = "User updated successfully."
        });
    }

    [HttpGet("getUsers")]
    public async Task<JsonResult> getUsers()
    {
        try
        {
            var client = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await client.GetAsync($"{configurationManager.appSettings["api:routes:user:getUsers"]}");

            if(!responseGet.IsSuccessStatusCode)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = $"{responseGet.ReasonPhrase}"
                });
            }

            var responseGetAsJson = await responseGet.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<IEnumerable<entities.models.userModel>>(responseGetAsJson);
            client.Dispose();

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