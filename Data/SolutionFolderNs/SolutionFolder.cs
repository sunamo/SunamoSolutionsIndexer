
namespace SunamoSolutionsIndexer.Data.SolutionFolderNs;
using SunamoSolutionsIndexer.Args;
using SunamoSolutionsIndexer.Interfaces;


public interface IProjectType
{
    ProjectsTypes typeProjectFolder { get; set; }
}




public class SolutionFolder : SolutionFolderSerialize, IProjectType, ISolutionFolder
{
    public static Type type = typeof(SolutionFolder);

    /// <summary>
    /// Return csproj full paths in subfolders of A1 (one depth)
    /// Must use as A1 SolutionFolder, coz in CreateSolutionFolder is filled projects variable
    ///
    /// From every folder is taked all csproj => even only file name is shown is good keep also upfolder
    ///
    /// A2 whether return only names to csproj files without path
    /// </summary>
    /// <param name="sf"></param>
    /// <param name="onlyNames"></param>
    public static void GetCsprojs(SolutionFolder sf, GetCsprojsArgs a = null)
    {
        if (a == null)
        {
            a = new GetCsprojsArgs();
        }

        // && sf.projectsGetCsprojs.Count == 0 - for better performance, when will have zero, its not VS sln
        if (sf.projectsGetCsprojs == null || a.forceReload)
        {

#if DEBUG

            if (sf.fullPathFolder.TrimEnd(AllCharsSE.bs) == @"\monoConsoleSqlClient")
            {

            }
#endif

            List<string> csprojs = new List<string>();
            var projectsFolder = SolutionsIndexerHelper.ProjectsInSolution(true, sf.fullPathFolder, false);

            foreach (var projectFolder in projectsFolder)
            {
                var files = FS.GetFiles(projectFolder, FS.MascFromExtension(AllExtensions.csproj), SearchOption.TopDirectoryOnly, new GetFilesArgs { _trimA1AndLeadingBs = a.onlyNames });
                foreach (var item in files)
                {
                    csprojs.Add(item);
                }
            }

#if DEBUG
            if (sf.fullPathFolder.TrimEnd(AllCharsSE.bs) == @"\monoConsoleSqlClient")
            {

            }
#endif


            sf.projectsGetCsprojs = new DebugCollection<string>(csprojs);
            sf.SourceOfProjects = SourceOfProjects.GetCsprojs;
        }
        else
        {
            if (sf.projectsGetCsprojs == null)
            {
                sf.projectsGetCsprojs = new List<string>();
            }
        }
    }

    public SolutionFolder(SolutionFolderSerialize t)
    {
        displayedText = t.displayedText;
        _fullPathFolder = t._fullPathFolder;
        _nameSolution = t._nameSolution;
        projectFolder = t.projectFolder;
        slnFullPath = t.slnFullPath;
        if (t.GetType() == type)
        {
            var t2 = (SolutionFolder)t;
            slnNameWoExt = t2.slnNameWoExt;
        }

    }

    /// <summary>
    /// C# Projects
    /// PHP PHP_Projects etc.
    /// </summary>
    public ProjectsTypes typeProjectFolder { get; set; } = ProjectsTypes.None;

    public void UpdateModules(PpkOnDrive toSelling)
    {
        if (toSelling != null)
        {
            modulesSelling = SolutionsIndexerHelper.ModulesInSolution(projectsInSolution, fullPathFolder, true, toSelling);
            modulesNotSelling = SolutionsIndexerHelper.ModulesInSolution(projectsInSolution, fullPathFolder, false, toSelling);
        }
    }

    public SolutionFolder()
    {
    }

    /// <summary>
    /// SolutionFolder.GetCsprojs. SolutionsIndexerHelper.ProjectsInSolution
    /// </summary>
    public SourceOfProjects SourceOfProjects;
    List<string> _projects = new List<string>();

    /// <summary>
    /// Only name without path
    /// Is filled in ctor with CreateSolutionFolder()
    /// Only subfolders. csproj files must be find out manually
    /// Csproj are available to get with APSH.GetCsprojs()
    /// </summary>
    public List<string> projectsInSolution
    {
        get => _projects;
        set => _projects = value;
    }

    List<string> _projectsGetCsprojs = null;

    /// <summary>
    /// SolutionFolder.GetCsprojs
    ///
    /// </summary>
    public List<string> projectsGetCsprojs
    {
        get
        {
            return _projectsGetCsprojs;
        }
        set
        {
            if (fullPathFolder.Contains("Mixed") && value.Count == 0)
            {

            }
            _projectsGetCsprojs = value;
        }
    }

    /// <summary>
    /// In format solution name\project name\module name
    /// </summary>
    public List<string> modulesSelling = new List<string>();
    /// <summary>
    /// In format solution name\project name\module name
    /// </summary>
    public List<string> modulesNotSelling = new List<string>();

