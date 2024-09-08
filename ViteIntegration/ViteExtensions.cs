using Soukoku.AspNetCore.ViteIntegration;
using WebApp.Controllers;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Contains extension methods for adding Vite support to an <see cref="IServiceCollection"/> instance.
/// </summary>
public static class ViteExtensions
{
    /// <summary>
    /// Adds a Vite manifest to the <see cref="IServiceCollection"/> and setup the Vite dev server proxy.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="devTimeUrl">Url to vite dev server at dev time. If base is used, add it with slash like https://localhost/3000/somebase/.</param>
    /// <returns></returns>
    public static IServiceCollection AddViteManifest(this IServiceCollection services, string? devTimeUrl = "https://localhost:3000")
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<ViteBuildManifest>();
        DevSpaProxyController.SetDevTimeUrl(devTimeUrl);

        return services;
    }
}
