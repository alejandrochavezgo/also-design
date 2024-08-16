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
public class supplierController : ControllerBase
{
    private IUserService _userService;
    private facadeSupplier _facadeSupplier;

    public supplierController(IUserService userService)
    {
        _userService = userService;
        _facadeSupplier = new facadeSupplier();
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
            if(supplier == null)
                return BadRequest("The supplier was not modified.");

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
            if(supplier == null)
                return BadRequest("Missing data.");

            if (!_facadeSupplier.addSupplier(supplier))
                return BadRequest("Supplier not added.");

            return Ok();
        }
        catch(Exception e)
        {
            return BadRequest(JsonConvert.SerializeObject(e));
        }
    }
}