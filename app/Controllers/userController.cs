namespace app.controllers;

using providerData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using entities.models;
using Microsoft.AspNetCore.Authorization;
using authorization;
using NuGet.Common;

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

    [HttpGet]
    [authorization]
    public IActionResult list()
    {
        return View();
    }

    [HttpGet]
    [authorization]
    public IActionResult getUsers()
    {
        return Json(new List<object>
        {
            new {
                id = 1,
                email = "alejandrocg1015@gmail.com",
                username = "achavez",
                firstname = "Alejandro",
                lastname = "Chavez",
                status = "active",
                color = "success",
                creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            },
            new {
                id = 2,
                email = "user2@example.com",
                username = "user2",
                firstname = "John",
                lastname = "Doe",
                status = "locked",
                color = "warning",
                creationDate = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd HH:mm:ss")
            },
            new {
                id = 3,
                email = "user3@example.com",
                username = "user3",
                firstname = "Jane",
                lastname = "Smith",
                status = "active",
                color = "dark",
                creationDate = DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd HH:mm:ss")
            },
            new {
                id = 4,
                email = "user4@example.com",
                username = "user4",
                firstname = "Michael",
                lastname = "Johnson",
                status = "inactive",
                color = "dark",
                creationDate = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss")
            },
            new {
                id = 5,
                email = "user5@example.com",
                username = "user5",
                firstname = "Emily",
                lastname = "Brown",
                status = "locked",
                color = "warning",
                creationDate = DateTime.Now.AddDays(-8).ToString("yyyy-MM-dd HH:mm:ss")
            },
            new {
                id = 6,
                email = "user6@example.com",
                username = "user6",
                firstname = "William",
                lastname = "Wilson",
                status = "inactive",
                color = "dark",
                creationDate = DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd HH:mm:ss")
            },
            new {
                id = 7,
                email = "user7@example.com",
                username = "user7",
                firstname = "Olivia",
                lastname = "Garcia",
                status = "locked",
                color = "warning",
                creationDate = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd HH:mm:ss")
            },
            new {
                id = 8,
                email = "user8@example.com",
                username = "user8",
                firstname = "Daniel",
                lastname = "Martinez",
                status = "inactive",
                color = "dark",
                creationDate = DateTime.Now.AddDays(-12).ToString("yyyy-MM-dd HH:mm:ss")
            },
            new {
                id = 9,
                email = "user9@example.com",
                username = "user9",
                firstname = "Sophia",
                lastname = "Lopez",
                status = "active",
                color = "success",
                creationDate = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd HH:mm:ss")
            },
            new {
                id = 10,
                email = "user10@example.com",
                username = "user10",
                firstname = "David",
                lastname = "Hernandez",
                status = "inactive",
                color = "dark",
                creationDate = DateTime.Now.AddDays(-18).ToString("yyyy-MM-dd HH:mm:ss")
            }
        });
    }
}