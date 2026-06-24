namespace SunamoSolutionsIndexer;

public class ProjectFoldersSerialize
{
    public List<ProjectFolderSerialize> Projects { get; set; } = new List<ProjectFolderSerialize>();

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
