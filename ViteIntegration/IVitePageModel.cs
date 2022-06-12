using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViteIntegration
{
    public interface IVitePageModel
    {
        /// <summary>
        /// Path to the entry script for the page (e.g. src/main.ts).
        /// </summary>
        public string Entry { get; set; }
    }


    public class VitePageMvcModel : IVitePageModel
    {
        /// <summary>
        /// Path to the entry script for the page (e.g. src/main.ts).
        /// </summary>
        public string Entry { get; set; } = "";
    }

    public abstract class VitePageModel : PageModel, IVitePageModel
    {
        /// <summary>
        /// Path to the entry script for the page (e.g. src/main.ts).
        /// </summary>
        public string Entry { get; set; } = "";
    }
}
