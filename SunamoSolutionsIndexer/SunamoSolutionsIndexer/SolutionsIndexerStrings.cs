namespace SunamoSolutionsIndexer;

/// <summary>
/// Represents the solutions indexer strings type.
/// </summary>
public class SolutionsIndexerStrings
{
    /// <summary>
    /// The visual studio 2017 constant.
    /// </summary>
    public const string VisualStudio2017 = @"vs";
    /// <summary>
    /// The git hub constant.
    /// </summary>
    public const string GitHub = "GitHub";
    /// <summary>
    /// The git hub my constant.
    /// </summary>
    public const string GitHubMy = "GitHubMy";
    /// <summary>
    /// The bit bucket constant.
    /// </summary>
    public const string BitBucket = "BitBucket";
    /// <summary>
    /// The project postfix constant.
    /// </summary>
    public const string ProjectPostfix = "_Projects";

    // Is used to get relative paths.
    /// <summary>
    /// The with direct solutions.
    /// </summary>
    public static readonly List<string> WithDirectSolutions = new List<string> { GitHubMy, SolutionsIndexerConsts.ProjectsFolderName };
}
