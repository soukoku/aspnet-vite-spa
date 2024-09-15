using Soukoku.AspNet.Mvc.ViteIntegration.Controllers;
using System.Web.Routing;

namespace System.Web.Mvc
{
    /// <summary>
    /// Contains extension methods for adding Vite support to an 
    /// aspnet mvc app.
    /// </summary>
    public static class ViteExtensions
    {
        /// <summary>
        /// Adds a proxy handler to Vite dev server.
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="devTimeUrl">Url to vite dev server at dev time. If base is used, add it with slash like https://localhost/3000/somebase/.</param>
        /// <returns></returns>
        public static void MapViteSpaProxy(this RouteCollection routes, string? devTimeUrl = "https://localhost:3000")
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            DevSpaProxyController.SetDevTimeUrl(devTimeUrl);

            //AttributeRoutingMapper.MapAttributeRoutes(routes, new DefaultInlineConstraintResolver());
        }

    }
}
