namespace api.Controllers;

using Microsoft.AspNetCore.Mvc;
using api.authorization;
using api.services;
using business.facade;
using providerData.entitiesData;
using Newtonsoft.Json;

[ApiController]
[authorize]
[Route("[controller]")]
public class quotationController : ControllerBase
{
    private IUserService _userService;
    private facadeQuotation _facadeQuotation;

    public quotationController(IUserService userService)
    {
        _userService = userService;
        _facadeQuotation = new facadeQuotation();
    }

    [HttpGet("getAll")]
    public IActionResult getAll()
    {
        try
        {
            return Ok(_facadeQuotation.getQuotations());
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }
}