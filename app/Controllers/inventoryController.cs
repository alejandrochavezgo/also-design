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
public class inventoryController : Controller
{
    private readonly ILogger<inventoryController> _logger;
    private readonly UserManager<applicationUser> _userManager;
    private readonly SignInManager<applicationUser> _signInManager;
    private readonly IHttpClientFactory _clientFactory;

    public inventoryController(ILogger<inventoryController> logger, UserManager<applicationUser> userManager, SignInManager<applicationUser> signInManager, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _clientFactory = clientFactory;
    }

    [HttpGet("inventory/getItemByTerm")]
    public async Task<IActionResult> getItemByTerm(string description)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var client = new inventoryModel { description = description };
            var responseGet = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:inventory:getItemByTerm"]}", new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json"));

            if(!responseGet.IsSuccessStatusCode)
            {
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = responseGet.ReasonPhrase });
            }

            var responseGetAsJson = await responseGet.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<List<entities.models.inventoryModel>>(responseGetAsJson);
            clientHttp.Dispose();

            return Json(results);
        }
        catch (Exception e)
        {
            return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }
}