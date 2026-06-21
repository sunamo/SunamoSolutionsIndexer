namespace SunamoDevCode.SunamoSolutionsIndexer;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class FoldersWithSolutions
{
    /// <summary>
    /// Finds usual folders and special folders (starting with _) and processes them recursively
    /// </summary>
    /// <param name="projects">List to add project folders to</param>
    /// <param name="folder">Folder to scan</param>
    void AddProjectsFolder(List<string> projects, string folder)
    {
        List<string> specialFolders, normalFolders;
        ReturnNormalAndSpecialFolders(folder, out specialFolders, out normalFolders);
        normalFolders = CA.EnsureBackslash(normalFolders);
        projects.AddRange(normalFolders);
        foreach (string specialFolder in specialFolders)
        {
            AddProjectsFolder(projects, specialFolder);
        }
    }

    /// <summary>
    /// Gets full path folders from repository
    /// </summary>
    /// <param name="usedRepository">Repository to get folders from</param>
    /// <param name="returnOnlyThese">Optional filter for specific solution names</param>
    /// <returns>List of full path folders</returns>
    public static List<string> FullPathFolders(RepositoryLocal usedRepository, List<string>? returnOnlyThese = null)
    {
        Dictionary<string, SolutionFolder>? solutionFoldersMap = null;
        return FullPathFolders(usedRepository, solutionFoldersMap, returnOnlyThese);
    }

    /// <summary>
    /// Gets full path folders from repository with optional solution folder mapping
    /// </summary>
    /// <param name="usedRepository">Repository to get folders from</param>
    /// <param name="solutionFoldersMap">Optional dictionary to populate with solution folder mappings</param>
    /// <param name="returnOnlyThese">Optional filter for specific solution names</param>
    /// <returns>List of full path folders</returns>
    public static List<string> FullPathFolders(RepositoryLocal usedRepository, Dictionary<string, SolutionFolder>? solutionFoldersMap, List<string>? returnOnlyThese = null)
    {
        List<string> lines = new List<string>();
        foreach (var item in Fwss)
        {
            var slns = item.GetSolutions(usedRepository);
            foreach (var sln in slns)
            {
                if (returnOnlyThese != null)
                {
#if DEBUG
                    if (sln.NameSolution.Contains("OnlyWeb"))
                    {
                    }
#endif
                    if (!returnOnlyThese.Contains(sln.NameSolution))
                    {
                        continue;
                    }
                }

                if (solutionFoldersMap != null)
                {
                    solutionFoldersMap.Add(sln.FullPathFolder, sln);
                }

                lines.Add(sln.FullPathFolder);
            }
        }

        return lines;
    }
}