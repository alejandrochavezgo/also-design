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
public class workOrderController : ControllerBase
{
    private IUserService _userService;
    private userModel _user;
    private facadeWorkOrder _facadeWorkOrder;
    private facadeTrace _facadeTrace;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public workOrderController(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _user = (providerData.entitiesData.userModel)_httpContextAccessor!.HttpContext?.Items["user"]!;
        if (_user != null)
            _facadeWorkOrder = new facadeWorkOrder(new entities.models.userModel { id = _user.id });
    }

    [HttpPost("add")]
    public IActionResult add(entities.models.workOrderModel workOrder)
    {
        try
        {
            if(workOrder == null || !new workOrderFormHelper().isAddFormValid(workOrder))
                return BadRequest("Missing data.");

            if (!_facadeWorkOrder.addWorkOrder(workOrder))
                return BadRequest("Work Order not added.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
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