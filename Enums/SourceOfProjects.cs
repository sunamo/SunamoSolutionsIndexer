namespace SunamoSolutionsIndexer.Enums;

/// <summary>
/// SolutionFolder.GetCsprojs. SolutionsIndexerHelper.ProjectsInSolution
/// </summary>
public enum SourceOfProjects
{
    /// <summary>
    /// AllProjectsSearchHelper
    /// Return csproj full paths in subfolders of A1 (one depth)
    /// </summary>
    GetCsprojs,
    /// <summary>
    /// SolutionsIndexerHelper
    /// Find as subfolders (is not guarantee in subfolder will be .csproj). Can remove VS folders and return only names
    /// </summary>
    ProjectsInSolution
}
