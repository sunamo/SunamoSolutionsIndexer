namespace SunamoSolutionsIndexer;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class FoldersWithSolutions
{
    // toSelling can be null
    public static SolutionFolder CreateSolutionFolder(ILogger logger, string documentsFolder, string solutionFolder, PpkOnDriveDC toSelling, string? projName = null)
    {
        return CreateSolutionFolder(logger, documentsFolder, null!, solutionFolder, toSelling, projName);
    }

    // toSelling can be null
    public static SolutionFolder CreateSolutionFolder(ILogger logger, string documentsFolder, SolutionFolderSerialize solutionFolderData, string solutionFolder, PpkOnDriveDC toSelling, string? projName = null)
    {
        if (projName == null)
        {
            projName = Path.GetFileName(solutionFolder);
        }

        SolutionFolder? solutionFolderInstance = null;
        if (solutionFolderData != null)
        {
            solutionFolderInstance = new SolutionFolder(solutionFolderData);
        }
        else
        {
            solutionFolderInstance = new SolutionFolder();
        }

        solutionFolderInstance.Repository = RepositoryFromFullPath(solutionFolder);
        IdentifyProjectType(documentsFolder, solutionFolder, solutionFolderInstance);
        solutionFolderInstance.DisplayedText = GetDisplayedName(solutionFolder);
        solutionFolderInstance.FullPathFolder = solutionFolder;
        // Nevím zda je to nutné tak jsem to zakomentoval aby to bylo rychlejší
        //solutionFolderInstance.projects = new DebugCollection<string>( SolutionsIndexerHelper.ProjectsInSolution(true, solutionFolderInstance.FullPathFolder));
        //solutionFolderInstance.SourceOfProjects = SourceOfProjects.ProjectsInSolution;
        solutionFolderInstance.UpdateModules(logger, toSelling);
        solutionFolderInstance.NameSolutionWithoutDiacritic = SH.TextWithoutDiacritic(projName);
        return solutionFolderInstance;
    }

    private static RepositoryLocal RepositoryFromFullPath(string fullPathFolder)
    {
        if (fullPathFolder.Contains(SolutionsIndexerStrings.VisualStudio2017))
        {
            return RepositoryLocal.Vs17;
        }
        else if (fullPathFolder.Contains(SolutionsIndexerConsts.BitBucket))
        {
            return RepositoryLocal.BitBucket;
        }
        else if (fullPathFolder.Contains(BasePathsHelper.CRepos))
        {
            return RepositoryLocal.Vs17;
        }
        else if (fullPathFolder.Contains(BasePathsHelper.BpVps))
        {
            return RepositoryLocal.Vs17;
        }

        ThrowEx.NotImplementedCase(fullPathFolder);
        return RepositoryLocal.All;
    }

    // Get name based on relative but always fully recognized project
    private static string GetDisplayedName(string item)
    {
        return SolutionsIndexerHelper.GetDisplayedSolutionName(item);
    }

    public List<SolutionFolder> SolutionsUap(IList<string>? skipThese = null)
    {
        var slns = GetSolutions(RepositoryLocal.Vs17, false, skipThese);
        var uap = slns.Where(d => d.FullPathFolder.Contains(@"\_Uap\")).ToList();
        return uap;
    }

    public IList<SolutionFolder> SolutionsWildcard(RepositoryLocal repository, string wildcardPattern)
    {
        var result = GetSolutions(repository);
        for (int i = result.Count - 1; i >= 0; i--)
        {
            var solutionName = result[i].NameSolution;
            if (!SH.MatchWildcard(solutionName, wildcardPattern))
            {
                result.RemoveAt(i);
            }
        }

        return result;
    }

    // Gets solutions from repository with optional filtering
    // Excludes from SolutionsIndexerConsts.SolutionsExcludeWhileWorkingOnSourceCode if Debugger is attached and !loadAll
    // skipThese can use wildcard patterns
    public List<SolutionFolder> GetSolutions(RepositoryLocal repository, bool isLoadingAll = true, IList<string>? skipThese = null)
    {
        var result = new List<SolutionFolder>(Solutions);
        if (repository != RepositoryLocal.All)
        {
            result.RemoveAll(solution => solution.Repository != repository);
        }

        List<string>? skip = null;
        if (skipThese != null)
        {
            skip = skipThese.ToList();
        }
        else
        {
            skip = new List<string>();
        }

        if (!isLoadingAll)
        {
            if (Debugger.IsAttached)
            {
                skip.AddRange(SolutionsIndexerConsts.SolutionsExcludeWhileWorkingOnSourceCode);
            }
        }

        Dictionary<string, Wildcard> dict = new Dictionary<string, Wildcard>();
        foreach (var item in skip)
        {
            dict.Add(item, new Wildcard(item));
        }

        for (int i = result.Count - 1; i >= 0; i--)
        {
            var solution = result[i];
            foreach (var wildcardEntry in dict)
            {
                if (wildcardEntry.Value.IsMatch(solution.NameSolution))
                {
                    result.RemoveAt(i);
                    break;
                }
            }
        }
        //result.RemoveAll(d => CAG.IsEqualToAnyElement(d.NameSolution, skip));
        ////////DebugLogger.Instance.WriteCount("Solutions in " + documentsFolder, solutions);
        return result;
    }

    private List<string> ReturnAllProjectFolders(params string[] additionalFolders)
    {
        List<string> projects = new List<string>();
        var basePath = BasePathsHelper.BpMb;
        //if (Directory.Exists(basePath))
        //{
        //if (BasePathsHelper.BpVps == basePath)
        //{
        //    AddProjectsFolder(projects, basePath);
        //}
        //else
        //{
        List<string> visualStudioFolders = new List<string>([basePath]); // Directory.GetDirectories(folderWithVisualStudioFolders, VpsHelperSunamo.IsQ ? "_" : SolutionsIndexerStrings.VisualStudio2017, SearchOption.TopDirectoryOnly));
        foreach (var folder in additionalFolders)
        {
            AddProjectsFolder(projects, folder);
        }

        foreach (var vsFolder in visualStudioFolders)
        {
            List<string>? languageFolders = null;
            List<string> languageFoldersOutsideVs17 = new List<string>();
            try
            {
                languageFolders = Directory.GetDirectories(vsFolder).ToList();
            }
            catch (Exception)
            {
                continue;
            }

            //languageFoldersOutsideVs17SH.Leading(Path.Combine(folderWithVisualStudioFolders.Replace("E:\\", "D:\\"), SolutionsIndexerConsts.BitBucket));
            foreach (var languageFolder in languageFolders)
            {
#region New
                string folderName = Path.GetFileName(languageFolder);
                if (SolutionsIndexerHelper.IsTheSolutionsFolder(folderName) || folderName.StartsWith("_"))
                {
                    AddProjectsFolder(projects, languageFolder);
                }
#endregion
            }

            foreach (var languageFolder in languageFoldersOutsideVs17)
            {
#region New
                if (Directory.Exists(languageFolder))
                {
                    string folderName = Path.GetFileName(languageFolder);
                    AddProjectsFolder(projects, languageFolder);
                }
#endregion
            }
        }

        //}
        //}
        //else if (VpsHelperSunamo.IsVps)
        //{
        //    return new List<string>();
        //}
        //else
        //{
        //    throw new Exception(folderWithVisualStudioFolders + " not exists, therefore will be return none slsn");
        //}
        CAChangeContent.ChangeContent0(null!, projects, SH.FirstCharUpper);
        return projects;
    }

    public static Tuple<List<string>, List<string>> ReturnNormalAndSpecialFolders(string folder)
    {
        List<string>? specialFolders = null;
        List<string>? normalFolders = null;
        ReturnNormalAndSpecialFolders(folder, out specialFolders, out normalFolders);
        return new Tuple<List<string>, List<string>>(normalFolders, specialFolders);
    }

    public static void ReturnNormalAndSpecialFolders(string folder, out List<string> specialFolders, out List<string> normalFolders)
    {
        specialFolders = new List<string>();
        normalFolders = new List<string>();
        try
        {
            var directories = Directory.GetDirectories(folder);
            foreach (string folderPath in directories)
            {
                string name = Path.GetFileName(folderPath);
                if (name.StartsWith("_"))
                {
                    specialFolders.Add(folderPath);
                }
                else
                {
                    normalFolders.Add(folderPath);
                }
            }
        }
        catch (Exception)
        {
        }
    }
}