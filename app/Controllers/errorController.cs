namespace app.controllers;

using providerData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

public class errorController : Controller
{
    private readonly ILogger<errorController> _logger;
    private readonly UserManager<applicationUser> _userManager;
    private readonly SignInManager<applicationUser> _signInManager;
    private readonly IHttpClientFactory _clientFactory;

    public errorController(ILogger<errorController> logger, UserManager<applicationUser> userManager, SignInManager<applicationUser> signInManager, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _clientFactory = clientFactory;
    }

    [HttpGet]
    public IActionResult errorWithParams(int errorCode, string errorMessage)
    {
        try
        {
            TempData["errorCode"] = errorCode;
            TempData["errorMessage"] = errorMessage;
            return RedirectToAction("error");
        }
        catch(Exception exception)
        {
            throw exception;
        }
    }

    [HttpGet]
    public IActionResult error()
    {
        try
        {
            ViewData["errorCode"] = TempData["errorCode"];
            ViewData["errorMessage"] = TempData["errorMessage"];
            return View();
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
}