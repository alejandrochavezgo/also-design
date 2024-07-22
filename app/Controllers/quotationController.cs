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
                return Json(new
                {
                    isSuccess = false,
                    message = $"{responseGet.ReasonPhrase}"
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
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = responsePostUser.ReasonPhrase });
            }
            var responsePostUserAsJson = await responsePostUser.Content.ReadAsStringAsync();
            var resultUser = JsonConvert.DeserializeObject<entities.models.userModel>(responsePostUserAsJson);

            var enterprise = new enterpriseModel
            {
                id = 1,
                quotation = new quotationModel
                {
                    configType = entities.enums.configType.QUOTATIONS
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
            ViewData["enterprise.quotation.toolNotes"] = resultEnterprises!.First().quotation!.notes;
            ViewData["enterprise.quotation.generalNotes"] = resultEnterprises!.Last().quotation!.notes;

            return View();
        }
        catch (Exception e)
        {
            return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpPost("quotation/add")]
    public async Task<JsonResult> Add(IFormCollection form)
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
            if (quotation == null || !ModelState.IsValid)
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

            //save images into server... done.
            //save quotation with paths... pending.

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
}