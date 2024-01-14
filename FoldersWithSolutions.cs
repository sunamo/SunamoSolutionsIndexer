using SunamoI18N.Values;

namespace SunamoSolutionsIndexer;

public class FoldersWithSolutions
{
    #region data fields
    /// <summary>
    /// 3-10-23 bylo static ale nevím proč když solutions je instanční
    /// </summary>
    public List<SolutionFolder> solutions = null;
    /// <summary>
    /// D:\Documents
    /// </summary>
    public string documentsFolder = null;
    #endregion
    static Type type = typeof(FoldersWithSolutions);
    static FoldersWithSolutionsInstance _fws = null;
    static FoldersWithSolutionsInstance fws = _fws == null ? FoldersWithSolutionsInstance.Instance : _fws;
    public static Repository usedRepository = Repository.Vs17;

    public static void IdentifyProjectType(string documentsFolder, string solutionFolder, SolutionFolder sf, bool useBp)
    {
        //if (!useBp)
        //{
        //    return;
        //}

        // SolutionFolderSerialize doesn't have InVsFolder or typeProjectFolder
        //sf.InVsFolder = solutionFolder.Contains(SolutionsIndexerStrings.VisualStudio2017);
        //if (sf.InVsFolder)
        //{


        solutionFolder = SHTrim.TrimStart(solutionFolder, documentsFolder);
        var p = SHSplit.SplitChar(solutionFolder, AllChars.bs);
        //var dx = p.IndexOf(SolutionsIndexerStrings.VisualStudio2017);

        var pr = p[0];
        pr = pr.Replace(SolutionsIndexerStrings.ProjectPostfix, string.Empty);
        if (projectTypes._d2.ContainsKey(pr))
        {
            sf.typeProjectFolder = projectTypes._d2[pr];
        }
        else
        {
            ThrowEx.KeyNotFound<string, ProjectsTypes>(projectTypes._d2, "projectTypes._d2", pr);
        }
        //}
    }

    /// <summary>
    /// Složka ve které se má hledat na složku Projects a složky Visual Studia
    ///
    /// přidává se mi zde když volám ctor FoldersWithSolutionsInstance
    ///
    /// pokud nemám sln, zavolat new FoldersWithSolutions(DefaultPaths.vs, null);
    /// </summary>
    public static FoldersWithSolutionsList fwss = new FoldersWithSolutionsList();

    #region ctor
    /// <summary>
    /// A1 = D:\Documents
    /// This class should be instaniate only once and then call reload by needs
    /// A1 toSelling can be null
    /// </summary>
    public FoldersWithSolutions(string documentsFolder, PpkOnDrive toSelling, bool addAlsoSolutions = true)
    {
        // documentsFolder může být null / SE, stejně to poté doplňuji z actualPlatform
        ThrowEx.DirectoryExists(documentsFolder);

        this.documentsFolder = documentsFolder;
        if (addAlsoSolutions)
        {
            solutions = Reload(documentsFolder, toSelling);

            //if (fwss.Count == 0)
            //{
            //    fwss.AddRange(new FoldersWithSolutions() );
            //}
        }


        FoldersWithSolutions.fwss.Add(this);
        //    FoldersWithSolutions.fwss.Add(new FoldersWithSolutionsInstance());


        // Nemůžu použít, FoldersWithSolutions není odvozené od FoldersWithSolutionsInstance
        //fwss.Add(new FoldersWithSolutions(this));
    }

    public FoldersWithSolutions(FoldersWithSolutionsInstance fws2)
    {
        fws = fws2;
        solutions = fws2.solutions;
        documentsFolder = fws2.documentsFolder;
        fwss = fws2.fwss;
        onlyRealLoadedSolutionsFolders = fws2.onlyRealLoadedSolutionsFolders;
    }
    #endregion

    #region Returns solutions in various objects
    public List<SolutionFolderWithFiles> SolutionsWithFiles()
    {
        List<SolutionFolderWithFiles> vr = new List<SolutionFolderWithFiles>();
        foreach (var item in solutions)
        {
            vr.Add(new SolutionFolderWithFiles(item));
        }
        return vr;
    }


    #endregion

