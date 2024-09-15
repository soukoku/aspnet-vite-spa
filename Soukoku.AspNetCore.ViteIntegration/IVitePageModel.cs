#if NETFRAMEWORK
namespace Soukoku.AspNet.ViteIntegration;
#else
namespace Soukoku.AspNetCore.ViteIntegration;
#endif

/// <summary>
/// Interface for a model that will be passed to the vite views.
/// </summary>
public interface IVitePageModel
{
/// <summary>
/// Path to the entry script for the page (e.g. src/main.ts).
/// </summary>
string Entry { get; set; }
}
