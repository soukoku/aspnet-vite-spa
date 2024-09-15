using Soukoku.AspNet.Mvc.ViteIntegration.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;

namespace AspnetMvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            // for prod the entire dist output should be placed in web app root.
            var rootPath = HostingEnvironment.MapPath("~/");
            var manifestPath = Path.GetFullPath(Path.Combine($"{rootPath}../vite-app/dist/.vite/manifest.json"));
            routes.MapViteSpaProxy(manifestPath);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