    /// <summary>
    /// Get all projects in A1(Visual Studio Projects *) and GitHub folder and insert to global variable solutions
    /// A1 toSelling must be null
    /// </summary>
    /// <param name="documentsFolder"></param>
    public List<SolutionFolder> Reload(string documentsFolder, PpkOnDrive toSelling, bool ignorePartAfterUnderscore = false)
    {
        PairProjectFolderWithEnum();

        // Get all projects in A1(Visual Studio Projects *) and GitHub folder
        List<string> solutionFolders = ReturnAllProjectFolders(documentsFolder /*, Path.Combine(documentsFolder, SolutionsIndexerStrings.GitHubMy)*/);

        List<string> projOnlyNames = new List<string>(solutionFolders.Count);
        var ta = solutionFolders.ToArray();
        var on = FS.OnlyNamesNoDirectEdit(ta);
        projOnlyNames.AddRange(on);
        // Initialize global variable solutions
        solutions = new List<SolutionFolder>(solutionFolders.Count);

        for (int i = 0; i < solutionFolders.Count; i++)
        {
            var solutionFolder = solutionFolders[i];

            SolutionFolder sf = CreateSolutionFolder(solutionFolder, toSelling, projOnlyNames[i]);

            solutions.Add(sf);
        }

        return solutions;
    }

    static TwoWayDictionary<ProjectsTypes, string> projectTypes = new TwoWayDictionary<ProjectsTypes, string>();

    public static void PairProjectFolderWithEnum()
    {
        if (projectTypes._d1.Count > 0)
        {
            return;
        }

        var p2 = DefaultPaths.bp;
        if (!FS.ExistsDirectory(p2))
        {
            return;
        }

        var folders = FS.GetFolders(p2, "*", SearchOption.TopDirectoryOnly);

        foreach (var item in folders)
        {
            var fn = FS.GetFileName(item);
            if (fn.EndsWith(SolutionsIndexerStrings.ProjectPostfix))
            {
                ProjectsTypes p = ProjectsTypes.None;

                var l = fn.Replace(SolutionsIndexerStrings.ProjectPostfix, string.Empty);
                var l2 = l.Replace(AllStrings.lowbar, string.Empty).Trim();
                switch (l2)
                {
                    case "C++":
                        p = ProjectsTypes.Cpp;
                        break;
                    //case "":
                    //    p = ProjectsTypes.Cs;
                    //    break;
                    default:
                        p = EnumHelper.Parse<ProjectsTypes>(l2, ProjectsTypes.None);
                        break;
                }

                if (p == ProjectsTypes.None)
                {
                    ThrowEx.Custom(sess.i18n(XlfKeys.CanTAssignToEnumTypeOfFolder) + " " + item);
                }

                projectTypes.Add(p, l);
            }
        }
        projectTypes.Add(ProjectsTypes.Cs, XlfKeys.Projects);
    }

    public static List<string> onlyRealLoadedSolutionsFolders = new List<string>();

    public static void AllCsprojs()
    {

    }

