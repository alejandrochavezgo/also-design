namespace app.Controllers;

using Microsoft.AspNetCore.Mvc;

public class dashboardController : Controller
{
    public IActionResult index()
    {
        

        return View();
    }
}