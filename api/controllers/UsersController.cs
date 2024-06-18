namespace api.Controllers;

using Microsoft.AspNetCore.Mvc;
using api.authorization;
using api.services;
using providerData.entities;
using Newtonsoft.Json;

[ApiController]
// [Authorize]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult authenticate(AuthenticateRequest model)
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

    [HttpGet]
    public IActionResult getTest()
    {
        try
        {
            return Ok("I'm OK!");
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }
}