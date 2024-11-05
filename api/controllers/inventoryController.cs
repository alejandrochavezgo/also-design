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
public class inventoryController : ControllerBase
{
    private IUserService _userService;
    private facadeInventory _facadeInventory;

    public inventoryController(IUserService userService)
    {
        _userService = userService;
        _facadeInventory = new facadeInventory();
    }

    [HttpPost("getItemByTerm")]
    public IActionResult getItemByTerm(entities.models.inventoryListModel inventoryItem)
    {
        try
        {
            return Ok(_facadeInventory.getItemByTerm(inventoryItem.itemName!));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpGet("getAllInventoryCatalogs")]
    public IActionResult getAllInventoryCatalogs()
    {
        try
        {
            return Ok(_facadeInventory.getAllInventoryCatalogs());
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
            return Ok(_facadeInventory.getInventoryItems());
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpGet("getInventoryMovementsByPurchaseOrderIdAndInventoryItemId")]
    public IActionResult getInventoryMovementsByPurchaseOrderIdAndInventoryItemId(int inventoryItemId)
    {
        try
        {
            return Ok(_facadeInventory.getInventoryMovementsByPurchaseOrderIdAndInventoryItemId(inventoryItemId));
        }
        catch (Exception e)
        {
            return BadRequest(new { isSuccess = false, message = e.Message });
        }
    }

    [HttpPost("add")]
    public IActionResult add(entities.models.inventoryItemModel inventoryItem)
    {
        try
        {
            if(inventoryItem == null || !new inventoryItemFormHelper().isAddFormValid(inventoryItem))
                return BadRequest("Missing data.");

            if (!_facadeInventory.addInventoryItem(inventoryItem))
                return BadRequest("Inventory item not added.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("getItemInventoryById")]
    public IActionResult getItemInventoryById(entities.models.inventoryItemModel inventoryItem)
    {
        try
        {
            return Ok(_facadeInventory.getItemInventoryById(inventoryItem.id));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("getCatalogByName")]
    public IActionResult getCatalogByName(entities.models.catalogModel catalog)
    {
        try
        {
            if (catalog == null || string.IsNullOrEmpty(catalog.description))
                return BadRequest("Missing data.");

            return Ok(_facadeInventory.getCatalogByName(catalog.description));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("update")]
    public IActionResult update(entities.models.inventoryItemModel inventoryItem)
    {
        try
        {
            if(inventoryItem == null || !new inventoryItemFormHelper().isUpdateFormValid(inventoryItem))
                return BadRequest("Missing data.");

            if (!_facadeInventory.updateInventoryItem(inventoryItem))
                return BadRequest("Quotation not updated.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("delete")]
    public IActionResult delete(entities.models.inventoryItemModel inventoryItem)
    {
        try
        {
            if(!inventoryItemFormHelper.isUpdateFormValid(inventoryItem, true))
                return BadRequest("Missing data.");

            if (!_facadeInventory.deleteItemInventoryById(inventoryItem.id))
                return BadRequest("Invetory item can't be deleted.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }
}