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
public class employeeController : ControllerBase
{
    private IUserService _userService;
    private facadeEmployee _facadeEmployee;

    public employeeController(IUserService userService)
    {
        _userService = userService;
        _facadeEmployee = new facadeEmployee();
    }

    [HttpGet("getAllEmployeeCatalogs")]
    public IActionResult getAllEmployeeCatalogs()
    {
        try
        {
            return Ok(_facadeEmployee.getAllEmployeeCatalogs());
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
            return Ok(_facadeEmployee.getEmployees());
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("getEmployeeById")]
    public IActionResult getEmployeeById(entities.models.userModel user)
    {
        try
        {
            return Ok(_facadeEmployee.getEmployeeById(user.id));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("update")]
    public IActionResult update(entities.models.userModel user)
    {
        try
        {
            if(user == null)
                return BadRequest("The employee was not modified.");

            if (!_facadeEmployee.updateEmployee(user))
                return BadRequest("Employee not updated.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("add")]
    public IActionResult add(entities.models.userModel user)
    {
        try
        {
            if(user == null)
                return BadRequest("The employee was not created.");

            if (!_facadeEmployee.addEmployee(user))
                return BadRequest("Employee not created.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }
}