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
public class projectController : ControllerBase
{
    private IUserService _userService;
    private facadeProject _facadeProject;
    private providerData.entitiesData.userModel _user;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public projectController(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _user = (providerData.entitiesData.userModel)_httpContextAccessor!.HttpContext?.Items["user"]!;
        _facadeProject = new facadeProject(new entities.models.userModel { id = _user.id });
    }

    [HttpGet("getAllProjectCatalogs")]
    public IActionResult getAllProjectCatalogs()
    {
        try
        {
            return Ok(_facadeProject.getAllProjectCatalogs());
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }


    [HttpPost("add")]
    public IActionResult add(entities.models.projectModel project)
    {
        try
        {
            if(project == null || !new projectFormHelper().isAddFormValid(project))
                return BadRequest("Missing data.");

            if (!_facadeProject.addProject(project))
                return BadRequest("Project not added.");

            return Ok();
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
            return Ok(_facadeProject.getProjects());
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("getProjectById")]
    public IActionResult getProjectById(entities.models.projectModel project)
    {
        try
        {
            return Ok(_facadeProject.getProjectById(project.id));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpGet("getProjectTracesByProjectId")]
    public IActionResult getProjectTracesByProjectId(int id)
    {
        try
        {
            return Ok(_facadeProject.getProjectTracesByProjectId(id));
        }
        catch (Exception e)
        {
            return BadRequest(new { isSuccess = false, message = e.Message });
        }
    }

    [HttpGet("getProjectTraceById")]
    public IActionResult getProjectTraceById(int id)
    {
        try
        {
            return Ok(_facadeProject.getProjectTraceById(id));
        }
        catch (Exception e)
        {
            return BadRequest(new { isSuccess = false, message = e.Message });
        }
    }

    [HttpPost("update")]
    public IActionResult update(entities.models.projectModel project)
    {
        try
        {
            if(project == null || !new projectFormHelper().isUpdateFormValid(project))
                return BadRequest("The Project was not modified.");

            if (!_facadeProject.updateProject(project))
                return BadRequest("Project not modified.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("delete")]
    public IActionResult delete(entities.models.projectModel project)
    {
        try
        {
            if(!new projectFormHelper().isUpdateFormValid(project, true))
                return BadRequest("Missing data.");

            if (!_facadeProject.deleteProjectById(project.id))
                return BadRequest("Project can't be deleted.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("getProjectsByTerm")]
    public IActionResult getProjectsByTerm(entities.models.projectModel project)
    {
        try
        {
            return Ok(_facadeProject.getProjectsByTerm(project.name!));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }
}