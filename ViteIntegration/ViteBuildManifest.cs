using Microsoft.AspNetCore.Hosting;
using System.Text.Json;

namespace Soukoku.AspNetCore.ViteIntegration
{
    /// <summary>
    /// Vite build manifest reader. Used for published
    /// builds.
    /// </summary>
    public class ViteBuildManifest
    {
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Ctor that takes a <see cref="IWebHostEnvironment"/>
        /// and reads the manifest.json file in <see cref="IWebHostEnvironment.WebRootPath"/>.
        /// </summary>
        /// <param name="environment"></param>
        public ViteBuildManifest(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        private IReadOnlyDictionary<string, ViteFileChunk>? _current;

        /// <summary>
        /// Gets the parsed manifest chunks. Key is the entry file.
        /// </summary>
        public IReadOnlyDictionary<string, ViteFileChunk> Current
        {
            get { return _current ??= GetCurrent(); }
        }


        IReadOnlyDictionary<string, ViteFileChunk> GetCurrent()
        {
            IReadOnlyDictionary<string, ViteFileChunk>? value = null;
            var manifestFile = Path.Combine(_environment.WebRootPath, "manifest.json");
            if (File.Exists(manifestFile))
            {
                var json = File.ReadAllText(manifestFile);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                value = JsonSerializer.Deserialize<Dictionary<string, ViteFileChunk>>(json, options);
            }
            return value ?? new Dictionary<string, ViteFileChunk>();
        }

        /// <summary>
        /// Resolves all files related to an entry chunk.
        /// </summary>
        /// <param name="chunkKey">chunk file key in the manifest.</param>
        /// <param name="pathBase">Base url path to use.</param>
        /// <returns></returns>
        public ResolvedFiles ResolveEntryChunk(string chunkKey, string pathBase = "/")
        {
            var resolved = new ResolvedFiles();
            if (Current.TryGetValue(chunkKey, out ViteFileChunk? rootChunk) &&
                rootChunk != null && rootChunk.IsEntry)
            {
                resolved.MainModule = pathBase + rootChunk.File;
                if (rootChunk.Css != null)
                {
                    resolved.CssFiles.AddRange(rootChunk.Css.Select(path => pathBase + path));
                }
                if (rootChunk.DynamicImports != null)
                {
                    foreach (var subKey in rootChunk.DynamicImports)
                    {
                        PopulateSubChunk(resolved, subKey, pathBase);
                    }
                }

                resolved.CssFiles = resolved.CssFiles.Distinct().ToList();
            }
            return resolved;
        }

        private void PopulateSubChunk(ResolvedFiles resolved, string chunkKey, string pathBase)
        {
            var chunk = Current[chunkKey];
            resolved.PreloadModules.Add(pathBase + chunk.File);
            if (chunk.Css != null)
            {
                resolved.CssFiles.AddRange(chunk.Css.Select(path => pathBase + path));
            }
            if (chunk.DynamicImports != null)
            {
                foreach (var subKey in chunk.DynamicImports)
                {
                    PopulateSubChunk(resolved, subKey, pathBase);
                }
            }
        }
    }
}