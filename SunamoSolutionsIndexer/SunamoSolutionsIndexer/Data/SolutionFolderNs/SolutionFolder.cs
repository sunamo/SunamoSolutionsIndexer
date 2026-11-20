namespace SunamoSolutionsIndexer.Data.SolutionFolderNs;

/// <summary>
/// Represents the solution folder type.
/// </summary>
public partial class SolutionFolder : SolutionFolderSerialize, ISolutionFolder
{
    /// <summary>
    /// Gets the type.
    /// </summary>
    public new static Type Type { get; } = typeof(SolutionFolder);

    // Returns csproj full paths in subfolders of solution folder (one depth).
    // Must use SolutionFolder, because in CreateSolutionFolder is filled projects variable.
    // From every folder all csproj files are taken.
    /// <summary>
    /// Get csprojs.
    /// </summary>
    /// <param name="depth">The depth.</param>
    /// <returns>The get csprojs result.</returns>
    public static void GetCsprojs(ILogger logger, SolutionFolder solutionFolder, GetCsprojsArgs? args = null)
    {
        args ??= new GetCsprojsArgs();

        if (solutionFolder.ProjectsGetCsprojs == null || args.ForceReload)
        {
            var csprojs = new List<string>();
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
    /// Solution folder.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns>The solution folder result.</returns>
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

    // Gets or sets the type of project folder (C# Projects, PHP PHP_Projects, etc.).
    /// <summary>
    /// Gets or sets the type project folder.
    /// </summary>
    public ProjectsTypes TypeProjectFolder { get; set; } = ProjectsTypes.None;

    /// <summary>
    /// Update modules.
    /// </summary>
    /// <param name="Projects">The projects.</param>
    /// <param name="PHP_Projects">The PHP projects.</param>
    /// <param name="etc.">The etc.</param>
    /// <returns>The update modules result.</returns>
    public void UpdateModules(ILogger logger, PpkOnDriveDC toSelling)
    {
        if (toSelling != null)
        {
            ModulesSelling = SolutionsIndexerHelper.ModulesInSolution(logger, ProjectsInSolution, FullPathFolder, true, toSelling);
            ModulesNotSelling = SolutionsIndexerHelper.ModulesInSolution(logger, ProjectsInSolution, FullPathFolder, false, toSelling);
        }
    }

    /// <summary>
    /// Solution folder.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="ProjectsInSolution">The projects in solution.</param>
    /// <param name="FullPathFolder">The full path folder.</param>
    /// <param name="true">The true.</param>
    /// <param name="toSelling">The to selling.</param>
    /// <returns>The solution folder result.</returns>
    public SolutionFolder()
    {
    }

    // Gets or sets the source of projects (SolutionFolder.GetCsprojs or SolutionsIndexerHelper.ProjectsInSolution).
    /// <summary>
    /// Gets or sets the source of projects.
    /// </summary>
    public SourceOfProjects SourceOfProjects { get; set; }

    private List<string> _projects = new List<string>();

    // Gets or sets projects in solution.
    // Only name without path.
    // Is filled in constructor with CreateSolutionFolder().
    // Only subfolders. Csproj files must be found out manually.
    // Csproj are available to get with GetCsprojs().
    /// <summary>
    /// Gets the projects in solution.
    /// </summary>
    public List<string> ProjectsInSolution { get => _projects; set => _projects = value; }

    private List<string>? _projectsGetCsprojs = null;

    // Gets or sets projects from SolutionFolder.GetCsprojs method.
    /// <summary>
    /// The projects get csprojs.
    /// </summary>
    public List<string> ProjectsGetCsprojs
    {
        get => _projectsGetCsprojs!;
        set => _projectsGetCsprojs = value;
    }

    // Gets or sets modules for selling in format solution name\project name\module name.
    /// <summary>
    /// Gets or sets the modules selling.
    /// </summary>
    public List<string> ModulesSelling { get; set; } = new List<string>();

    // Gets or sets modules not for selling in format solution name\project name\module name.
    /// <summary>
    /// Gets or sets the modules not selling.
    /// </summary>
    public List<string> ModulesNotSelling { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the name solution without diacritic.
    /// </summary>
    public string NameSolutionWithoutDiacritic { get; set; } = "";

    /// <summary>
    /// Gets or sets the count of images.
    /// </summary>
    public int CountOfImages { get; set; } = 0;

    /// <summary>
    /// Gets or sets the in vs folder.
    /// </summary>
    public bool InVsFolder { get; set; } = false;

    /// <summary>
    /// Gets or sets the repository.
    /// </summary>
    public RepositoryLocal Repository { get; set; }

    /// <summary>
    /// Gets or sets the sln name without extension.
    /// </summary>
    public string? SlnNameWithoutExtension { get; set; } = null;

    /// <summary>
    /// To string.
    /// </summary>
    /// <typeparam name="string">The type string.</typeparam>
    /// <returns>The to string result.</returns>
    public override string ToString()
    {
        if (CountOfImages != 0)
        {
            return $"{DisplayedText} ({CountOfImages} images)";
        }

        return DisplayedText;
    }

    /// <summary>
    /// Operator.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The operator result.</returns>
    public static bool operator>(SolutionFolder left, SolutionFolder right)
    {
        return left.CountOfImages > right.CountOfImages;
    }

    /// <summary>
    /// Operator.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The operator result.</returns>
    public static bool operator <(SolutionFolder left, SolutionFolder right)
    {
        return left.CountOfImages < right.CountOfImages;
    }
}