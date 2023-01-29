using Microsoft.AspNetCore.Mvc;
namespace fika_yazılım_test.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        return View();
    }
}

