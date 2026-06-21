namespace SunamoSolutionsIndexer;

/// <summary>
/// Settings for Solutions Indexer.
/// </summary>
public class SolutionsIndexerSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether to ignore the part after underscore when processing solution names.
    /// </summary>
    public static bool IgnorePartAfterUnderscore { get; set; } = false;
}