    public string nameSolutionWithoutDiacritic = "";

    /// <summary>
    ///
    /// </summary>
    public int countOfImages = 0;

    public bool InVsFolder = false;
    public Repository repository;
    public string slnNameWoExt = null;

    public override string ToString()
    {
        if (countOfImages != 0)
        {
            return displayedText + " (" + countOfImages.ToString() + " images)";
        }
        return displayedText;
    }

    public static bool operator >(SolutionFolder a, SolutionFolder b)
    {
        if (a.countOfImages > b.countOfImages)
        {
            return true;
        }
        return false;
    }

    public static bool operator <(SolutionFolder a, SolutionFolder b)
    {
        if (a.countOfImages < b.countOfImages)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// A3 = jestli už jsem vytvořil sln s project distinctions (.Wpf, .Cmd atd.)
    /// 
    /// </summary>
    /// <param name="sln"></param>
    /// <param name="projectDistinction"></param>
    /// <param name="standaloneSlnForProject"></param>
    /// <param name="addProtectedWhenSelling"></param>
    /// <returns></returns>
    public string ExeToRelease(SolutionFolder sln, string projectDistinction, bool standaloneSlnForProject, bool addProtectedWhenSelling = false, bool publish = false)
    {
#if DEBUG
        if (sln.nameSolution.Contains("GitForDebug") || projectDistinction.Contains("Wpf"))
        {

        }
#endif
        // zde můžu přiřadit jen ty co skutečně existují
        string existingExeReleaseFolder = null;
        // 
        var solutionFolder = sln.fullPathFolder.TrimEnd(AllCharsSE.bs);

        var exeName = sln.nameSolution;
        string exeNameWithExt = exeName + AllExtensions.exe;

        var projectFolderPath = Path.Combine(solutionFolder, exeName);

        if (!Directory.Exists(projectFolderPath))
        {
            return null;
        }

        var bp = Path.Combine(projectFolderPath, @"bin\");

        string p1 = null;
#if DEBUG
        if (sln.nameSolution.Contains("ConsoleApp1") || projectDistinction.Contains("Wpf"))
        {

        }
#endif

        #region MyRegion
        var baseReleaseFolder = Path.Combine(projectFolderPath, @"bin\Release\");
        var net7 = Path.Combine(baseReleaseFolder, "net8.0\\");
        var net7Windows = Path.Combine(baseReleaseFolder, "net8.0-windows\\");

        if (publish)
        {
            net7 += "win-x64\\publish\\";
            net7Windows += "win-x64\\publish\\";
        }

        var b1 = Directory.Exists(net7);
        var b2 = Directory.Exists(net7Windows);

        string exePath = null;

        if (b1)
        {
            exePath = Path.Combine(net7, exeName + ".exe");
            if (File.Exists(exePath))
            {
                existingExeReleaseFolder = net7;
            }
            else
            {
                existingExeReleaseFolder = FindExistingFolderWithRightArchitecture(net7, exeNameWithExt);
            }
        }
        if (b2 && existingExeReleaseFolder == null)
        {
            exePath = Path.Combine(net7Windows, exeName + ".exe");
            if (File.Exists(exePath))
            {
                existingExeReleaseFolder = net7Windows;
            }
            else
            {
                existingExeReleaseFolder = FindExistingFolderWithRightArchitecture(net7Windows, exeNameWithExt);
            }
        }
        #endregion

        // Kontroluje mi pouze na cestu zda existuje. soubor jako takový nemusí existovat
        //if (File.Exists(net4))
        //{
        //    return null;
        //}

#if DEBUG
        if (sln.nameSolution.Contains("ConsoleApp1") || projectDistinction.Contains("Wpf"))
        {

        }
#endif

        if (existingExeReleaseFolder == null)
        {
            return null;
        }

        var result = Path.Combine(existingExeReleaseFolder, exeNameWithExt);
        return result;
    }

    private string FindExistingFolderWithRightArchitecture(string net7, string exeNameWithExt)
    {
        // https://learn.microsoft.com/en-us/dotnet/core/rid-catalog
        var maybe = Path.Combine(net7, "win-x64", exeNameWithExt);

        if (File.Exists(maybe))
        {
            return FSSE.GetDirectoryName(maybe);
        }

        maybe = Path.Combine(net7, "win-x86", exeNameWithExt);

        if (File.Exists(maybe))
        {
            return FSSE.GetDirectoryName(maybe);
        }

        return null;
    }

    /// <summary>
    /// Working
    /// </summary>
    public bool HaveGitFolder()
    {
        var f = Path.Combine(fullPathFolder, VisualStudioTempFse.gitFolderName);
        bool vr = Directory.Exists(f);

        return vr;
    }
}
