namespace SunamoSolutionsIndexer;

/// <summary>
/// Helper methods for solution indexing, finding, and managing projects within solutions.
/// </summary>
public class SolutionsIndexerHelper
{
    /// <summary>
    /// Finds a solution folder by name. Can also accept web names like apps.sunamo.cz.
    /// </summary>
    /// <param name="name">Solution name or web address to find.</param>
    /// <returns>Matching solution folder or null if not found.</returns>
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
                    if (originName != String.Empty)
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

    /// <summary>
    /// not full path, only name of folder for more accurate deciding
    /// </summary>
    /// <param name = "nameOfFolder"></param>
    public static bool IsTheSolutionsFolder(string nameOfFolder)
    {
        return nameOfFolder.Contains(SolutionsIndexerConsts.ProjectsFolderName) || nameOfFolder == SolutionsIndexerStrings.GitHub || nameOfFolder == SolutionsIndexerStrings.BitBucket;
    }

    /// <summary>
    /// Finds projects as subfolders with optional VS folder removal and name-only mode
    /// </summary>
    /// <param name="isRemovingVsFolders">If true, removes Visual Studio temp folders</param>
    /// <param name="folderPath">Folder path to scan</param>
    /// <param name="isReturningOnlyNames">If true, returns only folder names; if false, returns full paths</param>
    /// <returns>List of project folders</returns>
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

    /// <summary>
    /// Removes Visual Studio temp folders from list
    /// </summary>
    /// <param name="isRemovingVsFolders">If true, removes VS folders</param>
    /// <param name="directories">List to remove folders from</param>
    public static void RemoveVsFolders(bool isRemovingVsFolders, List<string> directories)
    {
        if (isRemovingVsFolders)
        {
            VisualStudioTempFse.FoldersInSolutionDownloaded.ToList().ForEach(folder => CA.RemoveWildcard(directories, folder));
            //
            VisualStudioTempFse.FoldersInProjectDownloaded.ToList().ForEach(folder => CA.RemoveWildcard(directories, folder));
        }
    }

    /// <summary>
    /// Builds a display-friendly solution name by combining parent folder names up to the solutions root.
    /// </summary>
    /// <param name="item">Full path to the solution folder.</param>
    /// <returns>Slash-separated display name for the solution.</returns>
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

    /// <summary>
    /// Gets modules in solution from UserControl/UC/UserControls folders
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="projects">List of project names</param>
    /// <param name="fullPathFolder">Full path to solution folder</param>
    /// <param name="isSelling">If true, returns only selling modules</param>
    /// <param name="toSelling">Selling configuration</param>
    /// <returns>List of module paths</returns>
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

    /// <summary>
    /// Adds modules from path to result list based on selling configuration
    /// result and isSelling parameters are paired
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <param name="path">Path to scan for modules</param>
    /// <param name="slnProject">Solution and project name</param>
    /// <param name="result">List to add modules to</param>
    /// <param name="isSelling">If true, adds only selling modules; if false, adds non-selling modules</param>
    /// <param name="toSelling">Selling configuration</param>
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