using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

public class TestController : Controller
{
    public ActionResult Index()
    {
        return View();
    }
}