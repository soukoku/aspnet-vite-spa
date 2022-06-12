namespace Soukoku.AspNetCore.ViteIntegration
{
    /// <summary>
    /// Chunk info from vite manifest.
    /// </summary>
    public class ViteFileChunk
    {
        /// <summary>
        /// Main file of the chunk.
        /// </summary>
        public string File { get; set; } = "";

        /// <summary>
        /// Whether the chunk is an entry.
        /// </summary>
        public bool IsEntry { get; set; }

        /// <summary>
        /// Additional dynamic imports for the chunk.
        /// </summary>
        public List<string>? DynamicImports { get; set; }

        /// <summary>
        /// Additional css files for the chunk.
        /// </summary>
        public List<string> Css { get; set; } = new List<string>();

        /// <summary>
        /// Additional asset files for the chunk.
        /// </summary>
        public List<string> Assets { get; set; } = new List<string>();
    }
}