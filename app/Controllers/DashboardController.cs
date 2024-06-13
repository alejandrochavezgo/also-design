namespace app.Controllers;

using providerData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using entities.models;
using Microsoft.AspNetCore.Authorization;
using authorization;

public class dashboardController : Controller
{
    private readonly ILogger<dashboardController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IHttpClientFactory _clientFactory;

    public dashboardController(ILogger<dashboardController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _clientFactory = clientFactory;
    }

    [HttpGet]
    [authorization]
    public IActionResult dashboard()
    {
        return View();
    }
}