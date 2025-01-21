namespace app.controllers;

using providerData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using entities.models;
using authorization;
using System.Net.Http.Headers;
using common.configurations;
using System.Text;
using providerData.helpers;
using helpers;
using System.Text.RegularExpressions;

[authorization]
public class workOrderController : Controller
{
    private readonly ILogger<workOrderController> _logger;
    private readonly UserManager<applicationUser> _userManager;
    private readonly SignInManager<applicationUser> _signInManager;
    private readonly IHttpClientFactory _clientFactory;

    public workOrderController(ILogger<workOrderController> logger, UserManager<applicationUser> userManager, SignInManager<applicationUser> signInManager, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _clientFactory = clientFactory;
    }

    [HttpGet("workOrder/add")]
    public IActionResult add()
    {
        try
        {
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            ViewData["user.id"] = userCookie.id;
            return View();
        }
        catch (Exception e)
        {
            return RedirectToAction("errorWithParams", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpPost("workOrder/add")]
    public async Task<JsonResult> add([FromBody] workOrderModel workOrder)
    {
        try
        {
            if (!ModelState.IsValid || !workOrderFormHelper.isAddFormValid(workOrder))
                return Json(new
                { 
                    isSuccess = false,
                    message = "Invalid data."
                });

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync(configurationManager.appSettings["api:routes:workOrder:add"], new StringContent(JsonConvert.SerializeObject(workOrder), Encoding.UTF8, "application/json"));

            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePost.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePost.ReasonPhrase : errorMessage;
                return Json(new
                {
                    isSuccess = false,
                    message = $"{message}"
                });
            }
            clientHttp.Dispose();

            return Json(new
            {
                isSuccess = true,
                message = "Work Order added successfully."
            });
        }
        catch (Exception exception)
        {
            return Json(new
            {
                isSuccess = false,
                message = $"{exception.Message}"
            });
        }
    }

    [HttpPost("workOrder/delete")]
    public async Task<JsonResult> delete([FromBody] workOrderModel workOrder)
    {
        try
        {
            if (!ModelState.IsValid || !workOrderFormHelper.isUpdateFormValid(workOrder, true))
                return Json(new
                {
                    isSuccess = false,
                    message = "To delete a work order, they must first be cancelled."
                });

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:workOrder:delete"]}", new StringContent(JsonConvert.SerializeObject(workOrder), Encoding.UTF8, "application/json"));

            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePost.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePost.ReasonPhrase : errorMessage;
                return Json(new
                {
                    isSuccess = false,
                    message = $"{message}"
                });
            }
            clientHttp.Dispose();

            return Json(new
            { 
                isSuccess = true,
                message = "Work order deleted successfully."
            });
        }
        catch(Exception exception)
        {
            return Json(new
            {
                isSuccess = false,
                message = $"{exception.Message}"
            });
        }
    }

    [HttpGet("workOrder/getAll")]
    public async Task<JsonResult> getAll()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:workOrder:getAll"]}");
            if(!responseGet.IsSuccessStatusCode)
            {
                var errorMessage = await responseGet.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responseGet.ReasonPhrase : errorMessage;
                return Json(new
                {
                    isSuccess = false,
                    message = $"{message}"
                });
            }

            var responseGetAsJson = await responseGet.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<IEnumerable<entities.models.workOrderModel>>(responseGetAsJson);
            clientHttp.Dispose();

            return Json(new
            {
                isSuccess = true,
                message = "Ok.",
                results
            });
        }
        catch (Exception exception)
        {
            return Json(new
            {
                isSuccess = false,
                message = $"{exception.Message}"
            });
        }
    }

    [HttpGet("workOrder/list")]
    public IActionResult list()
    {
        try
        {
            return View();
        }
        catch (Exception e)
        {
            return RedirectToAction("errorWithParams", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpGet("workOrder/getCatalogs")]
    public async Task<IActionResult> getCatalogs()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:workOrder:getAllWorkOrderCatalogs"]}");

            if(!responseGet.IsSuccessStatusCode)
            {
                var errorMessage = await responseGet.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responseGet.ReasonPhrase : errorMessage;
                return Json(new
                {
                    isSuccess = false,
                    message = $"{message}"
                });
            }

            var responseGetAsJson = await responseGet.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<IEnumerable<IEnumerable<catalogModel>>>(responseGetAsJson);
            clientHttp.Dispose();

            return Json(new
            {
                isSuccess = true,
                message = "Ok.",
                results
            });
        }
        catch (Exception exception)
        {
            return Json(new
            {
                isSuccess = false,
                message = $"{exception.Message}"
            });
        }
    }

    [HttpGet("workOrder/update")]
    public async Task<IActionResult> update(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var workOrder = new workOrderModel { id = id };
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:workOrder:getFullWorkOrderById"]}", new StringContent(JsonConvert.SerializeObject(workOrder), Encoding.UTF8, "application/json"));

            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePost.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePost.ReasonPhrase : errorMessage;
                return RedirectToAction("errorWithParams", "error", new { errorCode = 0, errorMessage = message });
            }

            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<entities.models.workOrderModel>(responsePostAsJson);
            clientHttp.Dispose();

            ViewData["workOrder.id"] = result!.id;
            ViewData["user.id"] = result!.userId;
            ViewData["quotation.id"] = result!.quotationId;
            ViewData["quotation.code"] = result!.quotationCode;
            ViewData["client.name"] = result!.clientName;
            ViewData["project.name"] = result!.projectName;
            ViewData["workOrder.items"] = result!.items;
            ViewData["workOrder.creationDateAsString"] = result!.creationDateAsString;
            ViewData["payment.description"] = result!.quotationPaymentDescription;
            ViewData["currency.description"] = result!.quotationCurrencyDescription;
            ViewData["quotation.subtotal"] = result!.quotationSubtotal;
            ViewData["quotation.tax"] = result!.quotationTax;
            ViewData["quotation.total"] = result!.quotationTotal;
            ViewData["quotation.items"] = result!.quotationItems;
            ViewData["workOrder.priority"] = result!.priorityId;
            ViewData["workOrder.rfq"] = result!.rfq;
            ViewData["workOrder.status"] = result!.status;
            ViewData["workOrder.deliveryDateAsString"] = result!.deliveryDateAsString;

            return View();
        }
        catch(Exception e)
        {
            return RedirectToAction("errorWithParams", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpPost("workOrder/update")]
    public async Task<JsonResult> update([FromBody] workOrderModel workOrder)
    {
        try
        {
            if (!ModelState.IsValid || !workOrderFormHelper.isUpdateFormValid(workOrder))
                return Json(new
                {
                    isSuccess = false,
                    message = "Invalid data."
                });

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync(configurationManager.appSettings["api:routes:workOrder:update"], new StringContent(JsonConvert.SerializeObject(workOrder), Encoding.UTF8, "application/json"));

            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePost.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePost.ReasonPhrase : errorMessage;
                return Json(new
                {
                    isSuccess = false,
                    message = $"{message}"
                });
            }
            clientHttp.Dispose();

            return Json(new
            {
                isSuccess = true,
                message = "Work order updated successfully."
            });
        }
        catch (Exception exception)
        {
            return Json(new
            {
                isSuccess = false,
                message = $"{exception.Message}"
            });
        }
    }

    [HttpGet("workOrder/getWorkOrderTracesByWorkOrderId")]
    public async Task<IActionResult> getWorkOrderTracesByWorkOrderId(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:workOrder:getWorkOrderTracesByWorkOrderId"]}?id={id}");
            if (!responseGet.IsSuccessStatusCode)
            {
                var errorMessage = await responseGet.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responseGet.ReasonPhrase : errorMessage;
                return Json(new
                {
                    isSuccess = false,
                    message = $"{message}"
                });
            }

            var responseGetAsJson = await responseGet.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<IEnumerable<traceModel>>(responseGetAsJson);
            clientHttp.Dispose();

            return Json(new
            {
                isSuccess = true,
                message = "Ok.",
                results
            });
        }
        catch (Exception exception)
        {
            return Json(new
            {
                isSuccess = false,
                message = $"{exception.Message}"
            });
        }
    }

    [HttpGet("workOrder/getWorkOrderTraceById")]
    public async Task<IActionResult> getWorkOrderTraceById(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:workOrder:getWorkOrderTraceById"]}?id={id}");
            if (!responseGet.IsSuccessStatusCode)
            {
                var errorMessage = await responseGet.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responseGet.ReasonPhrase : errorMessage;
                return Json(new
                {
                    isSuccess = false,
                    message = $"{message}"
                });
            }

            var responseGetAsJson = await responseGet.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<traceModel>(responseGetAsJson);
            clientHttp.Dispose();

            return Json(new
            {
                isSuccess = true,
                message = "Ok.",
                results
            });
        }
        catch (Exception exception)
        {
            return Json(new
            {
                isSuccess = false,
                message = $"{exception.Message}"
            });
        }
    }

    [HttpGet("workOrder/detail")]
    public async Task<IActionResult> detail(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var workOrder = new workOrderModel { id = id };
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:workOrder:getFullWorkOrderById"]}", new StringContent(JsonConvert.SerializeObject(workOrder), Encoding.UTF8, "application/json"));

            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePost.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePost.ReasonPhrase : errorMessage;
                return RedirectToAction("errorWithParams", "error", new { errorCode = 0, errorMessage = message });
            }

            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<entities.models.workOrderModel>(responsePostAsJson);
            clientHttp.Dispose();

            ViewData["workOrder.id"] = result!.id;
            ViewData["workOrder.code"] = result!.code;
            ViewData["workOrder.rfq"] = result!.rfq;
            ViewData["workOrder.deliveryDateAsString"] = result!.deliveryDateAsString;
            ViewData["client.businessName"] = result!.clientName;
            ViewData["quotation.code"] = result!.quotationCode;
            ViewData["project.name"] = result!.projectName;
            ViewData["priority.description"] = result!.priorityDescription;
            ViewData["status.description"] = result!.statusName.ToUpper();
            ViewData["status.color"] = result!.statusColor;
            return View();
        }
        catch(Exception e)
        {
            return RedirectToAction("errorWithParams", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

}