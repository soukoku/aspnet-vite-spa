using Microsoft.AspNetCore.Hosting;
using System.Text.Json;

namespace ViteIntegration
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
    }
}