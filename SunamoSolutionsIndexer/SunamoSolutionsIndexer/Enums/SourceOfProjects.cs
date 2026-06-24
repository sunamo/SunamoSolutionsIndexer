namespace SunamoSolutionsIndexer;

// SolutionFolder.GetCsprojs. SolutionsIndexerHelper.ProjectsInSolution
public enum SourceOfProjects
{
    // AllProjectsSearchHelper
    // Return csproj full paths in subfolders of A1 (one depth)
    GetCsprojs,
    // SolutionsIndexerHelper
    // Find as subfolders (is not guarantee in subfolder will be .csproj). Can remove VS folders and return only names
    ProjectsInSolution
}
