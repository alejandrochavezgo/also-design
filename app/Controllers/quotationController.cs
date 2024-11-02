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
public class quotationController : Controller
{
    private readonly ILogger<quotationController> _logger;
    private readonly UserManager<applicationUser> _userManager;
    private readonly SignInManager<applicationUser> _signInManager;
    private readonly IHttpClientFactory _clientFactory;

    public quotationController(ILogger<quotationController> logger, UserManager<applicationUser> userManager, SignInManager<applicationUser> signInManager, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _clientFactory = clientFactory;
    }

    [HttpGet("quotation/list")]
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

    [HttpGet("quotation/getCatalogs")]
    public async Task<IActionResult> getCatalogs()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:quotation:getAllQuotationCatalogs"]}");
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

    [HttpGet("quotation/getAll")]
    public async Task<JsonResult> getAll()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:quotation:getAll"]}");

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
            var results = JsonConvert.DeserializeObject<IEnumerable<entities.models.quotationModel>>(responseGetAsJson);
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

    [HttpGet("quotation/add")]
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
                    configType = entities.enums.configType.QUOTATIONS
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
            ViewData["enterprise.defaultValues.items.notes"] = resultEnterprises!.First().defaultValues!.text;
            ViewData["enterprise.defaultValues.generalNotes"] = resultEnterprises!.Last().defaultValues!.text;

            return View();
        }
        catch (Exception e)
        {
            return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpPost("quotation/add")]
    public async Task<JsonResult> add(IFormCollection form)
    {
        try
        {
            var quotationJson = form["quotation"].FirstOrDefault();
            if (string.IsNullOrEmpty(quotationJson))
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "Quotation data is missing."
                });
            }

            var quotation = JsonConvert.DeserializeObject<quotationModel>(quotationJson);
            if (quotation == null || !ModelState.IsValid || !quotationFormHelper.isAddFormValid(quotation))
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "Invalid data."
                });
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images", "quotationItems");
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
                    quotation.items![index].imagePath = filePath;
                }
            }

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:quotation:add"]}", new StringContent(JsonConvert.SerializeObject(quotation), Encoding.UTF8, "application/json"));

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
                message = "Quotation added successfully."
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

    [HttpPost("quotation/delete")]
    public async Task<JsonResult> delete([FromBody] quotationModel quotation)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:quotation:delete"]}", new StringContent(JsonConvert.SerializeObject(quotation), Encoding.UTF8, "application/json"));

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
                message = "Quotation deleted successfully."
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

    [HttpGet("quotation/update")]
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

            var quotation = new quotationModel { id = id };
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:quotation:getQuotationById"]}", new StringContent(JsonConvert.SerializeObject(quotation), Encoding.UTF8, "application/json"));
            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePost.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePost.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }
            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<entities.models.quotationModel>(responsePostAsJson);
            clientHttp.Dispose();

            ViewData["user.id"] = resultUser!.id;
            ViewData["user.email"] = resultUser!.email;
            ViewData["user.firstname"] = resultUser!.firstname;
            ViewData["user.lastname"] = resultUser!.lastname;
            ViewData["employee.profession"] = resultUser.employee!.profession;
            ViewData["employee.jobPosition"] = resultUser.employee!.jobPosition;
            ViewData["employee.contactPhones"] = resultUser.employee!.contactPhones;
            ViewData["employee.mainContactPhone"] = result!.user!.employee!.mainContactPhone;
            ViewData["quotation.id"] = result!.id;
            ViewData["client.businessName"] = result!.client!.businessName;
            ViewData["client.id"] = result!.client!.id;
            ViewData["client.rfc"] = result!.client!.rfc;
            ViewData["client.address"] = result!.client!.address;
            ViewData["client.city"] = result!.client.city;
            ViewData["client.mainContactName"] = result!.client.mainContactName;
            ViewData["client.mainContactPhone"] = result!.client.mainContactPhone;
            ViewData["client.contactNames"] = result!.client.contactNames;
            ViewData["client.contactPhones"] = result!.client.contactPhones;
            ViewData["quotation.id"] = result!.id;
            ViewData["quotation.code"] = result!.code;
            ViewData["quotation.payment.id"] = result!.payment!.id;
            ViewData["quotation.currency.id"] = result!.currency!.id;
            ViewData["quotation.generalNotes"] = result!.generalNotes;
            ViewData["quotation.subtotal"] = result!.subtotal;
            ViewData["quotation.taxRate"] = result!.taxRate;
            ViewData["quotation.taxAmount"] = result!.taxAmount;
            ViewData["quotation.totalAmount"] = result!.totalAmount;
            foreach(var item in result.items!)
                item.imageString = !string.IsNullOrEmpty(item.imagePath) ? $"data:image/jpg;base64,{Convert.ToBase64String(System.IO.File.ReadAllBytes(item.imagePath!))}" : string.Empty;
            ViewData["quotation.items"] = result!.items;

            return View();
        }
        catch(Exception e)
        {
            return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpPost("quotation/update")]
    public async Task<JsonResult> update(IFormCollection form)
    {
        try
        {
            var quotationJson = form["quotation"].FirstOrDefault();
            if (string.IsNullOrEmpty(quotationJson))
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "Quotation data is missing."
                });
            }

            var quotation = JsonConvert.DeserializeObject<quotationModel>(quotationJson);
            if (quotation == null || !ModelState.IsValid || !quotationFormHelper.isUpdateFormValid(quotation))
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "Invalid data."
                });
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images", "quotationItems");
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
                    quotation.items![index].imagePath = filePath;
                }
            }

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:quotation:update"]}", new StringContent(JsonConvert.SerializeObject(quotation), Encoding.UTF8, "application/json"));

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
                message = "Quotation updated successfully."
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