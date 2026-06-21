namespace SunamoDevCode.SunamoSolutionsIndexer.Data.SolutionFolderNs;

/// <summary>
/// Represents a solution folder with projects and modules.
/// </summary>
public partial class SolutionFolder : SolutionFolderSerialize, ISolutionFolder
{
    /// <summary>
    /// Gets the type of SolutionFolder class.
    /// </summary>
    public new static Type Type { get; } = typeof(SolutionFolder);
    /// <summary>
    /// Returns csproj full paths in subfolders of solution folder (one depth).
    /// Must use SolutionFolder, because in CreateSolutionFolder is filled projects variable.
    /// From every folder all csproj files are taken.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="solutionFolder">Solution folder to process.</param>
    /// <param name="args">Arguments for getting csproj files.</param>
    public static void GetCsprojs(ILogger logger, SolutionFolder solutionFolder, GetCsprojsArgs? args = null)
    {
        if (args == null)
        {
            args = new GetCsprojsArgs();
        }

        if (solutionFolder.ProjectsGetCsprojs == null || args.ForceReload)
        {
            List<string> csprojs = new List<string>();
            var projectsFolder = SolutionsIndexerHelper.ProjectsInSolution(true, solutionFolder.FullPathFolder, false);
            foreach (var projectFolder in projectsFolder)
            {
                var files = FSGetFiles.GetFiles(logger, projectFolder, "*.csproj", SearchOption.TopDirectoryOnly, new GetFilesArgsDC { TrimA1AndLeadingBs = args.OnlyNames });
                foreach (var item in files)
                {
                    csprojs.Add(item);
                }
            }

            solutionFolder.ProjectsGetCsprojs = new List<string>(csprojs);
            solutionFolder.SourceOfProjects = SourceOfProjects.GetCsprojs;
        }
        else
        {
            if (solutionFolder.ProjectsGetCsprojs == null)
            {
                solutionFolder.ProjectsGetCsprojs = new List<string>();
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionFolder"/> class from serialized data.
    /// </summary>
    /// <param name="source">Source solution folder data.</param>
    public SolutionFolder(SolutionFolderSerialize source)
    {
        DisplayedText = source.DisplayedText;
        _FullPathFolder = source._FullPathFolder;
        _NameSolution = source._NameSolution;
        projectFolder = source.projectFolder;
        slnFullPath = source.slnFullPath;
        if (source.GetType() == Type)
        {
            var sourceSolutionFolder = (SolutionFolder)source;
            SlnNameWithoutExtension = sourceSolutionFolder.SlnNameWithoutExtension;
        }
    }

    /// <summary>
    /// Gets or sets the type of project folder (C# Projects, PHP PHP_Projects, etc.).
    /// </summary>
    public ProjectsTypes TypeProjectFolder { get; set; } = ProjectsTypes.None;

    /// <summary>
    /// Updates modules in the solution.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="toSelling">PPK on drive for selling modules.</param>
    public void UpdateModules(ILogger logger, PpkOnDriveDC toSelling)
    {
        if (toSelling != null)
        {
            ModulesSelling = SolutionsIndexerHelper.ModulesInSolution(logger, ProjectsInSolution, FullPathFolder, true, toSelling);
            ModulesNotSelling = SolutionsIndexerHelper.ModulesInSolution(logger, ProjectsInSolution, FullPathFolder, false, toSelling);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionFolder"/> class.
    /// </summary>
    public SolutionFolder()
    {
    }

    /// <summary>
    /// Gets or sets the source of projects (SolutionFolder.GetCsprojs or SolutionsIndexerHelper.ProjectsInSolution).
    /// </summary>
    public SourceOfProjects SourceOfProjects { get; set; }

    private List<string> _projects = new List<string>();

    /// <summary>
    /// Gets or sets projects in solution.
    /// Only name without path.
    /// Is filled in constructor with CreateSolutionFolder().
    /// Only subfolders. Csproj files must be found out manually.
    /// Csproj are available to get with GetCsprojs().
    /// </summary>
    public List<string> ProjectsInSolution { get => _projects; set => _projects = value; }

    private List<string>? _projectsGetCsprojs = null;

    /// <summary>
    /// Gets or sets projects from SolutionFolder.GetCsprojs method.
    /// </summary>
    public List<string> ProjectsGetCsprojs
    {
        get => _projectsGetCsprojs!;
        set => _projectsGetCsprojs = value;
    }

    /// <summary>
    /// Gets or sets modules for selling in format solution name\project name\module name.
    /// </summary>
    public List<string> ModulesSelling { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets modules not for selling in format solution name\project name\module name.
    /// </summary>
    public List<string> ModulesNotSelling { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the solution name without diacritics.
    /// </summary>
    public string NameSolutionWithoutDiacritic { get; set; } = "";

    /// <summary>
    /// Gets or sets the count of images in the solution.
    /// </summary>
    public int CountOfImages { get; set; } = 0;

    /// <summary>
    /// Gets or sets a value indicating whether the solution is in Visual Studio folder.
    /// </summary>
    public bool InVsFolder { get; set; } = false;

    /// <summary>
    /// Gets or sets the repository location.
    /// </summary>
    public RepositoryLocal Repository { get; set; }

    /// <summary>
    /// Gets or sets the solution name without extension.
    /// </summary>
    public string? SlnNameWithoutExtension { get; set; } = null;
    /// <summary>
    /// Returns the string representation of the solution folder.
    /// </summary>
    /// <returns>String representation including image count if present.</returns>
    public override string ToString()
    {
        if (CountOfImages != 0)
        {
            return DisplayedText + " (" + CountOfImages.ToString() + " images)";
        }

        return DisplayedText;
    }

    /// <summary>
    /// Compares two solution folders by their image count.
    /// </summary>
    /// <param name="left">First solution folder.</param>
    /// <param name="right">Second solution folder.</param>
    /// <returns>True if left has more images than right.</returns>
    public static bool operator>(SolutionFolder left, SolutionFolder right)
    {
        return left.CountOfImages > right.CountOfImages;
    }

    /// <summary>
    /// Compares two solution folders by their image count.
    /// </summary>
    /// <param name="left">First solution folder.</param>
    /// <param name="right">Second solution folder.</param>
    /// <returns>True if left has fewer images than right.</returns>
    public static bool operator <(SolutionFolder left, SolutionFolder right)
    {
        return left.CountOfImages < right.CountOfImages;
    }
}