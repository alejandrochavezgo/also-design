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
using helpers;
using System.Text.RegularExpressions;

[authorization]
public class workOrderController : Controller
{
    private readonly ILogger<workOrderController> _logger;
    private readonly UserManager<applicationUser> _userManager;
    private readonly SignInManager<applicationUser> _signInManager;
    private readonly IHttpClientFactory _clientFactory;

    public workOrderController(ILogger<workOrderController> logger, UserManager<applicationUser> userManager, SignInManager<applicationUser> signInManager, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _clientFactory = clientFactory;
    }

    [HttpGet("workOrder/add")]
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

    // [HttpPost("workOrder/add")]
    // public async Task<JsonResult> add([FromBody] workOrderModel workOrder)
    // {
    //     try
    //     {
    //         if (!ModelState.IsValid || !clientFormHelper.isAddFormValid(client))
    //             return Json(new
    //             { 
    //                 isSuccess = false,
    //                 message = "Invalid data."
    //             });

    //         var clientHttp = _clientFactory.CreateClient();
    //         var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
    //         clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
    //         var responsePost = await clientHttp.PostAsync(configurationManager.appSettings["api:routes:client:add"], new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json"));

    //         if(!responsePost.IsSuccessStatusCode)
    //         {
    //             var errorMessage = await responsePost.Content.ReadAsStringAsync();
    //             var message = string.IsNullOrEmpty(errorMessage) ? responsePost.ReasonPhrase : errorMessage;
    //             return Json(new
    //             {
    //                 isSuccess = false,
    //                 message = $"{message}"
    //             });
    //         }
    //         clientHttp.Dispose();

    //         return Json(new
    //         {
    //             isSuccess = true,
    //             message = "Client added successfully."
    //         });
    //     }
    //     catch (Exception exception)
    //     {
    //         return Json(new
    //         {
    //             isSuccess = false,
    //             message = $"{exception.Message}"
    //         });
    //     }
    // }

    [HttpGet("workOrder/getCatalogs")]
    public async Task<IActionResult> getCatalogs()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:workOrder:getAllWorkOrderCatalogs"]}");

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