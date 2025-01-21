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
using app.helpers;

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
            if (!ModelState.IsValid || !userFormHelper.isAddFormValid(user))
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
                return RedirectToAction("errorWithParams", "error", new { errorCode = 0, errorMessage = message });
            }

            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<entities.models.userModel>(responsePostAsJson);
            clientHttp.Dispose();

            ViewData["user.id"] = result!.id;
            ViewData["user.email"] = result!.email;
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
                return RedirectToAction("errorWithParams", "error", new { errorCode = 0, errorMessage = message });
            }

            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<entities.models.userModel>(responsePostAsJson);
            clientHttp.Dispose();

            ViewData["user.id"] = result!.id;
            ViewData["user.email"] = result!.email;
            ViewData["user.firstname"] = result!.firstname;
            ViewData["user.lastname"] = result!.lastname;
            ViewData["user.userRoleDescription"] = result!.userRoleDescription;
            ViewData["user.userAccess"] = result!.userAccess;
            ViewData["statusName"] = result!.statusName;
            ViewData["statusColor"] = result!.statusColor;
            ViewData["employee.id"] = result.employee!.id;
            ViewData["employee.genderDescription"] = result.employee!.genderDescription;
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
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            if (!ModelState.IsValid || !userFormHelper.isUpdateFormValid(user))
                return Json(new
                { 
                    isSuccess = false,
                    message = "Invalid data."
                });

            user.username = userCookie!.username;
            if (!string.IsNullOrEmpty(user.newPassword))
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

    [HttpGet("user/getUserTracesByUserId")]
    public async Task<IActionResult> getUserTracesByUserId(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:user:getUserTracesByUserId"]}?id={id}");
            if (!responseGet.IsSuccessStatusCode)
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
            var results = JsonConvert.DeserializeObject<IEnumerable<traceModel>>(responseGetAsJson);
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

    [HttpGet("user/getUserTraceById")]
    public async Task<IActionResult> getUserTraceById(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:user:getUserTraceById"]}?id={id}");
            if (!responseGet.IsSuccessStatusCode)
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
            var results = JsonConvert.DeserializeObject<traceModel>(responseGetAsJson);
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

    [HttpPost("user/delete")]
    public async Task<JsonResult> delete([FromBody] userModel user)
    {
        try
        {
            if (!ModelState.IsValid || !userFormHelper.isUpdateFormValid(user, true))
                return Json(new
                {
                    isSuccess = false,
                    message = "To delete a user, they must first be active."
                });

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:user:delete"]}", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));

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
                message = "User deleted successfully."
            });
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

    [HttpGet("user/getUsersByTerm")]
    public async Task<IActionResult> getUsersByTerm(string username)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var user = new userModel { username = username };
            var responseGet = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:user:getUsersByTerm"]}", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));

            if(!responseGet.IsSuccessStatusCode)
            {
                var errorMessage = await responseGet.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responseGet.ReasonPhrase : errorMessage;
                return RedirectToAction("errorWithParams", "error", new { errorCode = 0, errorMessage = message });
            }

            var responseGetAsJson = await responseGet.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<List<userModel>>(responseGetAsJson);
            clientHttp.Dispose();

            return Json(results);
        }
        catch (Exception e)
        {
            return RedirectToAction("errorWithParams", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }
}