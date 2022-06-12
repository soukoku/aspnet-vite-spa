using Microsoft.AspNetCore.Hosting;
using System.Text.Json;

namespace ViteIntegration
{
    public class ViteBuildManifest
    {
        private readonly IWebHostEnvironment environment;

        public ViteBuildManifest(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        private IReadOnlyDictionary<string, ViteFileChunk>? _current;

        public IReadOnlyDictionary<string, ViteFileChunk> Current
        {
            get { return _current ??= GetCurrent(); }
        }


        IReadOnlyDictionary<string, ViteFileChunk> GetCurrent()
        {
            IReadOnlyDictionary<string, ViteFileChunk>? value = null;
            var manifestFile = Path.Combine(environment.WebRootPath, "manifest.json");
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