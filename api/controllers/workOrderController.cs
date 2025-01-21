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

    [HttpGet("getAll")]
    public IActionResult getAll()
    {
        try
        {
            return Ok(_facadeWorkOrder.getWorkOrders());
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("delete")]
    public IActionResult delete(entities.models.workOrderModel workOrder)
    {
        try
        {
            if(!new workOrderFormHelper().isUpdateFormValid(workOrder, true))
                return BadRequest("Missing data.");

            if (!_facadeWorkOrder.deleteWorkOrderById(workOrder.id))
                return BadRequest("Work order can't be deleted.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("getFullWorkOrderById")]
    public IActionResult getFullWorkOrderById(entities.models.workOrderModel workOrder)
    {
        try
        {
            return Ok(_facadeWorkOrder.getFullWorkOrderById(workOrder.id));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("update")]
    public IActionResult update(entities.models.workOrderModel workOrder)
    {
        try
        {
            if(workOrder == null || !new workOrderFormHelper().isUpdateFormValid(workOrder))
                return BadRequest("The Work order was not modified.");

            if (!_facadeWorkOrder.updateWorkOrder(workOrder))
                return BadRequest("Work order not modified.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpGet("getWorkOrderTracesByWorkOrderId")]
    public IActionResult getWorkOrderTracesByWorkOrderId(int id)
    {
        try
        {
            return Ok(_facadeWorkOrder.getWorkOrderTracesByWorkOrderId(id));
        }
        catch (Exception e)
        {
            return BadRequest(new { isSuccess = false, message = e.Message });
        }
    }

    [HttpGet("getWorkOrderTraceById")]
    public IActionResult getWorkOrderTraceById(int id)
    {
        try
        {
            return Ok(_facadeWorkOrder.getWorkOrderTraceById(id));
        }
        catch (Exception e)
        {
            return BadRequest(new { isSuccess = false, message = e.Message });
        }
    }
}