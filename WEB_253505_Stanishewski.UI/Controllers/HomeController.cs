using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253505_Stanishewski.UI.Models;

namespace WEB_253505_Stanishewski.UI.Controllers
{
    public class HomeController : Controller
    {
        [ViewData]
        public string Name { get; set; }
        public IActionResult Index()
        {
            var items = new List<ListDemo>
            {
                new ListDemo { Id=1, Name="Item 1"},
                new ListDemo { Id=2, Name="Item 2"},
                new ListDemo { Id=3, Name="Item 3"}
            };
            var selectList = new SelectList(items, "Id", "Name");
            Name = "Лабораторная работа 2";
            return View(selectList); ;
        }

    }
}
