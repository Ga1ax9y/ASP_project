using Microsoft.AspNetCore.Mvc;

namespace WEB_253505_Stanishewski.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
