namespace SunamoSolutionsIndexer;

/// <summary>
/// String constants for Solutions Indexer.
/// </summary>
public class SolutionsIndexerStrings
{
    /// <summary>
    /// Visual Studio 2017 folder name.
    /// </summary>
    public const string VisualStudio2017 = @"vs";

    /// <summary>
    /// GitHub folder name.
    /// </summary>
    public const string GitHub = "GitHub";

    /// <summary>
    /// GitHubMy folder name.
    /// </summary>
    public const string GitHubMy = "GitHubMy";

    /// <summary>
    /// BitBucket folder name.
    /// </summary>
    public const string BitBucket = "BitBucket";

    /// <summary>
    /// Project postfix suffix.
    /// </summary>
    public const string ProjectPostfix = "_Projects";

    /// <summary>
    /// Is used to get relative paths.
    /// </summary>
    public static readonly List<string> WithDirectSolutions = new List<string> { GitHubMy, SolutionsIndexerConsts.ProjectsFolderName };
}