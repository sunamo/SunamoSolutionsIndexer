namespace SunamoSolutionsIndexer.Interfaces;

/// <summary>
/// Interface for solution folder operations
/// </summary>
public interface ISolutionFolder
{
    /// <summary>
    /// Gets or sets the list of csproj file paths in this solution.
    /// </summary>
    List<string> ProjectsGetCsprojs { get; set; }
    /// <summary>
    /// Gets or sets the list of project names referenced in the solution file.
    /// </summary>
    List<string> ProjectsInSolution { get; set; }
    /// <summary>
    /// Gets or sets the project type classification for the folder.
    /// </summary>
    ProjectsTypes TypeProjectFolder { get; set; }

    /// <summary>
    /// Returns the path to the release executable for this solution.
    /// </summary>
    /// <param name="sln">Solution folder reference.</param>
    /// <param name="projectDistinction">Project distinction identifier.</param>
    /// <param name="isStandaloneSlnForProject">Whether this is a standalone sln for a single project.</param>
    /// <param name="isAddingProtectedWhenSelling">Whether to add protection when selling.</param>
    /// <param name="isPublishing">Whether this is for a publish operation.</param>
    /// <returns>Path to the release executable.</returns>
    string? ExeToRelease(SolutionFolder sln, string projectDistinction, bool isStandaloneSlnForProject, bool isAddingProtectedWhenSelling = false, bool isPublishing = false);
    /// <summary>
    /// Determines whether this solution folder contains a .git directory.
    /// </summary>
    /// <returns>True if a .git folder exists.</returns>
    bool HaveGitFolder();
    /// <summary>
    /// Returns a string representation of the solution folder.
    /// </summary>
    /// <returns>String representation.</returns>
    string ToString();
    /// <summary>
    /// Updates git submodules for this solution.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="toSelling">Package configuration.</param>
    void UpdateModules(ILogger logger, PpkOnDriveDC toSelling);
}