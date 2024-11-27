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
using providerData.helpers;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Razor.Language;

[authorization]
public class inventoryController : Controller
{
    private readonly ILogger<inventoryController> _logger;
    private readonly UserManager<applicationUser> _userManager;
    private readonly SignInManager<applicationUser> _signInManager;
    private readonly IHttpClientFactory _clientFactory;

    public inventoryController(ILogger<inventoryController> logger, UserManager<applicationUser> userManager, SignInManager<applicationUser> signInManager, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _clientFactory = clientFactory;
    }

    [HttpGet("inventory/list")]
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

    [HttpGet("inventory/getcCatalogByName")]
    public async Task<JsonResult> getcCatalogByName(string name)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var catalog = new catalogModel { description = name.ToUpper() };
            var responsePostCatalog = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:inventory:getCatalogByName"]}", new StringContent(JsonConvert.SerializeObject(catalog), Encoding.UTF8, "application/json"));
            var contentResponsePostCatalog = await responsePostCatalog.Content.ReadAsStringAsync();

            if(!responsePostCatalog.IsSuccessStatusCode)
            {
                var message = string.IsNullOrEmpty(contentResponsePostCatalog) ? responsePostCatalog.ReasonPhrase : contentResponsePostCatalog;
                return Json(new
                {
                    isSuccess = false,
                    message = "message"
                });
            }
            var results = JsonConvert.DeserializeObject<IEnumerable<entities.models.catalogModel>>(contentResponsePostCatalog);
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

    [HttpGet("inventory/getCatalogs")]
    public async Task<IActionResult> getCatalogs()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:inventory:getAllInventoryCatalogs"]}");

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

    [HttpGet("inventory/getAll")]
    public async Task<IActionResult> getAll()
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responseGet = await clientHttp.GetAsync($"{configurationManager.appSettings["api:routes:inventory:getAll"]}");

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
            var results = JsonConvert.DeserializeObject<IEnumerable<inventoryListModel>>(responseGetAsJson);
            clientHttp.Dispose();
            foreach (var item in results!)
                item.itemImagePath = $"data:image/jpg;base64,{Convert.ToBase64String(System.IO.File.ReadAllBytes(item.itemImagePath!))}";

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

    [HttpGet("inventory/getInventoryMovementsByPurchaseOrderIdAndInventoryItemId")]
    public async Task<IActionResult> getInventoryMovementsByPurchaseOrderIdAndInventoryItemId(int purchaseOrderId, int inventoryItemId)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var url = $"{configurationManager.appSettings["api:routes:inventory:getInventoryMovementsByPurchaseOrderIdAndInventoryItemId"]}?purchaseOrderId={purchaseOrderId}&inventoryItemId={inventoryItemId}";
            var responseGet = await clientHttp.GetAsync(url);
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
            var results = JsonConvert.DeserializeObject<IEnumerable<inventoryMovementModel>>(responseGetAsJson);
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

    [HttpGet("inventory/getItemByTerm")]
    public async Task<IActionResult> getItemByTerm(string description)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var inventoryItem = new inventoryListModel { itemName = description };
            var responseGet = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:inventory:getItemByTerm"]}", new StringContent(JsonConvert.SerializeObject(inventoryItem), Encoding.UTF8, "application/json"));

            if(!responseGet.IsSuccessStatusCode)
            {
                var errorMessage = await responseGet.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responseGet.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = responseGet.ReasonPhrase });
            }

            var responseGetAsJson = await responseGet.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<List<entities.models.inventoryListModel>>(responseGetAsJson);
            clientHttp.Dispose();

            return Json(results);
        }
        catch (Exception e)
        {
            return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpGet("inventory/add")]
    public IActionResult add()
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

    [HttpPost("inventory/add")]
    public async Task<JsonResult> add(IFormCollection form)
    {
        try
        {
            var inventoryItemJson = form["inventoryItem"].FirstOrDefault();
            if (string.IsNullOrEmpty(inventoryItemJson))
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "Inventory item data is missing."
                });
            }

            var inventoryItem = JsonConvert.DeserializeObject<inventoryItemModel>(inventoryItemJson);
            if (inventoryItem == null || !ModelState.IsValid || !inventoryItemFormHelper.isAddFormValid(inventoryItem))
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "Invalid data."
                });
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images", "inventoryItems");
            var files = form.Files;

            foreach (var file in files)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(stream);

                switch (file.Name)
                {
                    case "itemImage":
                        inventoryItem.itemImagePath = filePath;
                        break;
                    case "bluePrints":
                        inventoryItem.bluePrintsPath = filePath;
                        break;
                    case "technicalSpecifications":
                        inventoryItem.technicalSpecificationsPath = filePath;
                        break;
                    default:
                        break;
                }
            }

            if(files.FirstOrDefault(x => x.Name.Equals("itemImage")) == null)
                inventoryItem.itemImagePath = Path.Combine(folderPath, $"{configurationManager.appSettings["configurations:defaults:itemImageFileName"]}");

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:inventory:add"]}", new StringContent(JsonConvert.SerializeObject(inventoryItem), Encoding.UTF8, "application/json"));

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
                message = "Inventory item added successfully."
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

    [HttpGet("inventory/update")]
    public async Task<IActionResult> update(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var inventoryItem = new inventoryItemModel { id = id };
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:inventory:getItemInventoryById"]}", new StringContent(JsonConvert.SerializeObject(inventoryItem), Encoding.UTF8, "application/json"));

            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePost.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePost.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }

            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<entities.models.inventoryItemModel>(responsePostAsJson);
            clientHttp.Dispose();

            ViewData["id"] = result!.id;
            ViewData["itemCode"] = result!.itemCode;
            ViewData["itemName"] = result!.itemName;
            ViewData["status"] = result!.status;
            ViewData["description"] = result!.description;
            ViewData["material"] = result!.material;
            ViewData["finishType"] = result!.finishType;
            ViewData["classificationType"] = result!.classificationType;
            ViewData["diameter"] = result!.diameter;
            ViewData["unitDiameter"] = result!.unitDiameter;
            ViewData["length"] = result!.length;
            ViewData["unitLength"] = result!.unitLength;
            ViewData["weight"] = result!.weight;
            ViewData["unitWeight"] = result!.unitWeight;
            ViewData["tolerance"] = result!.tolerance;
            ViewData["unitTolerance"] = result!.unitTolerance;
            ViewData["warehouseLocation"] = result!.warehouseLocation;
            ViewData["reorderQty"] = result!.reorderQty;
            ViewData["notes"] = result!.notes;
            ViewData["itemImageString"] = $"data:image/jpg;base64,{Convert.ToBase64String(System.IO.File.ReadAllBytes(result.itemImagePath!))}";
            ViewData["hasBluePrints"] = !string.IsNullOrEmpty(result.bluePrintsPath);
            ViewData["hasTechnicalSpecifications"] = !string.IsNullOrEmpty(result.technicalSpecificationsPath);
            ViewData["creationDate"] = result!.creationDate;
            ViewData["lastRestockDate"] = result!.lastRestockDate;
            ViewData["modificationDate"] = result!.modificationDate;

            return View();
        }
        catch(Exception e)
        {
            return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpGet("inventory/detail")]
    public async Task<IActionResult> detail(int id)
    {
        try
        {
            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var inventoryItem = new inventoryItemModel { id = id };
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:inventory:getItemInventoryById"]}", new StringContent(JsonConvert.SerializeObject(inventoryItem), Encoding.UTF8, "application/json"));

            if(!responsePost.IsSuccessStatusCode)
            {
                var errorMessage = await responsePost.Content.ReadAsStringAsync();
                var message = string.IsNullOrEmpty(errorMessage) ? responsePost.ReasonPhrase : errorMessage;
                return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = message });
            }

            var responsePostAsJson = await responsePost.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<entities.models.inventoryItemModel>(responsePostAsJson);
            clientHttp.Dispose();

            ViewData["id"] = result!.id;
            ViewData["itemCode"] = result!.itemCode;
            ViewData["itemName"] = result!.itemName;
            ViewData["statusName"] = result!.statusName;
            ViewData["statusColor"] = result!.statusColor;
            ViewData["description"] = result!.description;
            ViewData["material"] = result!.materialDescription;
            ViewData["finishType"] = result!.finishTypeDescription;
            ViewData["classificationTypeDescription"] = result!.classificationTypeDescription;
            ViewData["diameter"] = result!.diameter;
            ViewData["unitDiameter"] = result!.unitDiameterDescription;
            ViewData["length"] = result!.length;
            ViewData["unitLength"] = result!.unitLengthDescription;
            ViewData["weight"] = result!.weight;
            ViewData["unitWeight"] = result!.unitWeightDescription;
            ViewData["tolerance"] = result!.tolerance;
            ViewData["unitTolerance"] = result!.unitToleranceDescription;
            ViewData["warehouseLocation"] = result!.warehouseLocationDescription;
            ViewData["reorderQty"] = result!.reorderQty;
            ViewData["notes"] = result!.notes;
            ViewData["itemImageString"] = $"data:image/jpg;base64,{Convert.ToBase64String(System.IO.File.ReadAllBytes(result.itemImagePath!))}";
            ViewData["hasBluePrints"] = !string.IsNullOrEmpty(result.bluePrintsPath);
            ViewData["hasTechnicalSpecifications"] = !string.IsNullOrEmpty(result.technicalSpecificationsPath);
            ViewData["creationDate"] = result!.creationDate;
            ViewData["lastRestockDate"] = result!.lastRestockDate;
            ViewData["modificationDate"] = result!.modificationDate;
            return View();
        }
        catch(Exception e)
        {
            return RedirectToAction("error", "error", new { errorCode = 0, errorMessage = e.Message });
        }
    }

    [HttpPost("inventory/update")]
    public async Task<JsonResult> update(IFormCollection form)
    {
        try
        {
            var inventoryItemJson = form["inventoryItem"].FirstOrDefault();
            if (string.IsNullOrEmpty(inventoryItemJson))
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "Inventory item data is missing."
                });
            }

            var inventoryItem = JsonConvert.DeserializeObject<inventoryItemModel>(inventoryItemJson);
            if (inventoryItem == null || !ModelState.IsValid || !inventoryItemFormHelper.isUpdateFormValid(inventoryItem))
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "Invalid data."
                });
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images", "inventoryItems");
            var files = form.Files;
            inventoryItem.itemDefaultImagePath = Path.Combine(folderPath, $"{configurationManager.appSettings["configurations:defaults:itemImageFileName"]}");

            foreach (var file in files)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(stream);

                switch (file.Name)
                {
                    case "itemImage":
                        inventoryItem.itemImagePath = filePath;
                        break;
                    case "bluePrints":
                        inventoryItem.bluePrintsPath = filePath;
                        break;
                    case "technicalSpecifications":
                        inventoryItem.technicalSpecificationsPath = filePath;
                        break;
                    default:
                        break;
                }
            }

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:inventory:update"]}", new StringContent(JsonConvert.SerializeObject(inventoryItem), Encoding.UTF8, "application/json"));

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
                message = "Inventory item updated successfully."
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

    [HttpPost("inventory/delete")]
    public async Task<JsonResult> delete([FromBody] inventoryItemModel inventoryItem)
    {
        try
        {
            if (!ModelState.IsValid || !inventoryItemFormHelper.isUpdateFormValid(inventoryItem, true))
                return Json(new
                {
                    isSuccess = false,
                    message = "To delete an inventory item, it must be 0.00."
                });

            var clientHttp = _clientFactory.CreateClient();
            var userCookie = JsonConvert.DeserializeObject<providerData.entitiesData.userModel>(Request.HttpContext.Request.Cookies["userCookie"]!);
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{userCookie!.token}");
            var responsePost = await clientHttp.PostAsync($"{configurationManager.appSettings["api:routes:inventory:delete"]}", new StringContent(JsonConvert.SerializeObject(inventoryItem), Encoding.UTF8, "application/json"));

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
                message = "Inventory item deleted successfully."
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
}