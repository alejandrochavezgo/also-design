namespace api.controllers;

using Microsoft.AspNetCore.Mvc;
using api.authorization;
using api.services;
using business.facade;
using providerData.entitiesData;
using Newtonsoft.Json;
using common.helpers;
using entities.enums;
using entities.models;

[ApiController]
[authorize]
[Route("[controller]")]
public class clientController : ControllerBase
{
    private IUserService _userService;
    private facadeClient _facadeClient;
    private providerData.entitiesData.userModel _user;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public clientController(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _user = (providerData.entitiesData.userModel)_httpContextAccessor!.HttpContext?.Items["user"]!;
        _facadeClient = new facadeClient(new entities.models.userModel { id = _user.id });
    }

    [HttpGet("getAllClientCatalogs")]
    public IActionResult getAllClientCatalogs()
    {
        try
        {
            return Ok(_facadeClient.getAllClientCatalogs());
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
            return Ok(_facadeClient.getClients());
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("getClientById")]
    public IActionResult getClientById(entities.models.clientModel client)
    {
        try
        {
            return Ok(_facadeClient.getClientById(client.id));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("getClientsByTerm")]
    public IActionResult getClientsByTerm(entities.models.clientModel client)
    {
        try
        {
            return Ok(_facadeClient.getClientsByTerm(client.businessName!));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("update")]
    public IActionResult update(entities.models.clientModel client)
    {
        try
        {
            if(client == null || !new clientFormHelper().isUpdateFormValid(client))
                return BadRequest("The Client was not modified.");

            if(_facadeClient.existClientByBusinessNameAndRfcAndId(client.businessName, client.rfc, client.id))
                return BadRequest("This Business Name and RFC already exist.");

            if (!_facadeClient.updateClient(client))
                return BadRequest("Client not modified.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("add")]
    public IActionResult add(entities.models.clientModel client)
    {
        try
        {
            if(client == null || !new clientFormHelper().isAddFormValid(client))
                return BadRequest("Missing data.");

            if(_facadeClient.existClientByBusinessNameAndRfc(client.businessName, client.rfc))
                return BadRequest("This Business Name and RFC already exist.");

            if (!_facadeClient.addClient(client))
                return BadRequest("Client not added.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("delete")]
    public IActionResult delete(entities.models.clientModel client)
    {
        try
        {
            if(!clientFormHelper.isUpdateFormValid(client, true))
                return BadRequest("Missing data.");

            if (!_facadeClient.deleteClientById(client.id))
                return BadRequest("Purchase order can't be deleted.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }
}