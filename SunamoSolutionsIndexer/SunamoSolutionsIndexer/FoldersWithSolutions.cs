namespace SunamoSolutionsIndexer;

public partial class FoldersWithSolutions
{
    // Was static on 3-10-23 but don't know why when solutions is instance field
    public List<SolutionFolder> Solutions { get; set; } = null!;

    // Path to documents folder, e.g., D:\Documents
    public string DocumentsFolder { get; set; } = null!;

    private static Type Type = typeof(FoldersWithSolutions);
    public static RepositoryLocal UsedRepository { get; set; } = RepositoryLocal.Vs17;
    protected static void IdentifyProjectType(string documentsFolder, string solutionFolder, SolutionFolder solutionFolderData)
    {
        // SolutionFolderSerialize doesn't have InVsFolder or typeProjectFolder
        solutionFolderData.InVsFolder = solutionFolder.Contains(SolutionsIndexerStrings.VisualStudio2017);
        if (solutionFolderData.InVsFolder)
        {
            solutionFolder = SHTrim.TrimStart(solutionFolder, documentsFolder);
            var pathParts = SHSplit.SplitChar(solutionFolder, '\\');
            var projectTypeFolder = pathParts[0];
            projectTypeFolder = projectTypeFolder.Replace(SolutionsIndexerStrings.ProjectPostfix, string.Empty);
            if (projectTypes.SecondToFirst.ContainsKey(projectTypeFolder))
            {
                solutionFolderData.TypeProjectFolder = projectTypes.SecondToFirst[projectTypeFolder];
            }
            else
            {
                solutionFolderData.TypeProjectFolder = ProjectsTypes.Unknown;
            }
        }
    }

    // Folder where to search for Projects folder and Visual Studio folders. Added here when calling FoldersWithSolutions ctor. If no sln, call new FoldersWithSolutions(BasePathsHelper.Vs, null);
    public static FoldersWithSolutionsList Fwss { get; set; } = new FoldersWithSolutionsList();
    public static void InsertIntoFwss(ILogger logger, string documentsFolder, PpkOnDriveDC toSelling, bool addAlsoSolutions = true)
    {
        new FoldersWithSolutions(logger, documentsFolder, toSelling, addAlsoSolutions);
    }

    // This class should be instaniate only once and then call reload by needs
    // A1 toSelling can be null
    public FoldersWithSolutions(ILogger logger, string documentsFolder, PpkOnDriveDC toSelling, bool addAlsoSolutions = true)
    {
        // documentsFolder can be null / SE, I fill it in from actualPlatform afterwards anyway
        ThrowEx.DirectoryExists(documentsFolder);
        this.DocumentsFolder = documentsFolder;
        if (addAlsoSolutions)
        {
            Solutions = Reload(logger, documentsFolder, toSelling);
        }

        Fwss.Add(this);
    // Cannot use, FoldersWithSolutions is not derived from FoldersWithSolutions
    }

    public FoldersWithSolutions(FoldersWithSolutions fws2)
    {
        Solutions = fws2.Solutions;
        DocumentsFolder = fws2.DocumentsFolder;
        Fwss = FoldersWithSolutions.Fwss;
        OnlyRealLoadedSolutionsFolders = FoldersWithSolutions.OnlyRealLoadedSolutionsFolders;
    }

    public List<SolutionFolderWithFiles> SolutionsWithFiles()
    {
        List<SolutionFolderWithFiles> result = new List<SolutionFolderWithFiles>();
        foreach (var solutionFolder in Solutions)
        {
            result.Add(new SolutionFolderWithFiles(solutionFolder));
        }

        return result;
    }

    public List<SolutionFolder> Reload(ILogger logger, string documentsFolder, PpkOnDriveDC toSelling /*, bool ignorePartAfterUnderscore = false*/)
    {
        PairProjectFolderWithEnum(logger, documentsFolder);
        // Get all projects in A1(Visual Studio Projects *) and GitHub folder
        List<string> solutionFolders = ReturnAllProjectFolders(documentsFolder /*, Path.Combine(documentsFolder, SolutionsIndexerStrings.GitHubMy)*/);
        // Remove trailing backslashes before extracting names
        var solutionFoldersWithoutTrailingSlash = solutionFolders.Select(f => f.TrimEnd('\\')).ToArray();
        List<string> projOnlyNames = new List<string>(solutionFolders.Count);
        var on = FS.OnlyNamesNoDirectEdit(solutionFoldersWithoutTrailingSlash);
        projOnlyNames.AddRange(on);
        // Initialize global variable solutions
        Solutions = new List<SolutionFolder>(solutionFolders.Count);
        for (int i = 0; i < solutionFolders.Count; i++)
        {
            var solutionFolder = solutionFolders[i];
            SolutionFolder sf = CreateSolutionFolder(logger, documentsFolder, solutionFolder, toSelling, projOnlyNames[i]);
            Solutions.Add(sf);
        }

        return Solutions;
    }

