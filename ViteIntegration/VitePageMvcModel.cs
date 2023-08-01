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

    /// <summary>
    /// Optional data to be written to the page as json.
    /// </summary>
    public object? PageData { get; set; }

    /// <summary>
    /// Writes antiforgery form value to the page.
    /// </summary>
    public bool UseAntiforgery { get; set; }
  }
}
