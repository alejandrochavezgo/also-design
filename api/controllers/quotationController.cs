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

    public quotationController(IUserService userService)
    {
        _userService = userService;
        _facadeQuotation = new facadeQuotation();
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