namespace SunamoSolutionsIndexer;

public partial class SolutionsIndexerHelper
{
    /// <summary>
    /// Can enter also name of web (apps.sunamo.cz etc.)
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static SolutionFolder SolutionWithName(string name)
    {
        IList<SolutionFolder> wpf = null;

        if (FoldersWithSolutions.fwss.Count > 1)
        {
            Debugger.Break();
        }

        //bool specificNameOfScz = false;
        var originName = string.Empty;

        if (name.Contains(".sunamo.cz"))
        {
            originName = name;
            name = "sunamo.cz";
        }

        foreach (var item in FoldersWithSolutions.fwss)
        {
            var slns = item.Solutions(Repository.All);
            //wpf = slns.Where(d => d.nameSolution.StartsWith(name[0].ToString().ToUpper()));

            foreach (var sln in slns)
            {
                if (sln.nameSolution == name)
                {
                    if (originName != String.Empty)
                    {
                        sln.slnNameWoExt = originName;
                    }
                    return sln;
                }
            }
        }

        ThisApp.Warning(name + " solution was not found");
        return null;
    }

    /// <summary>
    /// not full path, only name of folder for more accurate deciding
    /// </summary>
    /// <param name = "nameOfFolder"></param>
    public static bool IsTheSolutionsFolder(string nameOfFolder)
    {
        return nameOfFolder.Contains(SolutionsIndexerConsts.ProjectsFolderName) || nameOfFolder == SolutionsIndexerStrings.GitHub || nameOfFolder == SolutionsIndexerStrings.BitBucket;
    }

    /// <summary>
    /// Find as subfolders. Can remove VS folders and return only names
    /// </summary>
    /// <param name="removeVsFolders"></param>
    /// <param name="fp"></param>
    /// <param name="onlynames"></param>
    /// <returns></returns>
    public static List<string> ProjectsInSolution(bool removeVsFolders, string fp, bool onlynames = true)
    {
        // TODO: Filter auto created files, then uncomment
        List<string> d = Directory.GetDirectories(fp).ToList();
        d = FS.OnlyNamesNoDirectEdit(d);
        RemoveVsFolders(removeVsFolders, d);

        if (!onlynames)
        {
            for (int i = 0; i < d.Count; i++)
            {
                d[i] = Path.Combine(fp, d[i]);
            }
        }

        return d;
    }

    public static void RemoveVsFolders(bool removeVsFolders, List<string> d)
    {
        if (removeVsFolders)
        {
            VisualStudioTempFse.foldersInSolutionDownloaded.ToList().ForEach(folder => CA.RemoveWildcard(d, folder));
            //
            VisualStudioTempFse.foldersInProjectDownloaded.ToList().ForEach(folder => CA.RemoveWildcard(d, folder));
        }
    }

    public static string GetDisplayedSolutionName(string item)
    {
        List<string> tokens = new List<string>();
        tokens.Add(Path.GetFileName(item.TrimEnd(AllChars.bs)));
        while (true)
        {
            item = Path.GetDirectoryName(item);
            if (CA.ContainsElement<string>(FoldersWithSolutions.onlyRealLoadedSolutionsFolders, item))
            {
                break;
            }

            if (string.IsNullOrEmpty(item))
            {
                break;
            }

            var fn = Path.GetFileName(item);
            if (fn.StartsWith(SolutionsIndexerConsts.VisualStudio + " "))
            {
                tokens.Add(Path.GetFileName(item.TrimEnd(AllChars.bs)).Replace(SolutionsIndexerConsts.VisualStudio + " ", ""));
                break;
            }

            if (fn == AllStrings.lowbar)
            {
                break;
            }

            tokens.Add(Path.GetFileName(item.TrimEnd(AllChars.bs)));
        }

        tokens.Reverse();
        return string.Join(AllChars.slash, tokens.ToArray());
    }

    public static List<string> ModulesInSolution(List<string> projects, string fullPathFolder, bool selling, PpkOnDrive toSelling)
    {
        List<string> result = new List<string>();
        var slnName = Path.GetFileName(fullPathFolder);

        foreach (var item in projects)
        {
            var path = Path.Combine(fullPathFolder, Path.GetFileNameWithoutExtension(item));
            var projectName = Path.GetFileNameWithoutExtension(item);

            slnName = Path.GetFileName(fullPathFolder);
            AddModules(selling, toSelling, result, slnName, projectName, path, "UserControl");
            slnName = Path.GetFileName(fullPathFolder);
            AddModules(selling, toSelling, result, slnName, projectName, path, "UC");
            slnName = Path.GetFileName(fullPathFolder);
            AddModules(selling, toSelling, result, slnName, projectName, path, "UserControls");

        }

        return result;
    }

    private static string AddModules(bool selling, PpkOnDrive toSelling, List<string> result, string slnName, string projectName, string path, string nameFolder)
    {
        var path2 = Path.Combine(path, nameFolder);
        AddModules(path2, slnName + AllStrings.bs + projectName, result, selling, toSelling);
        return path2;
    }

    /// <summary>
    /// result a selling are pairing.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="SlnProject"></param>
    /// <param name="result"></param>
    /// <param name="selling"></param>
    /// <param name="toSelling"></param>
    private static void AddModules(string path, string SlnProject, List<string> result, bool selling, PpkOnDrive toSelling)
    {

        if (Directory.Exists(path))
        {
            var files = FS.GetFiles(path, FS.MascFromExtension(AllExtensions.xaml), System.IO.SearchOption.TopDirectoryOnly, new GetFilesArgs { _trimA1AndLeadingBs = true });
            for (int i = 0; i < files.Count; i++)
            {
                files[i] = Path.GetFileNameWithoutExtension(files[i]);
            }
            //files = Path.GetFileNamesWoExtension(files);
            foreach (var item in files)
            {
                var module = Path.GetFileName(item);
                var s = SlnProject + AllStrings.bs + module;
                if (toSelling.Contains(s))
                {
                    if (selling)
                    {
                        result.Add(s);
                    }
                }
                else
                {
                    if (!selling)
                    {
                        result.Add(s);
                    }
                }

            }

        }
    }
}
