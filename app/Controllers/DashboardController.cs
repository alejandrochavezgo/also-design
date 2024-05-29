namespace app.Controllers;

using providerData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using entities.models;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class DashboardController : Controller
{
   private readonly ILogger<DashboardController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IHttpClientFactory _clientFactory;

    public DashboardController(ILogger<DashboardController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _clientFactory = clientFactory;
    }

    [HttpGet]
    public IActionResult dashboard()
    {
        if (Request.Cookies.TryGetValue("userCookie", out var userCookie))
        {
            var userInfo = JsonConvert.DeserializeObject<UserModel>(userCookie);
        }
        else
        {
        }

        return View();
    }
}