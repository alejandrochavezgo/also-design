namespace api.Controllers;

using Microsoft.AspNetCore.Mvc;
using api.authorization;
using api.services;
using business.facade;
using common.helpers;
using providerData.entitiesData;
using Newtonsoft.Json;

[ApiController]
[authorize]
[Route("[controller]")]
public class employeeController : ControllerBase
{
    private IUserService _userService;
    private facadeEmployee _facadeEmployee;
    private providerData.entitiesData.userModel _user;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public employeeController(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _user = (providerData.entitiesData.userModel)_httpContextAccessor!.HttpContext?.Items["user"]!;
        _facadeEmployee = new facadeEmployee(new entities.models.userModel { id = _user.id });
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

    [HttpPost("delete")]
    public IActionResult delete(entities.models.employeeModel employee)
    {
        try
        {
            if(!new employeeFormHelper().isUpdateFormValid(employee, true))
                return BadRequest("Missing data.");

            if (!_facadeEmployee.deleteEmployeeById(employee.id))
                return BadRequest("Employee can't be deleted.");

            return Ok();
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
            if(user == null || !new employeeFormHelper().isUpdateFormValid(user))
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
            if(user == null || !new employeeFormHelper().isAddFormValid(user))
                return BadRequest("The employee was not created.");

            if (new facadeUser(user).getUserByEmail(user.email!.ToUpper().Trim()) != null)
                return BadRequest("This email already exist.");

            if (!_facadeEmployee.addEmployee(user))
                return BadRequest("Employee not created.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpGet("getEmployeeTracesByEmployeeId")]
    public IActionResult getEmployeeTracesByEmployeeId(int id)
    {
        try
        {
            return Ok(_facadeEmployee.getEmployeeTracesByEmployeeId(id));
        }
        catch (Exception e)
        {
            return BadRequest(new { isSuccess = false, message = e.Message });
        }
    }

    [HttpGet("getEmployeeTraceById")]
    public IActionResult getEmployeeTraceById(int id)
    {
        try
        {
            return Ok(_facadeEmployee.getEmployeeTraceById(id));
        }
        catch (Exception e)
        {
            return BadRequest(new { isSuccess = false, message = e.Message });
        }
    }
}