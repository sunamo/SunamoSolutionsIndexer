namespace SunamoSolutionsIndexer.Enums;

// SolutionFolder.GetCsprojs. SolutionsIndexerHelper.ProjectsInSolution
/// <summary>
/// Represents the source of projects type.
/// </summary>
public enum SourceOfProjects
{
    // AllProjectsSearchHelper
    // Return csproj full paths in subfolders of A1 (one depth)
    /// <summary>
    /// The get csprojs value.
    /// </summary>
    GetCsprojs,
    // SolutionsIndexerHelper
    // Find as subfolders (is not guarantee in subfolder will be .csproj). Can remove VS folders and return only names
    /// <summary>
    /// The projects in solution value.
    /// </summary>
    ProjectsInSolution
}