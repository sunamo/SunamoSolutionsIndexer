namespace SunamoDevCode.SunamoSolutionsIndexer;

/// <summary>
/// Constants used by the solutions indexer system.
/// </summary>
public class SolutionsIndexerConsts
{
    /// <summary>
    /// Identifier for the BitBucket repository source.
    /// </summary>
    public const string BitBucket = "BitBucket";
    /// <summary>
    /// Default folder name used for storing projects.
    /// </summary>
    public const string ProjectsFolderName = "Projects";
    /// <summary>
    /// Display name for Visual Studio.
    /// </summary>
    public const string VisualStudio = "Visual Studio";

    /// <summary>
    /// List of solution names to exclude while working on source code in debugger mode.
    /// </summary>
    public static List<string> SolutionsExcludeWhileWorkingOnSourceCode = new List<string>(["AllProjectsSearch", "sunamo", "CodeBoxControl"]);
}