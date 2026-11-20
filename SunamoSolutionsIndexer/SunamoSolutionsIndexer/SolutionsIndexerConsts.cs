namespace SunamoSolutionsIndexer;

/// <summary>
/// Represents the solutions indexer consts type.
/// </summary>
public class SolutionsIndexerConsts
{
    /// <summary>
    /// The bit bucket constant.
    /// </summary>
    public const string BitBucket = "BitBucket";
    /// <summary>
    /// The projects folder name constant.
    /// </summary>
    public const string ProjectsFolderName = "Projects";
    /// <summary>
    /// The visual studio constant.
    /// </summary>
    public const string VisualStudio = "Visual Studio";

    /// <summary>
    /// The solutions exclude while working on source code.
    /// </summary>
    public static List<string> SolutionsExcludeWhileWorkingOnSourceCode = new List<string>(["AllProjectsSearch", "sunamo", "CodeBoxControl"]);
}
