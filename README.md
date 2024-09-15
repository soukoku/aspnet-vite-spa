This is a lib for integrating a Vite app in an aspnet or aspnet core app. 
Aspnet app provides the backend while a Vite SPA app provides the frontend.


# For aspnet core

Add the NuGet package `Soukoku.AspNetCore.ViteIntegration`, then
in the typical **Program.cs** file, register it with

```cs
var builder = WebApplication.CreateBuilder(args);

// depends on the actual dev server url
builder.Services.AddViteManifest("https://localhost:3000");

```


In a controller method that should serve the SPA view, use "VuePage"
and give it a `VitePageMvcModel`.

```cs
[HttpGet("{*anyPath}")]
public ActionResult Index(string? anyPath = null)
{
    var model = new VitePageMvcModel
    {
        Entry = "src/main.ts",
        UseAntiforgery = true, // if necessary
        PageData = new         // as needed
        {
            Property = "howdy"
        }
    };
    return View("VuePage", model);
}
```



Run the typical `npm run dev` in your vite app folder, then debug in aspnet as usual.


# For aspnet mvc (fx 4.6.2+) 

Add the NuGet package `Soukoku.AspNet.Mvc.ViteIntegration`, then
in the typical **RouteConfig.cs** file, register it with.

```cs
// assuming "dist" output content is placed in site root
var manifestPath = HostingEnvironment.MapPath("~/.vite/manifest.json");
// depends on the actual dev server url
routes.MapViteSpaProxy(manifestPath, "https://localhost:3000");

```

In a controller method that should serve the SPA view, use "VuePage"
and give it a `VitePageMvcModel`.

```cs
public ActionResult Index()
{
    var model = new VitePageMvcModel
    {
        Entry = "src/main.ts",
        UseAntiforgery = true, // if necessary
        PageData = new         // as needed
        {
            Property = "howdy"
        }
    };
    return View("VuePage", model);
}
```

For dev time, ensure these are in the **web.config** file

```xml
  <!--used by spa dev proxy to allow : in url-->
  <location path="@id">
    <system.web>
      <httpRuntime requestPathInvalidCharacters="&lt;,&gt;,*,%,&amp;,\,?" />
    </system.web>
  </location>
  
  <!--allow path with file extensions to be handled by spa dev proxy-->
  <system.webServer>
    <modules runAllManagedModulesForAllRequests="true" />
    <handlers>
      <remove name="ExtensionlessUrlHandler-Integrated-4.0" />
      <remove name="OPTIONSVerbHandler" />
      <remove name="TRACEVerbHandler" />
      <add name="ExtensionlessUrlHandler-Integrated-4.0" path="*." verb="*" type="System.Web.Handlers.TransferRequestHandler" preCondition="integratedMode,runtimeVersionv4.0" />
    </handlers>
  </system.webServer>
```


Run the typical `npm run dev` in your vite app folder, then debug in aspnet as usual.


# Examples

A more complete example is in the repo.
