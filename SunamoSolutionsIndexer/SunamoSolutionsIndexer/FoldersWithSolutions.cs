namespace SunamoSolutionsIndexer;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class FoldersWithSolutions
{
    // EN: Was static on 3-10-23 but don't know why when solutions is instance field
    // CZ: 3-10-23 bylo static ale nevím proč když solutions je instanční
    public List<SolutionFolder> Solutions { get; set; } = null!;

    // EN: Path to documents folder, e.g., D:\Documents
    // CZ: D:\Documents
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

    // EN: Folder where to search for Projects folder and Visual Studio folders. Added here when calling FoldersWithSolutions ctor. If no sln, call new FoldersWithSolutions(BasePathsHelper.Vs, null);
    // CZ: Složka ve které se má hledat na složku Projects a složky Visual Studia. Přidává se mi zde když volám ctor FoldersWithSolutions. Pokud nemám sln, zavolat new FoldersWithSolutions(BasePathsHelper.Vs, null);
    public static FoldersWithSolutionsList Fwss { get; set; } = new FoldersWithSolutionsList();
    public static void InsertIntoFwss(ILogger logger, string documentsFolder, PpkOnDriveDC toSelling, bool addAlsoSolutions = true)
    {
        new FoldersWithSolutions(logger, documentsFolder, toSelling, addAlsoSolutions);
    }

    // A1 = D:\Documents
    // This class should be instaniate only once and then call reload by needs
    // A1 toSelling can be null
    public FoldersWithSolutions(ILogger logger, string documentsFolder, PpkOnDriveDC toSelling, bool addAlsoSolutions = true)
    {
        // documentsFolder může být null / SE, stejně to poté doplňuji z actualPlatform
        ThrowEx.DirectoryExists(documentsFolder);
        this.DocumentsFolder = documentsFolder;
        if (addAlsoSolutions)
        {
            Solutions = Reload(logger, documentsFolder, toSelling);
        //if (Fwss.Count == 0)
        //{
        //    Fwss.AddRange(new FoldersWithSolutions() );
        //}
        }

        Fwss.Add(this);
    //    FoldersWithSolutions.Fwss.Add(new FoldersWithSolutions());
    // Nemůžu použít, FoldersWithSolutions není odvozené od FoldersWithSolutions
    //Fwss.Add(new FoldersWithSolutions(this));
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
        // EN: Remove trailing backslashes before extracting names
        // CZ: Odstraň trailing backslashe před extrakcí názvů
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
    public static void PairProjectFolderWithEnum(ILogger logger, string documentsFolder)
    {
        if (projectTypes.FirstToSecond.Count > 0)
        {
            return;
        }

        var p2 = documentsFolder;
        //var p2 = BasePathsHelper.bp;
        //if (!Directory.Exists(p2))
        //{
        //    return;
        //}
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
                    //case "":
                    //    parameter = ProjectsTypes.Cs;
                    //    break;
                    default:
                        parameter = EnumHelper.Parse(l2, ProjectsTypes.None);
                        break;
                }

                if (parameter == ProjectsTypes.None)
                {
                    /* 
                     * Toto byl nesmysl. Když vytvořím novou složku (což vytvářím často, protože se furt něco nového učím)
                     * musím upravit i SunamoDevCode. Vytvořit nový nuget. 
                     * Pak ho updatovat do různých apps. Jen CommandsToAll* mám 3. 
                     * Zbuildit, zkopírovat, teprve pak to funguje. 
                     * 
                     * Navíc si nepamatuji že bych toto kdekoliv použil.
                     */
                    logger.LogWarning(Translate.FromKey(XlfKeys.CanTAssignToEnumTypeOfFolder) + " " + item);
                }
                else
                {
                    projectTypes.Add(parameter, list);
                }
            }
        }

        projectTypes.Add(ProjectsTypes.Cs, XlfKeys.Projects);
    }

    public static List<string> OnlyRealLoadedSolutionsFolders { get; set; } = new List<string>();

    // EN: In key is filename without .csproj, in value is full path
    // CZ: V klíči je jméno souboru bez .csproj, v hodnotě je úplná cesta
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
