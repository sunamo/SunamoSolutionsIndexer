namespace SunamoSolutionsIndexer;

public partial class FoldersWithSolutions
{
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
    /// Full path folders.
    /// </summary>
    /// <param name="usedRepository">The used repository.</param>
    /// <param name="returnOnlyThese">The return only these.</param>
    /// <returns>The full path folders result.</returns>
    public static List<string> FullPathFolders(RepositoryLocal usedRepository, List<string>? returnOnlyThese = null)
    {
        Dictionary<string, SolutionFolder>? solutionFoldersMap = null;
        return FullPathFolders(usedRepository, solutionFoldersMap, returnOnlyThese);
    }

    /// <summary>
    /// Full path folders.
    /// </summary>
    /// <param name="usedRepository">The used repository.</param>
    /// <param name="solutionFoldersMap">The solution folders map.</param>
    /// <param name="returnOnlyThese">The return only these.</param>
    /// <returns>The full path folders result.</returns>
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
