namespace api.Controllers;

using Microsoft.AspNetCore.Mvc;
using api.authorization;
using api.services;
using business.facade;
using providerData.entitiesData;
using common.helpers;
using Newtonsoft.Json;
using entities.enums;

[ApiController]
[authorize]
[Route("[controller]")]
public class purchaseOrderController : ControllerBase
{
    private IUserService _userService;
    private facadePurchaseOrder _facadePurchaseOrder;
    private providerData.entitiesData.userModel _user;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public purchaseOrderController(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _user = (providerData.entitiesData.userModel)_httpContextAccessor!.HttpContext?.Items["user"]!;
        _facadePurchaseOrder = new facadePurchaseOrder(new entities.models.userModel { id = _user.id });
    }

    [HttpGet("getAllPurchaseOrderCatalogs")]
    public IActionResult getAllPurchaseOrderCatalogs()
    {
        try
        {
            return Ok(_facadePurchaseOrder.getAllPurchaseOrderCatalogs());
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
            return Ok(_facadePurchaseOrder.getPurchaseOrders());
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("add")]
    public IActionResult add(entities.models.purchaseOrderModel purchaseOrder)
    {
        try
        {
            if(purchaseOrder == null || !new purchaseOrderFormHelper().isAddFormValid(purchaseOrder))
                return BadRequest("Missing data.");

            if (!_facadePurchaseOrder.addPurchaseOrder(purchaseOrder))
                return BadRequest("Purchase order not added.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("getPendingPurchaseOrderItemsByPurchaseOrderId")]
    public IActionResult getPendingPurchaseOrderItemsByPurchaseOrderId(entities.models.purchaseOrderModel purchaseOrder)
    {
        try
        {
            if(purchaseOrder == null || purchaseOrder.id <= 0)
                return BadRequest("Missing data.");

            return Ok(_facadePurchaseOrder.getPendingPurchaseOrderItemsByPurchaseOrderId(purchaseOrder.id));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("update")]
    public IActionResult update(entities.models.purchaseOrderModel purchaseOrder)
    {
        try
        {
            if(purchaseOrder == null || !new purchaseOrderFormHelper().isUpdateFormValid(purchaseOrder))
                return BadRequest("The Purchase Order was not modified.");

            if (!_facadePurchaseOrder.updatePurchaseOrder(purchaseOrder))
                return BadRequest("Purchase order not updated.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("deletePurchaseOrderById")]
    public IActionResult deletePurchaseOrderById(entities.models.purchaseOrderModel purchaseOrder)
    {
        try
        {
            if(purchaseOrder == null || purchaseOrder.status != (int)statusType.ACTIVE)
                return BadRequest("Missing data.");

            if (!_facadePurchaseOrder.deletePurchaseOrderById(purchaseOrder.id))
                return BadRequest("Purchase order can't be deleted.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("updateStatusByPurchaseOrderId")]
    public IActionResult updateStatusByPurchaseOrderId(entities.models.changeStatusModel changeStatus)
    {
        try
        {
            if(changeStatus == null || !new purchaseOrderFormHelper().isUpdateFormValid(changeStatus))
                return BadRequest("The status was not modified.");

            if (!_facadePurchaseOrder.updateStatusByPurchaseOrderId(changeStatus))
                return BadRequest("The status was not change.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpGet("getStatusCatalog")]
    public IActionResult getStatusCatalog()
    {
        try
        {
            return Ok(_facadePurchaseOrder.getStatusCatalog());
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("getPurchaseOrderById")]
    public IActionResult getPurchaseOrderById(entities.models.purchaseOrderModel purchaseOrder)
    {
        try
        {
            return Ok(_facadePurchaseOrder.getPurchaseOrderById(purchaseOrder.id));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpGet("getPurchaseOrderTracesByPurchaseOrderId")]
    public IActionResult getPurchaseOrderTracesByPurchaseOrderId(int id)
    {
        try
        {
            return Ok(_facadePurchaseOrder.getPurchaseOrderTracesByPurchaseOrderId(id));
        }
        catch (Exception e)
        {
            return BadRequest(new { isSuccess = false, message = e.Message });
        }
    }

    [HttpGet("getPurchaseOrderTraceById")]
    public IActionResult getPurchaseOrderTraceById(int id)
    {
        try
        {
            return Ok(_facadePurchaseOrder.getPurchaseOrderTraceById(id));
        }
        catch (Exception e)
        {
            return BadRequest(new { isSuccess = false, message = e.Message });
        }
    }
}