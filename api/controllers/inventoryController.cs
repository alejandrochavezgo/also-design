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
    public IActionResult getItemByTerm(entities.models.inventoryModel inventory)
    {
        try
        {
            return Ok(_facadeInventory.getItemByTerm(inventory.description!));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }
}