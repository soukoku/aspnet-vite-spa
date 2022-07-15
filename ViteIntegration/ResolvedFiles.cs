namespace Soukoku.AspNetCore.ViteIntegration
{
    /// <summary>
    /// Contains resolved script and css files for inclusion on a page.
    /// The inclusion order should be <see cref="MainModule"/>, <see cref="PreloadModules"/>,
    /// and <see cref="CssFiles"/> in the page head.
    /// </summary>
    public class ResolvedFiles
    {
        /// <summary>
        /// Main js file of the chunk.
        /// Can be included as &lt;script type="module" crossorigin src="filepath"&gt;&lt;/script&gt;.
        /// </summary>
        public string MainModule { get; internal set; } = "";

        /// <summary>
        /// Any additional js files used by the chunk.
        /// Can be included as &lt;link rel="modulepreload" href="filepath"&gt;.
        /// </summary>
        public List<string> PreloadModules { get; internal set; } = new List<string>();

        /// <summary>
        /// Any additional css files used by the chunk.
        /// Can be included as &lt;link rel="stylesheet" href="filepath"&gt;.
        /// </summary>
        public List<string> CssFiles { get; internal set; } = new List<string>();
    }
}