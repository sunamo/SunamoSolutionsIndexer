namespace SunamoDevCode.SunamoSolutionsIndexer.Args;

/// <summary>
/// Arguments for getting csproj files.
/// </summary>
public class GetCsprojsArgs
{
    /// <summary>
    /// Gets or sets a value indicating whether to return only names with extension.
    /// </summary>
    public bool OnlyNames { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to force reload.
    /// </summary>
    public bool ForceReload { get; set; } = false;
}