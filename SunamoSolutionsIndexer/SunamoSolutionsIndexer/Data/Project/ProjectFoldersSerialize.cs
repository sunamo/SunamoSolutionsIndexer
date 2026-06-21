namespace SunamoSolutionsIndexer.Data.Project;

/// <summary>
/// Collection of project folders for serialization.
/// </summary>
public class ProjectFoldersSerialize
{
    /// <summary>
    /// Gets or sets the list of projects.
    /// </summary>
    public List<ProjectFolderSerialize> Projects { get; set; } = new List<ProjectFolderSerialize>();

    /// <summary>
    /// Gets project folders by their names.
    /// </summary>
    /// <param name="projectNamesToFind">List of project names to find.</param>
    /// <param name="isAllowingMissing">Determines whether to allow missing projects without throwing an exception.</param>
    /// <returns>Result containing found project folders or exception if projects are missing and not allowed.</returns>
    public ResultWithExceptionDC<ProjectFoldersSerialize> GetWithName(List<string> projectNamesToFind, bool isAllowingMissing)
    {
        ResultWithExceptionDC<ProjectFoldersSerialize> result = new ResultWithExceptionDC<ProjectFoldersSerialize>();
        result.Data = new ProjectFoldersSerialize();

        foreach (var item in projectNamesToFind)
        {
            ProjectFolderSerialize? projectFolder = Projects.Find(project => project.NameProject == item);

            if (projectFolder == null)
            {
                if (!isAllowingMissing)
                {
                    result.Exc = Exceptions.ElementCantBeFound("", "projectNamesToFind", item)!;
                }
            }
            else
            {
                result.Data.Projects.Add(projectFolder);
            }
        }

        return result;
    }

    /// <summary>
    /// Removes projects with specified names.
    /// </summary>
    /// <param name="projectNamesToRemove">List of project names to remove.</param>
    public void RemoveWithName(List<string> projectNamesToRemove)
    {
        foreach (var item in projectNamesToRemove)
        {
            int foundIndex = Projects.FindIndex(project => project.NameProject == item);
            if (foundIndex != -1)
            {
                Projects.RemoveAt(foundIndex);
            }
        }
    }
}