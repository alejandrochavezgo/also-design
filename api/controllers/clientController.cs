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
public class clientController : ControllerBase
{
    private IUserService _userService;
    private facadeClient _facadeClient;

    public clientController(IUserService userService)
    {
        _userService = userService;
        _facadeClient = new facadeClient();
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
            if(client == null)
                return BadRequest("The client was not modified.");

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
            if(client == null)
                return BadRequest("Missing data.");

            if (!_facadeClient.addClient(client))
                return BadRequest("Client not added.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }
}