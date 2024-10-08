﻿#if NETFRAMEWORK
namespace Soukoku.AspNet.Mvc.ViteIntegration;
#else
namespace Soukoku.AspNetCore.ViteIntegration;
#endif


/// <summary>
/// Parsed vite build manifest.
/// </summary>
public class ViteBuildManifest
{
#if NETFRAMEWORK
    /// <summary>
    /// The global default for mvc view use.
    /// </summary>
    public static ViteBuildManifest Default { get; set; } = new("");
#endif

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

            value = JsonWrapper.Deserialize<Dictionary<string, ViteFileChunk>>(json);
        }
        Entries = value ?? new Dictionary<string, ViteFileChunk>();
    }

#if !NETFRAMEWORK
    /// <summary>
    /// Initializes with an assumed
    /// manifest.json file in <see cref="Microsoft.AspNetCore.Hosting.IWebHostEnvironment.WebRootPath"/>.
    /// </summary>
    /// <param name="environment"></param>
    public ViteBuildManifest(Microsoft.AspNetCore.Hosting.IWebHostEnvironment environment)
        : this(Path.Combine(environment.WebRootPath ?? "", ".vite", "manifest.json"))
    {
    }

#endif

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

        // skip processed in case if circular deps
        if (resolved.PreloadModules.Any(m => m.EndsWith(chunk.File))) return;

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