namespace SunamoSolutionsIndexer;

/// <summary>
/// Represents the i folders with solutions type.
/// </summary>
public interface IFoldersWithSolutions
{
    /// <summary>
    /// Solutions.
    /// </summary>
    /// <param name="repository">The repository.</param>
    /// <param name="isLoadingAll">The is loading all.</param>
    /// <param name="skipThese">The skip these.</param>
    /// <param name="prioritize">The prioritize.</param>
    /// <returns>The solutions result.</returns>
    SolutionFolders Solutions(RepositoryLocal repository, bool isLoadingAll = true, IList<string>? skipThese = null, ProjectsTypes prioritize = ProjectsTypes.None);
}
