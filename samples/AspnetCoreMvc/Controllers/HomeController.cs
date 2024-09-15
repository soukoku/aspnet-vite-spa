using Microsoft.AspNetCore.Mvc;
using Soukoku.AspNetCore.ViteIntegration;

namespace aspnet_vite_spa.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new VitePageMvcModel
            {
                Entry = "src/main.ts",
                UseAntiforgery = true,
                PageData = new
                {
                    Property = "howdy"
                }
            };
            return View("VuePage", model);
        }


        //public IActionResult Another()
        //{
        //  var model = new VitePageMvcModel
        //  {
        //    Entry = "src/pages/home-another.ts"
        //  };
        //  return View("VuePage", model);
        //}
    }
}
