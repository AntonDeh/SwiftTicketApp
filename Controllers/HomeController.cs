using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SwiftTicketApp.Models;
using SwiftTicketApp.ViewModels;

namespace SwiftTicketApp.Controllers;

public class HomeController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public IActionResult Index()
    {
        var model = new HomeViewModel
        {
            IsUserAuthenticated = User.Identity?.IsAuthenticated == true

        };
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
