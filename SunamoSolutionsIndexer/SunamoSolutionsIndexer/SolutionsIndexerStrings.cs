namespace SunamoSolutionsIndexer;

public class SolutionsIndexerStrings
{
    public const string VisualStudio2017 = @"vs";
    public const string GitHub = "GitHub";
    public const string GitHubMy = "GitHubMy";
    public const string BitBucket = "BitBucket";
    public const string ProjectPostfix = "_Projects";

    // Is used to get relative paths.
    public static readonly List<string> WithDirectSolutions = new List<string> { GitHubMy, SolutionsIndexerConsts.ProjectsFolderName };
}
