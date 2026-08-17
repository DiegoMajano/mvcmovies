using Microsoft.AspNetCore.Mvc;

namespace MVCPeliculas.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Privacy() => View();
}
