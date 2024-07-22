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
public class enterpriseController : ControllerBase
{
    private IUserService _userService;
    private facadeEnterprise _facadeEnterprise;

    public enterpriseController(IUserService userService)
    {
        _userService = userService;
        _facadeEnterprise = new facadeEnterprise();
    }

    [HttpPost("getEnterpriseFullInformationByIdAndConfigType")]
    public IActionResult getEnterpriseFullInformationByIdAndConfigType(entities.models.enterpriseModel enterprise)
    {
        try
        {
            return Ok(_facadeEnterprise.getEnterpriseFullInformationByIdAndConfigType(enterprise.id, enterprise.quotation!.configType));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }
}