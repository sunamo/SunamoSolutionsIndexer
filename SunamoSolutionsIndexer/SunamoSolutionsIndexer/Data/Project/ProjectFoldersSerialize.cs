namespace SunamoSolutionsIndexer.Data.Project;

/// <summary>
/// Represents the project folders serialize type.
/// </summary>
public class ProjectFoldersSerialize
{
    /// <summary>
    /// Gets or sets the projects.
    /// </summary>
    public List<ProjectFolderSerialize> Projects { get; set; } = new List<ProjectFolderSerialize>();

    /// <summary>
    /// Get with name.
    /// </summary>
    /// <typeparam name="ProjectFolderSerialize">The type ProjectFolderSerialize.</typeparam>
    /// <returns>The get with name result.</returns>
    public ResultWithExceptionDC<ProjectFoldersSerialize> GetWithName(List<string> projectNamesToFind, bool isAllowingMissing)
    {
        var result = new ResultWithExceptionDC<ProjectFoldersSerialize>();
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
    /// Remove with name.
    /// </summary>
    /// <param name="projectNamesToRemove">The project names to remove.</param>
    /// <returns>The remove with name result.</returns>
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