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
public class clientController : Controller
{
    private readonly ILogger<clientController> _logger;
    private readonly UserManager<applicationUser> _userManager;
    private readonly SignInManager<applicationUser> _signInManager;
    private readonly IHttpClientFactory _clientFactory;

    public clientController(ILogger<clientController> logger, UserManager<applicationUser> userManager, SignInManager<applicationUser> signInManager, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _clientFactory = clientFactory;
    }

    [HttpGet("client/list")]
    public IActionResult list()
    {
        try
        {
            return View();
        }
        catch (Exception e)
        {
            return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpGet("client/add")]
    public IActionResult add()
    {
        try
        {
            return View();
        } 
        catch (Exception e)
        {
            return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }
    
    [HttpGet("client/update")]
    public async Task<IActionResult> update(int clientId)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var client = new clientModel { id = clientId };
            var responseGet = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:client:getClientById"]}", new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json"));

            if(!responseGet.IsSuccessStatusCode)
            {
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = responseGet.ReasonPhrase });
            }

            var responseGetAsJson = await responseGet.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<entities.models.clientModel>(responseGetAsJson);
            clientHttp.Dispose();

            ViewData["id"] = result!.id;
            ViewData["businessName"] = result!.businessName;
            ViewData["rfc"] = result!.rfc;
            ViewData["address"] = result!.address;
            ViewData["zipcode"] = result!.zipCode;
            ViewData["city"] = result!.city;
            ViewData["state"] = result!.state;
            ViewData["country"] = result!.country;
            ViewData["isActive"] = result!.isActive;
            ViewData["contactEmails"] = result!.contactEmails;
            ViewData["contactPhones"] = result!.contactPhones;

            return View();
        }
        catch(Exception e)
        {
            return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpGet("client/getClients")]
    public async Task<JsonResult> getClients()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:client:getClients"]}");

            if(!responseGet.IsSuccessStatusCode)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = $"{responseGet.ReasonPhrase}"
                });
            }

            var responseGetAsJson = await responseGet.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<IEnumerable<entities.models.clientModel>>(responseGetAsJson);
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

    [HttpPost("client/addClient")]
    public async Task<JsonResult> addClient([FromBody] clientModel client)
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
            var responsePost = await clientHttp.PostAsync(configurationManager.appSettings["api:routes:client:addClient"], new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json"));

            if(!responsePost.IsSuccessStatusCode)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = $"{responsePost.ReasonPhrase}"
                });
            }
            clientHttp.Dispose();

            return Json(new
            { 
                isSuccess = true,
                message = "Client added successfully."
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

    [HttpPost("client/updateClient")]
    public async Task<JsonResult> updateClient([FromBody] clientModel client)
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
            var responsePost = await clientHttp.PostAsync(configurationManager.appSettings["api:routes:client:updateClient"], new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json"));

            if(!responsePost.IsSuccessStatusCode)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = $"{responsePost.ReasonPhrase}"
                });
            }
            clientHttp.Dispose();

            return Json(new
            { 
                isSuccess = true,
                message = "Client updated successfully."
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