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
using entities.enums;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Microsoft.DiaSymReader;

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

    [HttpGet("purchaseOrder/detail")]
    public async Task<IActionResult> detail(int id)
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
                var errorMessage = await responsePostUser.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePostUser.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }
            var responsePostUserAsJson = await responsePostUser.Content.ReadAsStringAsync();
            var resultUser = JsonConvert.DeserializeObject<entities.models.userModel>(responsePostUserAsJson);

            var purchaseOrder = new purchaseOrderModel { id = id };
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:purchaseOrder:getPurchaseOrderById"]}", new StringContent(JsonConvert.SerializeObject(purchaseOrder), Encoding.UTF8, "application/json"));
            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePostUser.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePostUser.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }
            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<entities.models.purchaseOrderModel>(responsePostAsJson);

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
                var errorMessage = await responsePostEnterprise.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePostEnterprise.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }
            var responsePostEnterpriseAsJson = await responsePostEnterprise.Content.ReadAsStringAsync();
            var resultEnterprises = JsonConvert.DeserializeObject<List<entities.models.enterpriseModel>>(responsePostEnterpriseAsJson);
            clientHttp.Dispose();

            ViewData["user.id"] = resultUser!.id;
            ViewData["user.email"] = resultUser!.email;
            ViewData["user.firstname"] = resultUser!.firstname;
            ViewData["user.lastname"] = resultUser!.lastname;
            ViewData["enterprise.location"] = $"{resultEnterprises!.First().city} {resultEnterprises!.First()!.state} {resultEnterprises!.First()!.country}";
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
            ViewData["quotation.project.projectName"] = result!.projectName;
            ViewData["purchaseOrder.code"] = result!.code;
            ViewData["purchaseOrder.payment.description"] = result!.payment!.description;
            ViewData["purchaseOrder.currency.description"] = result!.currency!.description;
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

    [HttpGet("purchaseOrder/getCatalogs")]
    public async Task<IActionResult> getCatalogs()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:purchaseOrder:getAllPurchaseOrderCatalogs"]}");
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
            var results = JsonConvert.DeserializeObject<IEnumerable<IEnumerable<entities.models.catalogModel>>>(responseGetAsJson);
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
                var errorMessage = await responseGet.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responseGet.ReasonPhrase : errorMessage;
                return Json(new
                {
                    isSuccess = false,
                    message = $"{message}"
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

    [HttpGet("purchaseOrder/getPurchaseOrderItemsByPurchaseOrderId")]
    public async Task<JsonResult> getPurchaseOrderItemsByPurchaseOrderId(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var purchaseOrder = new purchaseOrderModel { id = id };
            var responsePostPurchaseOrder = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:purchaseOrder:getPendingPurchaseOrderItemsByPurchaseOrderId"]}", new StringContent(JsonConvert.SerializeObject(purchaseOrder), Encoding.UTF8, "application/json"));
            var contentResponsePostPurchaseOrder = await responsePostPurchaseOrder.Content.ReadAsStringAsync();
            if(!responsePostPurchaseOrder.IsSuccessStatusCode)
            {
                var message = string.IsNullOrEmpty(contentResponsePostPurchaseOrder) ? responsePostPurchaseOrder.ReasonPhrase : contentResponsePostPurchaseOrder;
                return Json(new
                {
                    isSuccess = false,
                    message = "message"
                });
            }
            var results = JsonConvert.DeserializeObject<IEnumerable<entities.models.purchaseOrderItemsModel>>(contentResponsePostPurchaseOrder);
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
                var errorMessage = await responsePostUser.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePostUser.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
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
                var errorMessage = await responsePostEnterprise.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePostEnterprise.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
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
            if (purchaseOrder != null && purchaseOrder!.status != (int)statusType.ACTIVE)
                return Json(new
                {
                    isSuccess = false,
                    message = "Only purchase orders with an ACTIVE status can be deleted."
                });

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:purchaseOrder:delete"]}", new StringContent(JsonConvert.SerializeObject(purchaseOrder), Encoding.UTF8, "application/json"));

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

    [HttpPost("purchaseOrder/updateStatusByPurchaseOrderId")]
    public async Task<JsonResult> updateStatusByPurchaseOrderId([FromBody] changeStatusModel changeStatus)
    {
        try
        {
            if (!ModelState.IsValid || !purchaseOrderFormHelper.isUpdateFormValid(changeStatus))
                return Json(new
                { 
                    isSuccess = false,
                    message = "Invalid data."
                });

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            changeStatus.userId = userCookie.id;
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:purchaseOrder:updateStatusByPurchaseOrderId"]}", new StringContent(JsonConvert.SerializeObject(changeStatus), Encoding.UTF8, "application/json"));

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
                message = "Status updated successfully."
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

    [HttpGet("purchaseOrder/getStatusCatalog")]
    public async Task<IActionResult> getStatusCatalog()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:purchaseOrder:getStatusCatalog"]}");

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
            var results = JsonConvert.DeserializeObject<IEnumerable<IEnumerable<entities.models.catalogModel>>>(responseGetAsJson);
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

    [HttpGet("purchaseOrder/downloadPurchaseOrderByPurchaseOrderId")]
    public async Task<IActionResult> downloadPurchaseOrderByPurchaseOrderId(int id)
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
                var errorMessage = await responsePostUser.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePostUser.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }
            var responsePostUserAsJson = await responsePostUser.Content.ReadAsStringAsync();
            user = JsonConvert.DeserializeObject<userModel>(responsePostUserAsJson);

            var purchaseOrder = new purchaseOrderModel { id = id };
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:purchaseOrder:getPurchaseOrderById"]}", new StringContent(JsonConvert.SerializeObject(purchaseOrder), Encoding.UTF8, "application/json"));
            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePostUser.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePostUser.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }
            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            purchaseOrder = JsonConvert.DeserializeObject<purchaseOrderModel>(responsePostAsJson);

            var enterprises = new List<enterpriseModel>();
            var enterprise = new enterpriseModel
            {
                id = 1,
                defaultValues = new defaultValuesModel
                {
                    configType = configType.PURCHASE_ORDERS
                }
            };
            var responsePostEnterprise = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:enterprise:getEnterpriseFullInformationByIdAndConfigType"]}", new StringContent(JsonConvert.SerializeObject(enterprise), Encoding.UTF8, "application/json"));
            if(!responsePostEnterprise.IsSuccessStatusCode)
            {
                var errorMessage = await responsePostEnterprise.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePostEnterprise.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }
            var responsePostEnterpriseAsJson = await responsePostEnterprise.Content.ReadAsStringAsync();
            enterprises = JsonConvert.DeserializeObject<List<enterpriseModel>>(responsePostEnterpriseAsJson);
            clientHttp.Dispose();

            var pdfContent = pdfHelper.generatePurchaseOrderPdf(purchaseOrder!, enterprises!, user!);
            Response.Headers.Add("Content-Disposition", $"attachment; filename=PurchaseOrder_{id}.pdf");
            return File(pdfContent, "application/pdf");
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
                var errorMessage = await responsePostUser.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePostUser.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }
            var responsePostUserAsJson = await responsePostUser.Content.ReadAsStringAsync();
            var resultUser = JsonConvert.DeserializeObject<entities.models.userModel>(responsePostUserAsJson);

            var purchaseOrder = new purchaseOrderModel { id = id };
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:purchaseOrder:getPurchaseOrderById"]}", new StringContent(JsonConvert.SerializeObject(purchaseOrder), Encoding.UTF8, "application/json"));
            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePostUser.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePostUser.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }
            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<entities.models.purchaseOrderModel>(responsePostAsJson);

            if (result!.status != (int)statusType.ACTIVE)
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = "Only purchase orders with an ACTIVE status can be updated." });

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
                var errorMessage = await responsePostEnterprise.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePostEnterprise.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }
            var responsePostEnterpriseAsJson = await responsePostEnterprise.Content.ReadAsStringAsync();
            var resultEnterprises = JsonConvert.DeserializeObject<List<entities.models.enterpriseModel>>(responsePostEnterpriseAsJson);
            clientHttp.Dispose();

            ViewData["user.id"] = resultUser!.id;
            ViewData["user.email"] = resultUser!.email;
            ViewData["user.firstname"] = resultUser!.firstname;
            ViewData["user.lastname"] = resultUser!.lastname;
            ViewData["enterprise.location"] = $"{resultEnterprises!.First().city} {resultEnterprises!.First()!.state} {resultEnterprises!.First()!.country}";
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
            ViewData["purchaseOrder.payment.id"] = result!.payment!.id;
            ViewData["purchaseOrder.currency.id"] = result!.currency!.id;
            ViewData["purchaseOrder.generalNotes"] = result!.generalNotes;
            ViewData["purchaseOrder.subtotal"] = result!.subtotal;
            ViewData["purchaseOrder.taxRate"] = result!.taxRate;
            ViewData["purchaseOrder.taxAmount"] = result!.taxAmount;
            ViewData["purchaseOrder.totalAmount"] = result!.totalAmount;
            ViewData["purchaseOrder.items"] = result!.items;
            ViewData["purchaseOrder.project.id"] = result.projectId;
            ViewData["purchaseOrder.project.projectName"] = result.projectName;
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
                return Json(new
                {
                    isSuccess = false,
                    message = "Purchase order data is missing."
                });

            var purchaseOrder = JsonConvert.DeserializeObject<purchaseOrderModel>(purchaseOrderJson);
            if (purchaseOrder == null || !ModelState.IsValid || !purchaseOrderFormHelper.isUpdateFormValid(purchaseOrder))
                return Json(new
                {
                    isSuccess = false,
                    message = "Invalid data."
                });

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

    [HttpGet("purchaseOrder/getPurchaseOrderTracesByPurchaseOrderId")]
    public async Task<IActionResult> getPurchaseOrderTracesByPurchaseOrderId(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:purchaseOrder:getPurchaseOrderTracesByPurchaseOrderId"]}?id={id}");
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

    [HttpGet("purchaseOrder/getPurchaseOrderTraceById")]
    public async Task<IActionResult> getPurchaseOrderTraceById(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:purchaseOrder:getPurchaseOrderTraceById"]}?id={id}");
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
}