using Soukoku.AspNet.Mvc.ViteIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AspnetMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
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
            //return Content("Hello world");
        }


        public async Task<ActionResult> Test()
        {
            var resp = await new HttpClient().GetAsync("https://localhost:3000/node_modules/.vite/deps/vue.js?v=2782bb0b");

            var resp2 = await new HttpClient { BaseAddress = new Uri("https://localhost:3000") }.GetAsync("node_modules/.vite/deps/vue.js?v=2782bb0b");

            return Content(resp.StatusCode.ToString());
        }
    }
}