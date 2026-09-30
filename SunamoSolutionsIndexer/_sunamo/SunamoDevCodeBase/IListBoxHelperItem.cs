namespace SunamoSolutionsIndexer._sunamo;

/// <summary>
/// Item of a list box.
/// </summary>
internal interface IListBoxHelperItem
{
    string RunOne { get; }
    // For use in AllProjectsSearch - in LBH im working just with var and SolutionFolders are many.
    // Then I have to use very abstract ShortName or LongName
    string ShortName { get; }
    string LongName { get; }
}