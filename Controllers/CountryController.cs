using Microsoft.AspNetCore.Mvc;
using StoreProject.Models;

namespace StoreProject.Controllers
{
    public class CountryController : Controller
    {
        public IActionResult Index()
        {
            var model = new CountryViewModel();
            //model.Country = "ca";
            return View(model);
        }
    }
}
