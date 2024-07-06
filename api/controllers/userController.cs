﻿namespace api.Controllers;

using Microsoft.AspNetCore.Mvc;
using api.authorization;
using api.services;
using business.facade;
using providerData.entitiesData;
using Newtonsoft.Json;

[ApiController]
[authorize]
[Route("[controller]")]
public class userController : ControllerBase
{
    private IUserService _userService;
    private facadeUser _facadeUser;

    public userController(IUserService userService)
    {
        _userService = userService;
        _facadeUser = new facadeUser();
    }

    [allowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult authenticate(authenticateRequest model)
    {
        try
        {
            var response = _userService.authenticate(model);

            if(response == null)
                return BadRequest(new { message = "Username or password is incorrect." });

            return Ok(response);
        }
        catch (Exception exception)
        {
            return BadRequest(exception);
        }
    }

    [HttpGet("getUsers")]
    public IActionResult getUsers()
    {
        try
        {
            return Ok(_facadeUser.getUsers());
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("updateUser")]
    public IActionResult updateUser(entities.models.userModel user)
    {
        try
        {
            if(user == null)
                return BadRequest("The user was not modified.");

            if (!_facadeUser.updateUser(user))
                return BadRequest("User not updated.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("addUser")]
    public IActionResult addUser(entities.models.userModel user)
    {
        try
        {
            if(user == null)
                return BadRequest("The user was not modified.");

            if (!_facadeUser.addUser(user))
                return BadRequest("User not mo.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [allowAnonymous]
    [HttpGet("getTest")]
    public IActionResult getTest()
    {
        try
        {
            return Ok("I'm OK!");
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }
}