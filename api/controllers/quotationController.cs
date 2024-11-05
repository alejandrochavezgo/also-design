namespace api.Controllers;

using Microsoft.AspNetCore.Mvc;
using api.authorization;
using api.services;
using business.facade;
using providerData.entitiesData;
using Newtonsoft.Json;
using common.helpers;
using entities.models;

[ApiController]
[authorize]
[Route("[controller]")]
public class quotationController : ControllerBase
{
    private IUserService _userService;
    private facadeQuotation _facadeQuotation;
    private providerData.entitiesData.userModel _user;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public quotationController(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _user = (providerData.entitiesData.userModel)_httpContextAccessor!.HttpContext?.Items["user"]!;
        _facadeQuotation = new facadeQuotation(new entities.models.userModel { id = _user.id });
    }

    [HttpGet("getAllQuotationCatalogs")]
    public IActionResult getAllQuotationCatalogs()
    {
        try
        {
            return Ok(_facadeQuotation.getAllQuotationCatalogs());
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
            return Ok(_facadeQuotation.getQuotations());
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("add")]
    public IActionResult add(entities.models.quotationModel quotation)
    {
        try
        {
            if(quotation == null || !new quotationFormHelper().isAddFormValid(quotation))
                return BadRequest("Missing data.");

            if (!_facadeQuotation.addQuotation(quotation))
                return BadRequest("Quotation not added.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("update")]
    public IActionResult update(entities.models.quotationModel quotation)
    {
        try
        {
            if(quotation == null || !new quotationFormHelper().isUpdateFormValid(quotation))
                return BadRequest("Missing data.");

            if (!_facadeQuotation.updateQuotation(quotation))
                return BadRequest("Quotation not updated.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("deleteQuotationById")]
    public IActionResult deleteQuotationById(entities.models.quotationModel quotation)
    {
        try
        {
            if(quotation == null)
                return BadRequest("Missing data.");

            if (!_facadeQuotation.deleteQuotationById(quotation.id))
                return BadRequest("Quotation can't be deleted.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("updateStatusByQuotationId")]
    public IActionResult updateStatusByQuotationId(entities.models.changeStatusModel changeStatus)
    {
        try
        {
            if(changeStatus == null || !new quotationFormHelper().isUpdateFormValid(changeStatus))
                return BadRequest("The status was not modified.");

            if (!_facadeQuotation.updateStatusByQuotationId(changeStatus))
                return BadRequest("The status was not change.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("getQuotationById")]
    public IActionResult getQuotationById(entities.models.quotationModel quotation)
    {
        try
        {
            return Ok(_facadeQuotation.getQuotationById(quotation.id));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }
}