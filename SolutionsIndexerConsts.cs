namespace SunamoSolutionsIndexer;

public class SolutionsIndexerConsts
{
    public const string BitBucket = "BitBucket";
    public const string ProjectsFolderName = "Projects";
    public const string VisualStudio = "Visual Studio";

    public static List<string> SolutionsExcludeWhileWorkingOnSourceCode = CA.ToListString("AllProjectsSearch", "sunamo", "CodeBoxControl");
}
