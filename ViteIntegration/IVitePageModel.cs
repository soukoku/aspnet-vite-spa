using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Soukoku.AspNetCore.ViteIntegration
{
    /// <summary>
    /// Interface for a model that will be passed to the vite views.
    /// </summary>
    public interface IVitePageModel
    {
        /// <summary>
        /// Path to the entry script for the page (e.g. src/main.ts).
        /// </summary>
        public string Entry { get; set; }
    }

    /// <summary>
    /// Base model for mvc use.
    /// </summary>
    public class VitePageMvcModel : IVitePageModel
    {
        /// <summary>
        /// Path to the entry script for the page (e.g. src/main.ts).
        /// </summary>
        public string Entry { get; set; } = "";
    }

    /// <summary>
    /// Base model for razor page use.
    /// </summary>
    public abstract class VitePageModel : PageModel, IVitePageModel
    {
        /// <summary>
        /// Path to the entry script for the page (e.g. src/main.ts).
        /// </summary>
        public string Entry { get; set; } = "";
    }
}
