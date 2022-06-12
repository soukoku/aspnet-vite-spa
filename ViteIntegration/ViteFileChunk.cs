namespace ViteIntegration
{
    public class ViteFileChunk
    {
        public string File { get; set; } = "";
        public bool IsEntry { get; set; }
        public List<string>? DynamicImports { get; set; }
        public List<string> Css { get; set; } = new List<string>();
        public List<string> Assets { get; set; } = new List<string>();
    }
}