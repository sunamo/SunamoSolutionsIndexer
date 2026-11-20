namespace SunamoSolutionsIndexer.Interfaces;

/// <summary>
/// Represents the i solution folder type.
/// </summary>
public interface ISolutionFolder
{
    /// <summary>
    /// Gets or sets the projects get csprojs.
    /// </summary>
    List<string> ProjectsGetCsprojs { get; set; }
    /// <summary>
    /// Gets or sets the projects in solution.
    /// </summary>
    List<string> ProjectsInSolution { get; set; }
    /// <summary>
    /// Gets or sets the type project folder.
    /// </summary>
    ProjectsTypes TypeProjectFolder { get; set; }

    /// <summary>
    /// Exe to release.
    /// </summary>
    /// <param name="sln">The sln.</param>
    /// <param name="projectDistinction">The project distinction.</param>
    /// <param name="isStandaloneSlnForProject">The is standalone sln for project.</param>
    /// <param name="isAddingProtectedWhenSelling">The is adding protected when selling.</param>
    /// <param name="isPublishing">The is publishing.</param>
    /// <returns>The exe to release result.</returns>
    string? ExeToRelease(SolutionFolder sln, string projectDistinction, bool isStandaloneSlnForProject, bool isAddingProtectedWhenSelling = false, bool isPublishing = false);
    /// <summary>
    /// Have git folder.
    /// </summary>
    /// <param name="sln">The sln.</param>
    /// <param name="projectDistinction">The project distinction.</param>
    /// <param name="isStandaloneSlnForProject">The is standalone sln for project.</param>
    /// <param name="isAddingProtectedWhenSelling">The is adding protected when selling.</param>
    /// <param name="isPublishing">The is publishing.</param>
    /// <returns>The have git folder result.</returns>
    bool HaveGitFolder();
    /// <summary>
    /// To string.
    /// </summary>
    /// <param name="sln">The sln.</param>
    /// <param name="projectDistinction">The project distinction.</param>
    /// <param name="isStandaloneSlnForProject">The is standalone sln for project.</param>
    /// <param name="isAddingProtectedWhenSelling">The is adding protected when selling.</param>
    /// <param name="isPublishing">The is publishing.</param>
    /// <returns>The to string result.</returns>
    string ToString();
    /// <summary>
    /// Update modules.
    /// </summary>
    /// <param name="sln">The sln.</param>
    /// <param name="projectDistinction">The project distinction.</param>
    /// <param name="isStandaloneSlnForProject">The is standalone sln for project.</param>
    /// <param name="isAddingProtectedWhenSelling">The is adding protected when selling.</param>
    /// <param name="isPublishing">The is publishing.</param>
    /// <returns>The update modules result.</returns>
    void UpdateModules(ILogger logger, PpkOnDriveDC toSelling);
}