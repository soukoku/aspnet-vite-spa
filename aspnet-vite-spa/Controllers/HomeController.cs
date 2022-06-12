using Microsoft.AspNetCore.Mvc;
using ViteIntegration;

namespace aspnet_vite_spa.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new VitePageMvcModel
            {
                Entry = "src/pages/home-index.ts"
            };
            return View("VuePage", model);
        }


        public IActionResult Another()
        {
            var model = new VitePageMvcModel
            {
                Entry = "src/pages/home-another.ts"
            };
            return View("VuePage", model);
        }
    }
}
