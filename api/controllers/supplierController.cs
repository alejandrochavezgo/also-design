namespace api.Controllers;

using Microsoft.AspNetCore.Mvc;
using api.authorization;
using api.services;
using business.facade;
using providerData.entitiesData;
using Newtonsoft.Json;
using common.helpers;

[ApiController]
[authorize]
[Route("[controller]")]
public class supplierController : ControllerBase
{
    private IUserService _userService;
    private facadeSupplier _facadeSupplier;

    public supplierController(IUserService userService)
    {
        _userService = userService;
        _facadeSupplier = new facadeSupplier();
    }

    [HttpGet("getAllSupplierCatalogs")]
    public IActionResult getAllSupplierCatalogs()
    {
        try
        {
            return Ok(_facadeSupplier.getAllSupplierCatalogs());
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
            return Ok(_facadeSupplier.getSuppliers());
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("getSupplierById")]
    public IActionResult getSupplierById(entities.models.supplierModel supplier)
    {
        try
        {
            return Ok(_facadeSupplier.getSupplierById(supplier.id));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("getSuppliersByTerm")]
    public IActionResult getSuppliersByTerm(entities.models.supplierModel supplier)
    {
        try
        {
            return Ok(_facadeSupplier.getSuppliersByTerm(supplier.businessName!));
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("update")]
    public IActionResult update(entities.models.supplierModel supplier)
    {
        try
        {
            if(supplier == null || !new supplierFormHelper().isUpdateFormValid(supplier))
                return BadRequest("The supplier was not modified.");

            if(_facadeSupplier.existSupplierByBusinessNameAndRfcAndId(supplier.businessName, supplier.rfc, supplier.id))
                return BadRequest("This Business Name and RFC already exist.");

            if (!_facadeSupplier.updateSupplier(supplier))
                return BadRequest("Supplier not modified.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("add")]
    public IActionResult add(entities.models.supplierModel supplier)
    {
        try
        {
            if(supplier == null || !new supplierFormHelper().isAddFormValid(supplier))
                return BadRequest("Missing data.");

            if(_facadeSupplier.existSupplierByBusinessNameAndRfc(supplier.businessName, supplier.rfc))
                return BadRequest("This Business Name and RFC already exist.");

            if (!_facadeSupplier.addSupplier(supplier))
                return BadRequest("Supplier not added.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }

    [HttpPost("delete")]
    public IActionResult delete(entities.models.supplierModel supplier)
    {
        try
        {
            if(!supplierFormHelper.isUpdateFormValid(supplier, true))
                return BadRequest("Missing data.");

            if (!_facadeSupplier.deleteSupplierById(supplier.id))
                return BadRequest("Purchase order can't be deleted.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }
}