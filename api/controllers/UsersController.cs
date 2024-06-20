namespace api.Controllers;

using Microsoft.AspNetCore.Mvc;
using api.authorization;
using api.services;
using providerData.entitiesData;
using Newtonsoft.Json;

[ApiController]
[authorize]
[Route("[controller]")]
public class usersController : ControllerBase
{
    private IUserService _userService;

    public usersController(IUserService userService)
    {
        _userService = userService;
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

    [allowAnonymous]
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