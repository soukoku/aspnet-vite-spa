using System.Text.Json;

namespace aspnet_vite_spa
{
    public class ViteBuildManifest
    {
        private readonly IWebHostEnvironment environment;

        public ViteBuildManifest(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        private IReadOnlyDictionary<string, ViteChunk>? _current;

        public IReadOnlyDictionary<string, ViteChunk> Current
        {
            get { return _current ??= GetCurrent(); }
        }


        IReadOnlyDictionary<string, ViteChunk> GetCurrent()
        {
            IReadOnlyDictionary<string, ViteChunk>? value = null;
            var manifestFile = Path.Combine(environment.WebRootPath, "manifest.json");
            if (File.Exists(manifestFile))
            {
                var json = File.ReadAllText(manifestFile);
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                value = JsonSerializer.Deserialize<Dictionary<string, ViteChunk>>(json, options);
            }
            return value ?? new Dictionary<string, ViteChunk>();
        }
    }

    public class ViteChunk
    {
        public string File { get; set; } = "";
        public bool IsEntry { get; set; }
        public List<string>? DynamicImports { get; set; }
        public List<string> Css { get; set; } = new List<string>();
        public List<string> Assets { get; set; } = new List<string>();
    }
}
