namespace Soukoku.AspNetCore.ViteIntegration
{
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
}
