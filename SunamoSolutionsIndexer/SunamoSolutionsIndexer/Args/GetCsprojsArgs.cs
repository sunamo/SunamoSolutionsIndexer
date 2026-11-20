namespace SunamoSolutionsIndexer.Args;

/// <summary>
/// Represents the get csprojs args type.
/// </summary>
public class GetCsprojsArgs
{
    /// <summary>
    /// Gets or sets the only names.
    /// </summary>
    public bool OnlyNames { get; set; } = false;
    /// <summary>
    /// Gets or sets the force reload.
    /// </summary>
    public bool ForceReload { get; set; } = false;
}