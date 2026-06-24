namespace SunamoSolutionsIndexer;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
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

    public static List<string> FullPathFolders(RepositoryLocal usedRepository, List<string>? returnOnlyThese = null)
    {
        Dictionary<string, SolutionFolder>? solutionFoldersMap = null;
        return FullPathFolders(usedRepository, solutionFoldersMap, returnOnlyThese);
    }

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
