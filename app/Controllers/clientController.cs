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

    [HttpGet("client/detail")]
    public async Task<IActionResult> detail(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var client = new clientModel { id = id };
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:client:getClientById"]}", new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json"));

            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePost.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePost.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }

            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<entities.models.clientModel>(responsePostAsJson);
            clientHttp.Dispose();

            ViewData["id"] = result!.id;
            ViewData["businessName"] = result!.businessName;
            ViewData["rfc"] = result!.rfc;
            ViewData["address"] = result!.address;
            ViewData["zipcode"] = result!.zipCode;
            ViewData["city"] = result!.city;
            ViewData["state"] = result!.state;
            ViewData["country"] = result!.country;
            ViewData["statusName"] = result!.statusName;
            ViewData["statusColor"] = result!.statusColor;
            ViewData["contactEmails"] = result!.contactEmails;
            ViewData["contactPhones"] = result!.contactPhones;
            ViewData["contactNames"] = result!.contactNames;
            return View();
        }
        catch(Exception e)
        {
            return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpGet("client/getCatalogs")]
    public async Task<IActionResult> getCatalogs()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:client:getAllClientCatalogs"]}");
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
    public async Task<IActionResult> update(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var client = new clientModel { id = id };
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:client:getClientById"]}", new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json"));

            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePost.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePost.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }

            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<entities.models.clientModel>(responsePostAsJson);
            clientHttp.Dispose();

            ViewData["id"] = result!.id;
            ViewData["businessName"] = result!.businessName;
            ViewData["rfc"] = result!.rfc;
            ViewData["address"] = result!.address;
            ViewData["zipcode"] = result!.zipCode;
            ViewData["city"] = result!.city;
            ViewData["state"] = result!.state;
            ViewData["country"] = result!.country;
            ViewData["status"] = result!.status;
            ViewData["contactEmails"] = result!.contactEmails;
            ViewData["contactPhones"] = result!.contactPhones;
            ViewData["contactNames"] = result!.contactNames;

            return View();
        }
        catch(Exception e)
        {
            return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpGet("client/getAll")]
    public async Task<JsonResult> getAll()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:client:getAll"]}");
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

    [HttpGet("client/getClientByTerm")]
    public async Task<IActionResult> getClientByTerm(string businessName)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var client = new clientModel { businessName = businessName };
            var responseGet = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:client:getClientsByTerm"]}", new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json"));

            if(!responseGet.IsSuccessStatusCode)
            {
                var errorMessage = await responseGet.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responseGet.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }

            var responseGetAsJson = await responseGet.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<List<entities.models.clientModel>>(responseGetAsJson);
            clientHttp.Dispose();

            return Json(results);
        }
        catch (Exception e)
        {
            return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpPost("client/add")]
    public async Task<JsonResult> add([FromBody] clientModel client)
    {
        try
        {
            if (!ModelState.IsValid || !clientFormHelper.isAddFormValid(client))
                return Json(new
                { 
                    isSuccess = false,
                    message = "Invalid data."
                });

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync(configurationManager.appSettings["api:routes:client:add"], new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json"));

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

    [HttpPost("client/update")]
    public async Task<JsonResult> update([FromBody] clientModel client)
    {
        try
        {
            if (!ModelState.IsValid || !clientFormHelper.isUpdateFormValid(client))
                return Json(new
                {
                    isSuccess = false,
                    message = "Invalid data."
                });

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync(configurationManager.appSettings["api:routes:client:update"], new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json"));

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

    [HttpPost("client/delete")]
    public async Task<JsonResult> delete([FromBody] clientModel client)
    {
        try
        {
            if (!ModelState.IsValid || !clientFormHelper.isUpdateFormValid(client, true))
                return Json(new
                {
                    isSuccess = false,
                    message = "To delete a client, they must first be active."
                });

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:client:delete"]}", new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json"));

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
                message = "Client deleted successfully."
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

    [HttpGet("client/getClientTracesByClientId")]
    public async Task<IActionResult> getClientTracesByClientId(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:client:getClientTracesByClientId"]}?id={id}");
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

    [HttpGet("client/getClientTraceById")]
    public async Task<IActionResult> getClientTraceById(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:client:getClientTraceById"]}?id={id}");
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
}