    private static TwoWayDictionary<ProjectsTypes, string> projectTypes = new TwoWayDictionary<ProjectsTypes, string>();
    private static readonly object projectTypesLock = new object();

    /// <summary>
    /// Thread-safe wrapper: fills the static project type map only once, even when called concurrently.
    /// </summary>
    public static void PairProjectFolderWithEnum(ILogger logger, string documentsFolder)
    {
        lock (projectTypesLock)
        {
            PairProjectFolderWithEnumLocked(logger, documentsFolder);
        }
    }

    private static void PairProjectFolderWithEnumLocked(ILogger logger, string documentsFolder)
    {
        if (projectTypes.FirstToSecond.Count > 0)
        {
            return;
        }

        var p2 = documentsFolder;
        var folders = Directory.GetDirectories(p2, "*", SearchOption.TopDirectoryOnly);
        foreach (var item in folders)
        {
            var fn = Path.GetFileName(item);
            if (fn.EndsWith(SolutionsIndexerStrings.ProjectPostfix))
            {
                ProjectsTypes parameter = ProjectsTypes.None;
                var list = fn.Replace(SolutionsIndexerStrings.ProjectPostfix, string.Empty);
                var l2 = list.Replace("_", string.Empty).Trim();
                switch (l2)
                {
                    case "C++":
                        parameter = ProjectsTypes.Cpp;
                        break;
                    default:
                        parameter = EnumHelper.Parse(l2, ProjectsTypes.None);
                        break;
                }

                if (parameter == ProjectsTypes.None)
                {
                    /* 
                     * This was nonsense. When I create a new folder (which I do often, because I am constantly learning something new)
                     * I must also modify SunamoDevCode. Create a new nuget. 
                     * Then update it in the various apps. I have 3 CommandsToAll* alone. 
                     * Build, copy, only then it works. 
                     * 
                     * Besides, I do not remember using this anywhere.
                     */
                    logger.LogWarning("Can t assign to enum type of folder" + " " + item);
                }
                else
                {
                    projectTypes.Add(parameter, list);
                }
            }
        }

        projectTypes.Add(ProjectsTypes.Cs, "Projects");
    }

    public static List<string> OnlyRealLoadedSolutionsFolders { get; set; } = new List<string>();

    // In key is filename without .csproj, in value is full path
    public static Dictionary<string, List<string>> allCsprojGlobal { get; set; } = new Dictionary<string, List<string>>();
    public static
    async Task<Dictionary<string, List<string>>?>
 AllGlobalCsprojs(ILogger logger, bool listToClipboardInsteadOfThrowEx = false)
    {
        if (allCsprojGlobal.Count == 0)
        {
            foreach (var item in Fwss)
            {
                foreach (var sln in item.GetSolutions(UsedRepository))
                {
                    SolutionFolder.GetCsprojs(logger, sln);
                    foreach (var item2 in sln.ProjectsGetCsprojs)
                    {
                        ResultWithExceptionDC<XmlDocument>? xml = null;
                        xml = 
                            await
                        XmlDocumentsCache.Get(item2);
                        if (MayExcHelper.MayExc(xml.Exc))
                        {
                            continue;
                        }

                        if (xml.Data != null)
                        {
                            DictionaryHelper.AddOrCreate(allCsprojGlobal, Path.GetFileNameWithoutExtension(item2), item2);
                        }
                    }
                }
            }
        }

        var builder = IsAllProjectNamesUnique(listToClipboardInsteadOfThrowEx);
        if (!builder)
        {
            return null;
        }

        return allCsprojGlobal;
    }

    public static List<string> ProjectsWithDuplicateName { get; set; } = new List<string>();

    public static bool IsAllProjectNamesUnique(bool isListToClipboardInsteadOfThrowEx = false)
    {
        StringBuilder? stringBuilder = null;
        if (isListToClipboardInsteadOfThrowEx)
        {
            stringBuilder = new StringBuilder();
        }

        bool result = true;
        foreach (var projectEntry in allCsprojGlobal)
        {
            if (projectEntry.Value.Count > 1)
            {
                if (isListToClipboardInsteadOfThrowEx)
                {
                    foreach (var projectPath in projectEntry.Value)
                    {
                        stringBuilder!.AppendLine(projectPath);
                    }
                }
                else
                {
                    ProjectsWithDuplicateName.Add(Path.GetFileName(projectEntry.Key));
                    for (int i = 1; i < projectEntry.Value.Count; i++)
                    {
                        projectEntry.Value.RemoveAt(i);
                    }
                }

                result = false;
            }
        }

        return result;
    }

    public static SolutionFolder CreateSolutionFolder(ILogger logger, string documentsFolder, SolutionFolderSerialize solutionFolder, PpkOnDriveDC toSelling, string? projName = null)
    {
        return CreateSolutionFolder(logger, documentsFolder, null!, solutionFolder.FullPathFolder, toSelling, projName);
    }
}
