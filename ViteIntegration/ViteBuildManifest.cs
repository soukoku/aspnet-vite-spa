using Microsoft.AspNetCore.Hosting;
using System.Text.Json;

namespace Soukoku.AspNetCore.ViteIntegration
{
    /// <summary>
    /// Parsed vite build manifest.
    /// </summary>
    public class ViteBuildManifest
    {
        /// <summary>
        /// Gets the underlying manifest dictionary.
        /// </summary>
        public IReadOnlyDictionary<string, ViteFileChunk> Entries { get; }

        /// <summary>
        /// Initializes with a dictionary.
        /// </summary>
        /// <param name="manifest"></param>
        public ViteBuildManifest(IReadOnlyDictionary<string, ViteFileChunk> manifest)
        {
            Entries = manifest;
        }


        /// <summary>
        /// Initializes with a manifest file..
        /// </summary>
        /// <param name="manifestFile">File path to the manifest.json.</param>
        public ViteBuildManifest(string manifestFile)
        {
            IReadOnlyDictionary<string, ViteFileChunk>? value = null;
            if (File.Exists(manifestFile))
            {
                var json = File.ReadAllText(manifestFile);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                value = JsonSerializer.Deserialize<Dictionary<string, ViteFileChunk>>(json, options);
            }
            Entries = value ?? new Dictionary<string, ViteFileChunk>();
        }


        /// <summary>
        /// Initializes with an assumed
        /// manifest.json file in <see cref="IWebHostEnvironment.WebRootPath"/>.
        /// </summary>
        /// <param name="environment"></param>
        public ViteBuildManifest(IWebHostEnvironment environment)
            : this(Path.Combine(environment.WebRootPath, ".vite", "manifest.json"))
        {
        }

        /// <summary>
        /// Resolves all files related to an entry chunk.
        /// </summary>
        /// <param name="chunkKey">chunk file key in the manifest.</param>
        /// <returns></returns>
        public ResolvedFiles ResolveEntryChunk(string chunkKey)
        {
            var resolved = new ResolvedFiles();
            if (Entries.TryGetValue(chunkKey, out ViteFileChunk? rootChunk) &&
                rootChunk != null && rootChunk.IsEntry)
            {
                resolved.MainModule = "~/" + rootChunk.File;
                if (rootChunk.Css != null)
                {
                    resolved.CssFiles.AddRange(rootChunk.Css.Select(path => "~/" + path));
                }
                if (rootChunk.Imports != null)
                {
                    foreach (var subKey in rootChunk.Imports)
                    {
                        PopulateSubChunk(resolved, subKey);
                    }
                }

                resolved.CssFiles = resolved.CssFiles.Distinct().ToList();
            }
            return resolved;
        }

        private void PopulateSubChunk(ResolvedFiles resolved, string chunkKey)
        {
            var chunk = Entries[chunkKey];
            resolved.PreloadModules.Add("~/" + chunk.File);
            if (chunk.Css != null)
            {
                resolved.CssFiles.AddRange(chunk.Css.Select(path => "~/" + path));
            }
            if (chunk.Imports != null)
            {
                foreach (var subKey in chunk.Imports)
                {
                    PopulateSubChunk(resolved, subKey);
                }
            }
        }
    }
}