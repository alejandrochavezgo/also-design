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
public class purchaseOrderController : ControllerBase
{
    private IUserService _userService;
    private facadePurchaseOrder _facadePurchaseOrder;

    public purchaseOrderController(IUserService userService)
    {
        _userService = userService;
        _facadePurchaseOrder = new facadePurchaseOrder();
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

    [HttpPost("getPurchaseOrderItemsByPurchaseOrderId")]
    public IActionResult getPurchaseOrderItemsByPurchaseOrderId(entities.models.purchaseOrderModel purchaseOrder)
    {
        try
        {
            if(purchaseOrder == null || purchaseOrder.id <= 0)
                return BadRequest("Missing data.");

            return Ok(_facadePurchaseOrder.getPurchaseOrderItemsByPurchaseOrderId(purchaseOrder.id));
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
            if(purchaseOrder == null)
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
}