namespace SunamoSolutionsIndexer;

public class SolutionsIndexerHelper
{
    // not full path, only name of folder for more accurate deciding
    public static SolutionFolder? SolutionWithName(string name)
    {
        if (FoldersWithSolutions.Fwss.Count > 1)
        {
            System.Diagnostics.Debugger.Break();
        }

        //bool specificNameOfScz = false;
        var originName = string.Empty;

        if (name.Contains(".sunamo.cz"))
        {
            originName = name;
            name = "sunamo.cz";
        }

        foreach (var item in FoldersWithSolutions.Fwss)
        {
            var slns = item.GetSolutions(RepositoryLocal.All);
            //wpf = slns.Where(d => d.NameSolution.StartsWith(name[0].ToString().ToUpper()));

            foreach (var sln in slns)
            {
                if (sln.NameSolution == name)
                {
                    if (originName != string.Empty)
                    {
                        sln.SlnNameWithoutExtension = originName;
                    }
                    return sln;
                }
            }
        }

        //ThisApp.Warning(name + " solution was not found");
        return null;
    }

    public static bool IsTheSolutionsFolder(string nameOfFolder)
    {
        return nameOfFolder.Contains(SolutionsIndexerConsts.ProjectsFolderName) || nameOfFolder == SolutionsIndexerStrings.GitHub || nameOfFolder == SolutionsIndexerStrings.BitBucket;
    }

    public static List<string> ProjectsInSolution(bool isRemovingVsFolders, string folderPath, bool isReturningOnlyNames = true)
    {
        // TODO: Filter auto created files, then uncomment
        List<string> directories = Directory.GetDirectories(folderPath).ToList();
        directories = FS.OnlyNamesNoDirectEdit(directories);
        RemoveVsFolders(isRemovingVsFolders, directories);

        if (!isReturningOnlyNames)
        {
            for (int i = 0; i < directories.Count; i++)
            {
                directories[i] = Path.Combine(folderPath, directories[i]);
            }
        }

        return directories;
    }

    public static void RemoveVsFolders(bool isRemovingVsFolders, List<string> directories)
    {
        if (isRemovingVsFolders)
        {
            VisualStudioTempFse.FoldersInSolutionDownloaded.ToList().ForEach(folder => CA.RemoveWildcard(directories, folder));
            //
            VisualStudioTempFse.FoldersInProjectDownloaded.ToList().ForEach(folder => CA.RemoveWildcard(directories, folder));
        }
    }

    public static string GetDisplayedSolutionName(string item)
    {
        List<string> tokens = new List<string>();
        tokens.Add(Path.GetFileName(item.TrimEnd('\\')));
        while (true)
        {
            item = Path.GetDirectoryName(item)!;
            if (CA.ContainsElement<string>(FoldersWithSolutions.OnlyRealLoadedSolutionsFolders, item!))
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
                tokens.Add(Path.GetFileName(item.TrimEnd('\\')).Replace(SolutionsIndexerConsts.VisualStudio + " ", ""));
                break;
            }

            if (fn == "_")
            {
                break;
            }

            tokens.Add(Path.GetFileName(item.TrimEnd('\\')));
        }

        tokens.Reverse();
        return string.Join("/", tokens.ToArray());
    }

    public static List<string> ModulesInSolution(ILogger logger, List<string> projects, string fullPathFolder, bool isSelling, PpkOnDriveDC toSelling)
    {
        List<string> result = new List<string>();
        var slnName = Path.GetFileName(fullPathFolder);

        foreach (var item in projects)
        {
            var path = Path.Combine(fullPathFolder, Path.GetFileNameWithoutExtension(item));
            var projectName = Path.GetFileNameWithoutExtension(item);

            slnName = Path.GetFileName(fullPathFolder);
            AddModules(logger, isSelling, toSelling, result, slnName, projectName, path, "UserControl");
            slnName = Path.GetFileName(fullPathFolder);
            AddModules(logger, isSelling, toSelling, result, slnName, projectName, path, "UC");
            slnName = Path.GetFileName(fullPathFolder);
            AddModules(logger, isSelling, toSelling, result, slnName, projectName, path, "UserControls");

        }

        return result;
    }

    private static string AddModules(ILogger logger, bool isSelling, PpkOnDriveDC toSelling, List<string> result, string slnName, string projectName, string path, string nameFolder)
    {
        var path2 = Path.Combine(path, nameFolder);
        AddModules(logger, path2, slnName + "\\" + projectName, result, isSelling, toSelling);
        return path2;
    }

    // result and isSelling parameters are paired
    private static void AddModules(ILogger logger, string path, string slnProject, List<string> result, bool isSelling, PpkOnDriveDC toSelling)
    {

        if (Directory.Exists(path))
        {
            var files = FSGetFiles.GetFiles(logger, path, "*.xaml", System.IO.SearchOption.TopDirectoryOnly, new GetFilesArgsDC { TrimA1AndLeadingBs = true });
            for (int i = 0; i < files.Count; i++)
            {
                files[i] = Path.GetFileNameWithoutExtension(files[i]);
            }
            //files = Path.GetFileNamesWoExtension(files);
            foreach (var item in files)
            {
                var module = Path.GetFileName(item);
                var text = slnProject + "\\" + module;
                if (toSelling.Contains(text))
                {
                    if (isSelling)
                    {
                        result.Add(text);
                    }
                }
                else
                {
                    if (!isSelling)
                    {
                        result.Add(text);
                    }
                }

            }

        }
    }
}