    /// <summary>
    /// In key is fn without .csproj, in value is full path
    /// </summary>
    public static Dictionary<string, List<string>> allCsprojGlobal = new Dictionary<string, List<string>>();

#if ASYNC
    public static
#if ASYNC
    async Task<Dictionary<string, List<string>>>
#else
      Dictionary<string, List<string>>
#endif
 AllGlobalCsprojs(bool listToClipboardInsteadOfThrowEx = false)
#else
    public static Dictionary<string, List<string>> AllGlobalCsprojs(bool listToClipboardInsteadOfThrowEx = false)
#endif
    {
        if (allCsprojGlobal.Count == 0)
        {
            foreach (var item in fwss)
            {
                foreach (var sln in item.Solutions(usedRepository))
                {
                    SolutionFolder.GetCsprojs(sln);
                    foreach (var item2 in sln.projectsGetCsprojs)
                    {
                        ResultWithException<XmlDocument> xml = null;

                        xml =
#if ASYNC
                            await
#endif
                         XmlDocumentsCache.Get(item2);

                        if (MayExtHelper.XmlDocument(xml))
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
        var b = IsAllProjectNamesUnique(listToClipboardInsteadOfThrowEx);
        if (!b)
        {
            return null;
        }
        return allCsprojGlobal;
    }

    public static List<string> projectsWithDuplicateName = new List<string>();

    public static bool IsAllProjectNamesUnique(bool listToClipboardInsteadOfThrowEx = false)
    {
        StringBuilder sb = null;

        if (listToClipboardInsteadOfThrowEx)
        {
            sb = new StringBuilder();
        }

        bool vr = true;
        foreach (var item in allCsprojGlobal)
        {
            if (item.Value.Count > 1)
            {
                if (listToClipboardInsteadOfThrowEx)
                {
                    foreach (var item2 in item.Value)
                    {
                        sb.AppendLine(item2);
                    }
                }
                else
                {
                    projectsWithDuplicateName.Add(FS.GetFileName(item.Key));

                    for (int i = 1; i < item.Value.Count; i++)
                    {
                        item.Value.RemoveAt(i);
                    }
                    //ThrowEx.MoreThanOneElement("item.Value", item.Value.Count, "Key : "+ item.Key);
                }
                vr = false;
            }
        }

        if (listToClipboardInsteadOfThrowEx)
        {
            ClipboardHelper.SetText(sb.ToString());
        }


        return vr;
    }

    public static SolutionFolder CreateSolutionFolder(SolutionFolderSerialize solutionFolder, PpkOnDrive toSelling, string projName = null)
    {
        return CreateSolutionFolder(null, solutionFolder.fullPathFolder, toSelling, projName);
    }

    /// <summary>
    /// toSelling can be null
    /// </summary>
    /// <param name="solutionFolder"></param>
    /// <param name="toSelling"></param>
    /// <param name="projName"></param>
    /// <returns></returns>
    public static SolutionFolder CreateSolutionFolder(string solutionFolder, PpkOnDrive toSelling, string projName = null)
    {
        return CreateSolutionFolder(null, solutionFolder, toSelling, projName);
    }

    /// <summary>
    /// toSelling can be null
    /// </summary>
    /// <param name="sfs"></param>
    /// <param name="solutionFolder"></param>
    /// <param name="toSelling"></param>
    /// <param name="projName"></param>
    /// <returns></returns>
    public static SolutionFolder CreateSolutionFolder(SolutionFolderSerialize sfs, string solutionFolder, PpkOnDrive toSelling, string projName = null)
    {

        if (projName == null)
        {
            projName = FS.GetFileName(solutionFolder);
        }
        SolutionFolder sf = null;
        if (sfs != null)
        {
            sf = new SolutionFolder(sfs);
        }
        else
        {
            sf = new SolutionFolder();
        }
        sf.repository = RepositoryFromFullPath(solutionFolder);
        IdentifyProjectType(solutionFolder, sf);
        sf.displayedText = GetDisplayedName(solutionFolder);
        sf.fullPathFolder = solutionFolder;

        // Nevím zda je to nutné tak jsem to zakomentoval aby to bylo rychlejší
        //sf.projects = new DebugCollection<string>( SolutionsIndexerHelper.ProjectsInSolution(true, sf.fullPathFolder));
        //sf.SourceOfProjects = SourceOfProjects.ProjectsInSolution;

        sf.UpdateModules(toSelling);
        sf.nameSolutionWithoutDiacritic = SH.TextWithoutDiacritic(projName);
        return sf;
    }

    protected static void IdentifyProjectType(string solutionFolder, SolutionFolder sf)
    {
        // SolutionFolderSerialize doesn't have InVsFolder or typeProjectFolder
        sf.InVsFolder = solutionFolder.Contains(SolutionsIndexerStrings.VisualStudio2017);
        if (sf.InVsFolder)
        {


            solutionFolder = SHTrim.TrimStart(solutionFolder, DefaultPaths.bp);
            var p = SHSplit.SplitChar(solutionFolder, AllChars.bs);
            //var dx = p.IndexOf(SolutionsIndexerStrings.VisualStudio2017);

            var pr = p[0];
            pr = pr.Replace(SolutionsIndexerStrings.ProjectPostfix, string.Empty);
            if (projectTypes._d2.ContainsKey(pr))
            {
                sf.typeProjectFolder = projectTypes._d2[pr];
            }
            else
            {
                ThrowEx.KeyNotFound<string, ProjectsTypes>(projectTypes._d2, "projectTypes._d2", pr);
            }
        }
    }

    private static Repository RepositoryFromFullPath(string fullPathFolder)
    {
        if (fullPathFolder.Contains(SolutionsIndexerStrings.VisualStudio2017))
        {
            return Repository.Vs17;
        }
        else if (fullPathFolder.Contains(SolutionsIndexerConsts.BitBucket))
        {
            return Repository.BitBucket;
        }
        else if (fullPathFolder.Contains(DefaultPaths.cRepos))
        {
            return Repository.Vs17;
        }
        else if (fullPathFolder.Contains(DefaultPaths.bpVps))
        {
            return Repository.Vs17;
        }
        ThrowEx.NotImplementedCase(fullPathFolder);
        return Repository.All;
    }



    /// <summary>
    /// Get name based on relative but always fully recognized project
    /// </summary>
    /// <param name="item"></param>
    private static string GetDisplayedName(string item)
    {
        return SolutionsIndexerHelper.GetDisplayedSolutionName(item);
    }

    public List<SolutionFolder> SolutionsUap(IList<string> skipThese = null)
    {
        var slns = Solutions(Repository.Vs17, false, skipThese);
        var uap = slns.Where(d => d.fullPathFolder.Contains(@"\_Uap\")).ToList();
        return uap;
    }

    /// <summary>
    /// A1 not have to be wildcard
    /// </summary>
    /// <param name="vs17"></param>
    /// <param name="toFind"></param>
    /// <returns></returns>
    public IList<SolutionFolder> SolutionsWildcard(Repository r, string mayWildcard)
    {
        var result = Solutions(r);


        for (int i = result.Count - 1; i >= 0; i--)
        {
            var ns = result[i].nameSolution;
            if (!SH.MatchWildcard(ns, mayWildcard))
            {
                result.RemoveAt(i);
            }
        }


        return result;
    }

    /// <summary>
    /// Simple returns global variable solutions
    /// Exclude from SolutionsIndexerConsts.SolutionsExcludeWhileWorkingOnSourceCode if Debugger is attached and !A2
    /// A3 - can use wildcard
    /// </summary>
    public List<SolutionFolder> Solutions(Repository r, bool loadAll = true, IList<string> skipThese = null, ProjectsTypes cs = ProjectsTypes.Cs)
    {
        var result = new List<SolutionFolder>(solutions);

        if (r != Repository.All)
        {
            result.RemoveAll(d => d.repository != r);
        }

        List<string> skip = null;
        if (skipThese != null)
        {
            skip = skipThese.ToList();
        }
        else
        {
            skip = new List<string>();
        }

        if (!loadAll)
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

        var l = result.Count;
        for (int i = result.Count - 1; i >= 0; i--)
        {
            var it = result[i];
            foreach (var item in dict)
            {
                if (item.Value.IsMatch(it.nameSolution))
                {
                    result.RemoveAt(i);
                    break;
                }
            }
        }


        var l2 = result.Count;

        //result.RemoveAll(d => CA.IsEqualToAnyElement(d.nameSolution, skip));

        ////////DebugLogger.Instance.WriteCount("Solutions in " + documentsFolder, solutions);
        return result;
    }

    /// <summary>
    /// Return fullpath for all folder recursively - specific and ordi
    /// </summary>
    /// <param name="folderWithVisualStudioFolders"></param>
    /// <param name="alsoAdd"></param>
    private List<string> ReturnAllProjectFolders(string folderWithVisualStudioFolders, params string[] alsoAdd)
    {
        List<string> projs = new List<string>();
        var bp = BasePathsHelper.actualPlatform;

        if (FS.ExistsDirectory(bp))
        {
            if (DefaultPaths.bpVps == bp)
            {
                AddProjectsFolder(projs, bp);
            }
            else
            {
                List<string> visualStudioFolders = CAG.ToList<string>(bp); // FS.GetFolders(folderWithVisualStudioFolders, VpsHelperSunamo.IsQ ? "_" : SolutionsIndexerStrings.VisualStudio2017, SearchOption.TopDirectoryOnly));
                foreach (var item in alsoAdd)
                {
                    AddProjectsFolder(projs, item);
                }
                foreach (var item in visualStudioFolders)
                {
                    List<string> slozkySJazyky = null;
                    List<string> slozkySJazykyOutsideVs17 = new List<string>();
                    try
                    {
                        slozkySJazyky = FS.GetFolders(item);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                    slozkySJazykyOutsideVs17.Leading(Path.Combine(folderWithVisualStudioFolders.Replace("E:\\", "D:\\"), SolutionsIndexerConsts.BitBucket));

                    foreach (var item2 in slozkySJazyky)
                    {
                        #region New
                        string pfn = FS.GetFileName(item2);
                        if (SolutionsIndexerHelper.IsTheSolutionsFolder(pfn))
                        {
                            AddProjectsFolder(projs, item2);
                        }
                        #endregion
                    }

                    foreach (var item2 in slozkySJazykyOutsideVs17)
                    {
                        #region New
                        if (FS.ExistsDirectory(item2))
                        {
                            string pfn = FS.GetFileName(item2);

                            AddProjectsFolder(projs, item2);
                        }
                        #endregion
                    }
                }
            }
        }
        else if (VpsHelperSunamo.IsVps)
        {
            return new List<string>();
        }
        else
        {
            ThrowEx.Custom(folderWithVisualStudioFolders + " not exists, therefore will be return none slsn");
        }
        CAChangeContent.ChangeContent0(null, projs, SH.FirstCharUpper);
        return projs;
    }

    public static Tuple<List<string>, List<string>> ReturnNormalAndSpecialFolders(string sloz)
    {
        List<string> special = null;
        List<string> normal = null;
        ReturnNormalAndSpecialFolders(sloz, out special, out normal);
        return new Tuple<List<string>, List<string>>(normal, special);
    }

    /// <summary>
    /// Projde nerek slozky v A1 a vrati mi do A2 ty ktere zacinali na _ a do A3 zbytek.
    /// </summary>
    /// <param name="sloz"></param>
    /// <param name="Specialni"></param>
    /// <param name="normal2"></param>
    public static void ReturnNormalAndSpecialFolders(string sloz, out List<string> spec, out List<string> normal)
    {
        spec = new List<string>();
        normal = new List<string>();
        try
        {
            var slo = FS.GetFolders(sloz);
            foreach (string var in slo)
            {
                string nazev = FS.GetFileName(var);
                if (nazev.StartsWith(AllStrings.lowbar))
                {
                    spec.Add(var);
                }
                else
                {
                    normal.Add(var);
                }
            }
        }
        catch (Exception ex)
        {


        }
    }

    /// <summary>
    /// Find out usuall folder and specific (which starting on _) and process then to any level
    /// </summary>
    /// <param name="proj"></param>
    /// <param name="slozka"></param>
    void AddProjectsFolder(List<string> proj, string slozka)
    {
        List<string> spec, norm;
        ReturnNormalAndSpecialFolders(slozka, out spec, out norm);

        norm = CA.EnsureBackslash(norm);
        proj.AddRange(norm);
        foreach (string var2 in spec)
        {
            AddProjectsFolder(proj, var2);
        }
    }

    public static List<string> FullPathFolders(Repository usedRepository, List<string> returnOnlyThese = null)
    {
        Dictionary<string, SolutionFolder> sf = null;

        return FullPathFolders(usedRepository, sf, returnOnlyThese);
    }

    public static List<string> FullPathFolders(Repository usedRepository, Dictionary<string, SolutionFolder> sf, List<string> returnOnlyThese = null)
    {
        List<string> ls = new List<string>();
        foreach (var item in fwss)
        {
            var slns = item.Solutions(usedRepository);

            foreach (var sln in slns)
            {
                if (returnOnlyThese != null)
                {
#if DEBUG
                    if (sln.nameSolution.Contains("OnlyWeb"))
                    {

                    }
#endif
                    if (!returnOnlyThese.Contains(sln.nameSolution))
                    {
                        continue;
                    }
                }

                if (sf != null)
                {
                    sf.Add(sln.fullPathFolder, sln);
                }
                ls.Add(sln.fullPathFolder);
            }
        }
        return ls;
    }
}
