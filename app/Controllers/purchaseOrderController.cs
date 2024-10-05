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
using helpers;
using System.Text.RegularExpressions;

[authorization]
public class purchaseOrderController : Controller
{
    private readonly ILogger<purchaseOrderController> _logger;
    private readonly UserManager<applicationUser> _userManager;
    private readonly SignInManager<applicationUser> _signInManager;
    private readonly IHttpClientFactory _clientFactory;

    public purchaseOrderController(ILogger<purchaseOrderController> logger, UserManager<applicationUser> userManager, SignInManager<applicationUser> signInManager, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _clientFactory = clientFactory;
    }

    [HttpGet("purchaseOrder/list")]
    public IActionResult list()
    {
        try
        {
            return View();
        }
        catch (Exception e)
        {
            return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpGet("purchaseOrder/getAll")]
    public async Task<JsonResult> getAll()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:purchaseOrder:getAll"]}");

            if(!responseGet.IsSuccessStatusCode)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = $"{responseGet.ReasonPhrase}"
                });
            }

            var responseGetAsJson = await responseGet.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<IEnumerable<entities.models.purchaseOrderModel>>(responseGetAsJson);
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

    [HttpGet("purchaseOrder/add")]
    public async Task<IActionResult> add()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var user = new userModel { id = userCookie.id };
            var responsePostUser = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:user:getUserById"]}", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
            if(!responsePostUser.IsSuccessStatusCode)
            {
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = responsePostUser.ReasonPhrase });
            }
            var responsePostUserAsJson = await responsePostUser.Content.ReadAsStringAsync();
            var resultUser = JsonConvert.DeserializeObject<entities.models.userModel>(responsePostUserAsJson);

            var enterprise = new enterpriseModel
            {
                id = 1,
                defaultValues = new defaultValuesModel
                {
                    configType = entities.enums.configType.PURCHASE_ORDERS
                }
            };
            var responsePostEnterprise = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:enterprise:getEnterpriseFullInformationByIdAndConfigType"]}", new StringContent(JsonConvert.SerializeObject(enterprise), Encoding.UTF8, "application/json"));
            if(!responsePostEnterprise.IsSuccessStatusCode)
            {
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = responsePostEnterprise.ReasonPhrase });
            }
            var responsePostEnterpriseAsJson = await responsePostEnterprise.Content.ReadAsStringAsync();
            var resultEnterprises = JsonConvert.DeserializeObject<List<entities.models.enterpriseModel>>(responsePostEnterpriseAsJson);
            clientHttp.Dispose();

            ViewData["user.id"] = resultUser!.id;
            ViewData["user.email"] = resultUser!.email;
            ViewData["user.firstname"] = resultUser!.firstname;
            ViewData["user.lastname"] = resultUser!.lastname;
            ViewData["employee.profession"] = resultUser.employee!.profession;
            ViewData["employee.jobPosition"] = resultUser.employee!.jobPosition;
            ViewData["employee.contactPhones"] = resultUser.employee!.contactPhones;
            ViewData["enterprise.location"] = $"{resultEnterprises!.First().city} {resultEnterprises!.First()!.state} {resultEnterprises!.First()!.country}";
            ViewData["enterprise.defaultValues.generalNotes"] = resultEnterprises!.First().defaultValues!.text;

            return View();
        }
        catch (Exception e)
        {
            return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpPost("purchaseOrder/add")]
    public async Task<JsonResult> add(IFormCollection form)
    {
        try
        {
            var purchaseOrderJson = form["purchaseOrder"].FirstOrDefault();
            if (string.IsNullOrEmpty(purchaseOrderJson))
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "Purchase order data is missing."
                });
            }

            var purchaseOrder = JsonConvert.DeserializeObject<purchaseOrderModel>(purchaseOrderJson);
            if (purchaseOrder == null || !ModelState.IsValid || !purchaseOrderFormHelper.isAddFormValid(purchaseOrder))
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "Invalid data."
                });
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images", "purchaseOrderItems");
            var files = form.Files;
            foreach (var file in files)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var match = Regex.Match(file.Name, @"image_(\d+)");
                if (match.Success)
                {
                    int index = int.Parse(match.Groups[1].Value);
                    purchaseOrder!.items![index].imagePath = filePath;
                }
            }

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:purchaseOrder:add"]}", new StringContent(JsonConvert.SerializeObject(purchaseOrder), Encoding.UTF8, "application/json"));

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
                message = "Purchase Order added successfully."
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

    [HttpPost("purchaseOrder/delete")]
    public async Task<JsonResult> delete([FromBody] purchaseOrderModel purchaseOrder)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:purchaseOrder:delete"]}", new StringContent(JsonConvert.SerializeObject(purchaseOrder), Encoding.UTF8, "application/json"));

            if(!responsePost.IsSuccessStatusCode)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = $"{responsePost.ReasonPhrase}"
                });
            }
            clientHttp.Dispose();

            return Json(new
            { 
                isSuccess = true,
                message = "Purchase order deleted successfully."
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

    [HttpGet("purchaseOrder/update")]
    public async Task<IActionResult> update(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var user = new userModel { id = userCookie.id };
            var responsePostUser = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:user:getUserById"]}", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
            if(!responsePostUser.IsSuccessStatusCode)
            {
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = responsePostUser.ReasonPhrase });
            }
            var responsePostUserAsJson = await responsePostUser.Content.ReadAsStringAsync();
            var resultUser = JsonConvert.DeserializeObject<entities.models.userModel>(responsePostUserAsJson);

            var purchaseOrder = new purchaseOrderModel { id = id };
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:purchaseOrder:getPurchaseOrderById"]}", new StringContent(JsonConvert.SerializeObject(purchaseOrder), Encoding.UTF8, "application/json"));
            if(!responsePost.IsSuccessStatusCode)
            {
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = responsePost.ReasonPhrase });
            }
            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<entities.models.purchaseOrderModel>(responsePostAsJson);
            clientHttp.Dispose();

            ViewData["user.id"] = resultUser!.id;
            ViewData["user.email"] = resultUser!.email;
            ViewData["user.firstname"] = resultUser!.firstname;
            ViewData["user.lastname"] = resultUser!.lastname;
            ViewData["employee.profession"] = resultUser.employee!.profession;
            ViewData["employee.jobPosition"] = resultUser.employee!.jobPosition;
            ViewData["employee.contactPhones"] = resultUser.employee!.contactPhones;
            ViewData["employee.mainContactPhone"] = result!.user!.employee!.mainContactPhone;
            ViewData["purchaseOrder.id"] = result!.id;
            ViewData["supplier.businessName"] = result!.supplier!.businessName;
            ViewData["supplier.id"] = result!.supplier!.id;
            ViewData["supplier.rfc"] = result!.supplier!.rfc;
            ViewData["supplier.address"] = result!.supplier!.address;
            ViewData["supplier.city"] = result!.supplier.city;
            ViewData["supplier.mainContactName"] = result!.supplier.mainContactName;
            ViewData["supplier.mainContactPhone"] = result!.supplier.mainContactPhone;
            ViewData["supplier.contactNames"] = result!.supplier.contactNames;
            ViewData["supplier.contactPhones"] = result!.supplier.contactPhones;
            ViewData["purchaseOrder.id"] = result!.id;
            ViewData["purchaseOrder.code"] = result!.code;
            ViewData["purchaseOrder.generalNotes"] = result!.generalNotes;
            ViewData["purchaseOrder.subtotal"] = result!.subtotal;
            ViewData["purchaseOrder.taxRate"] = result!.taxRate;
            ViewData["purchaseOrder.taxAmount"] = result!.taxAmount;
            ViewData["purchaseOrder.totalAmount"] = result!.totalAmount;
            ViewData["purchaseOrder.items"] = result!.items;
            foreach(var item in result.items!)
                if (!string.IsNullOrEmpty(item.imagePath))
                    item.imageString = $"data:image/jpg;base64,{Convert.ToBase64String(System.IO.File.ReadAllBytes(item.imagePath!))}";

            return View();
        }
        catch(Exception e)
        {
            return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpPost("purchaseOrder/update")]
    public async Task<JsonResult> update(IFormCollection form)
    {
        try
        {
            var purchaseOrderJson = form["purchaseOrder"].FirstOrDefault();
            if (string.IsNullOrEmpty(purchaseOrderJson))
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "Purchase order data is missing."
                });
            }

            var purchaseOrder = JsonConvert.DeserializeObject<purchaseOrderModel>(purchaseOrderJson);
            if (purchaseOrder == null || !ModelState.IsValid || !purchaseOrderFormHelper.isUpdateFormValid(purchaseOrder))
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "Invalid data."
                });
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images", "purchaseOrderItems");
            var files = form.Files;
            foreach (var file in files)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var match = Regex.Match(file.Name, @"image_(\d+)");
                if (match.Success)
                {
                    int index = int.Parse(match.Groups[1].Value);
                    purchaseOrder.items![index].imagePath = filePath;
                }
            }

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:purchaseOrder:update"]}", new StringContent(JsonConvert.SerializeObject(purchaseOrder), Encoding.UTF8, "application/json"));

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
                message = "Purchase order updated successfully."
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
}