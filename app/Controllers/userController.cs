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
using providerData.helpers;

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

    [HttpGet("user/list")]
    public IActionResult list()
    {
        return View();
    }

    [HttpGet("user/getCatalogs")]
    public async Task<IActionResult> getCatalogs()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:user:getAllUserCatalogs"]}");
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

    [HttpGet("user/add")]
    public IActionResult add()
    {
        return View();
    }

    [HttpPost("user/add")]
    public async Task<JsonResult> add([FromBody] userModel user)
    {
        try
        {
            if (!ModelState.IsValid)
                {
                    return Json(new
                    { 
                        isSuccess = false,
                        message = "Invalid data."
                    });
                }

            user.passwordHash = userSecurityHelper.generateHash(_userManager,
            new applicationUser()
            {
                UserName = user.username,
                NormalizedUserName = user.username,
                Password = user.password,
                Email = user.email,
                NormalizedEmail = user.email
            },
            user.password!);

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync(configurationManager.appSettings["api:routes:user:add"], new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));

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
                message = "User added successfully."
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

    [HttpGet("user/update")]
    public async Task<IActionResult> update(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var user = new userModel { id = id };
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:user:getUserById"]}", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));

            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePost.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePost.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }

            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<entities.models.userModel>(responsePostAsJson);
            clientHttp.Dispose();

            ViewData["user.id"] = result!.id;
            ViewData["user.email"] = result!.email;
            ViewData["user.firstname"] = result!.firstname;
            ViewData["user.lastname"] = result!.lastname;
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

    [HttpGet("user/detail")]
    public async Task<IActionResult> detail(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var user = new userModel { id = id };
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:user:getUserById"]}", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));

            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePost.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePost.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }

            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<entities.models.userModel>(responsePostAsJson);
            clientHttp.Dispose();

            ViewData["user.id"] = result!.id;
            ViewData["user.email"] = result!.email;
            ViewData["user.firstname"] = result!.firstname;
            ViewData["user.lastname"] = result!.lastname;
            ViewData["statusName"] = result!.statusName;
            ViewData["statusColor"] = result!.statusColor;
            ViewData["employee.id"] = result.employee!.id;
            ViewData["employee.gender"] = result.employee!.genderDescription;
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

    [HttpPost("user/update")]
    public async Task<JsonResult> update([FromBody] userModel user)
    {
        try
        {
            if (!ModelState.IsValid)
                {
                    return Json(new
                    { 
                        isSuccess = false,
                        message = "Invalid data."
                    });
                }

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync(configurationManager.appSettings["api:routes:user:update"], new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));

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
                message = "User updated successfully."
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

    [HttpGet("user/getAll")]
    public async Task<JsonResult> getAll()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:user:getAll"]}");

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
            var results = JsonConvert.DeserializeObject<IEnumerable<entities.models.userModel>>(responseGetAsJson);
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