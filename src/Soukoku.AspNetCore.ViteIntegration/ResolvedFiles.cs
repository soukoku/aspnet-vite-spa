#if NETFRAMEWORK
namespace Soukoku.AspNet.Mvc.ViteIntegration;
#else
namespace Soukoku.AspNetCore.ViteIntegration;
#endif

/// <summary>
/// Contains resolved script and css files for inclusion on a page.
/// The inclusion order should be <see cref="MainModule"/>, <see cref="PreloadModules"/>,
/// and <see cref="CssFiles"/> in the page head.
/// The paths are virtual so they should be resolved with something like Url.Content().
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