namespace app.controllers;

using providerData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using entities.models;
using Microsoft.AspNetCore.Authorization;
using authorization;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Internal;

[authorization]
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
    public IActionResult error(int errorCode, string errorMessage)
    {
        try
        {
            return View(new errorModel
            {
                errorCode = errorCode,
                errorMessage = errorMessage
            });
        }
        catch(Exception e)
        {
            throw e;
        }
    }
}