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
        /// Source path of the <see cref="File"/> if applicable.
        /// </summary>
        public string? Src { get; set; }

        /// <summary>
        /// Whether the chunk is an entry.
        /// </summary>
        public bool IsEntry { get; set; }

        /// <summary>
        /// Additional dynamic imports for the chunk.
        /// </summary>
        public List<string> Imports { get; set; } = new List<string>();

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