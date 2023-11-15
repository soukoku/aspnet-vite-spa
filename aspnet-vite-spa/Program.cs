using Soukoku.AspNetCore.ViteIntegration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddControllersWithViews();
services.AddSingleton<ViteBuildManifest>();
services.AddAntiforgery();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
#pragma warning disable ASP0014 // using endpoints is required so controller match overrides spa in dev
  app.UseEndpoints(endpoints =>
  {
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
  });
#pragma warning restore ASP0014 // Suggest using top level route registrations

  app.UseSpa(spa =>
    {
      spa.UseProxyToSpaDevelopmentServer("https://localhost:3000");
    });
}
else
{
  app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

}

app.Run();
