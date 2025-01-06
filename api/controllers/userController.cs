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
public class userController : ControllerBase
{
    private userModel _user;
    private facadeUser _facadeUser;
    private facadeTrace _facadeTrace;
    private IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public userController(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _user = (providerData.entitiesData.userModel)_httpContextAccessor!.HttpContext?.Items["user"]!;
        if (_user != null)
            _facadeUser = new facadeUser(new entities.models.userModel { id = _user.id });
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

    [allowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult authenticate(authenticateRequest model)
    {
        try
        {
            var response = _userService.authenticate(model);
            if(response == null)
                return BadRequest(new { message = "Username or password is incorrect." });

            return Ok(response);
        }
        catch (Exception exception)
        {
            return BadRequest(exception);
        }
    }

    [HttpGet("getAll")]
    public IActionResult getAll()
    {
        try
        {
            return Ok(_facadeUser.getUsers());
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

    [HttpPost("update")]
    public IActionResult update(entities.models.userModel user)
    {
        try
        {
            if(user == null || !new userFormHelper().isUpdateFormValid(user))
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

    [HttpPost("generateUser")]
    public IActionResult generateUser(entities.models.userModel user)
    {
        try
        {
            if(user == null)
                return BadRequest("The user was not generated.");

            if (!_facadeUser.addUserToEmployee(user))
                return BadRequest("User not generated.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("add")]
    public IActionResult add(entities.models.userModel user)
    {
        try
        {
            if (user == null || !new userFormHelper().isAddFormValid(user))
                return BadRequest("The user was not created.");

            if (_facadeUser.getUserByEmailOrUsername(user.email!.ToUpper().Trim(), user.username!.ToUpper().Trim()) != null)
                return BadRequest("This email or username already exist.");

            if (!_facadeUser.addUser(user))
                return BadRequest("User not created");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpGet("getUserTracesByUserId")]
    public IActionResult getUserTracesByUserId(int id)
    {
        try
        {
            return Ok(_facadeUser.getUserTracesByUserId(id));
        }
        catch (Exception e)
        {
            return BadRequest(new { isSuccess = false, message = e.Message });
        }
    }

    [HttpGet("getUserTraceById")]
    public IActionResult getUserTraceById(int id)
    {
        try
        {
            return Ok(_facadeUser.getUserTraceById(id));
        }
        catch (Exception e)
        {
            return BadRequest(new { isSuccess = false, message = e.Message });
        }
    }

    [HttpPost("delete")]
    public IActionResult delete(entities.models.userModel user)
    {
        try
        {
            if(!new userFormHelper().isUpdateFormValid(user, true))
                return BadRequest("Missing data.");

            if (!_facadeUser.deleteUserById(user.id))
                return BadRequest("User can't be deleted.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("getUsersByTerm")]
    public IActionResult getUsersByTerm(entities.models.userModel user)
    {
        try
        {
            return Ok(_facadeUser.getUsersByTerm(user.username!));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }
}