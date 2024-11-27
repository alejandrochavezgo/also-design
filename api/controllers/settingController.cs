namespace api.Controllers;

using Microsoft.AspNetCore.Mvc;
using api.authorization;
using api.services;
using business.facade;
using providerData.entitiesData;
using Newtonsoft.Json;
using common.helpers;

[ApiController]
[authorize]
[Route("[controller]")]
public class settingController : ControllerBase
{
    private userModel _user;
    private facadeUser _facadeUser;
    private facadeTrace _facadeTrace;
    private IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public settingController(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _user = (providerData.entitiesData.userModel)_httpContextAccessor!.HttpContext?.Items["user"]!;
        if (_user != null)
            _facadeUser = new facadeUser(new entities.models.userModel { id = _user.id });
    }

    [HttpPost("updateUser")]
    public IActionResult update(entities.models.userModel user)
    {
        try
        {
            if(user == null || !new userSettingFormHelper().isUpdateFormValid(user))
                return BadRequest("The user was not modified.");

            var userFromDb = _facadeUser.getUserByEmail(user.email!.ToUpper().Trim());
            if (userFromDb != null && userFromDb.id != user.id)
                return BadRequest("This email already exist.");

            if (!_facadeUser.updateUser(user))
                return BadRequest("User not updated.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("getUserById")]
    public IActionResult getUserById(entities.models.userModel user)
    {
        try
        {
            return Ok(_facadeUser.getUserById(user.id));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpGet("getAllUserCatalogs")]
    public IActionResult getAllUserCatalogs()
    {
        try
        {
            return Ok(_facadeUser.getAllUserCatalogs());
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }
}