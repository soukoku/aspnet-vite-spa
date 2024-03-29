﻿using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Soukoku.AspNetCore.ViteIntegration
{
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
