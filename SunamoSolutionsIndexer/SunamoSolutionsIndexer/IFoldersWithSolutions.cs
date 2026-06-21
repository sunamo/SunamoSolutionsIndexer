namespace SunamoDevCode.SunamoSolutionsIndexer;

/// <summary>
/// Interface for managing folders containing Visual Studio solutions
/// </summary>
public interface IFoldersWithSolutions
{
    /// <summary>
    /// Gets solution folders from repository with optional filtering
    /// </summary>
    /// <param name="repository">Repository to get solutions from</param>
    /// <param name="isLoadingAll">If false and debugger attached, excludes working solutions</param>
    /// <param name="skipThese">Solution names to skip (supports wildcards)</param>
    /// <param name="prioritize">Project type to prioritize in results</param>
    /// <returns>Collection of solution folders</returns>
    SolutionFolders Solutions(RepositoryLocal repository, bool isLoadingAll = true, IList<string>? skipThese = null, ProjectsTypes prioritize = ProjectsTypes.None);
}