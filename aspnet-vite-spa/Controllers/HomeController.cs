using Microsoft.AspNetCore.Mvc;

namespace aspnet_vite_spa.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new SpaPageModel
            {
                Entry = "src/pages/home-index.ts"
            };
            return View("SpaPage", model);
        }


        public IActionResult Another()
        {
            var model = new SpaPageModel
            {
                Entry = "src/pages/home-another.ts"
            };
            return View("SpaPage", model);
        }
    }
}
