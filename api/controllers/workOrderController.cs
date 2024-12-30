namespace api.Controllers;

using Microsoft.AspNetCore.Mvc;
using api.authorization;
using api.services;
using business.facade;
using providerData.entitiesData;
using common.helpers;
using Newtonsoft.Json;

[ApiController]
[authorize]
[Route("[controller]")]
public class workOrderController : ControllerBase
{
    private IUserService _userService;
    private facadeWorkOrder _facadeWorkOrder;

    public workOrderController(IUserService userService)
    {
        _userService = userService;
        _facadeWorkOrder = new facadeWorkOrder();
    }

    [HttpGet("getAllWorkOrderCatalogs")]
    public IActionResult getAllWorkOrderCatalogs()
    {
        try
        {
            return Ok(_facadeWorkOrder.getAllWorkOrderCatalogs());
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }
}