namespace SunamoSolutionsIndexer.Data.Project;

/// <summary>
/// Represents a project folder for serialization.
/// TODO: Merge with SolutionInListBox
/// </summary>
public class ProjectFolderSerialize
{
    private SolutionFolderSerialize solution;
    private string nameProject;

    /// <summary>
    /// Is filled in constructor.
    /// </summary>
    private string displayedText;

    /// <summary>
    /// Gets the solution folder.
    /// </summary>
    public SolutionFolderSerialize Solution => solution;

    /// <summary>
    /// Gets the project name.
    /// </summary>
    public string NameProject => nameProject;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectFolderSerialize"/> class.
    /// </summary>
    /// <param name="solution">The solution folder.</param>
    /// <param name="nameProject">The project name.</param>
    public ProjectFolderSerialize(SolutionFolderSerialize solution, string nameProject)
    {
        this.solution = solution;
        this.nameProject = nameProject;
        displayedText = SolutionsIndexerHelper.GetDisplayedSolutionName(Path.Combine(solution.FullPathFolder, nameProject));
    }

    /// <summary>
    /// Returns the displayed text for this project.
    /// </summary>
    /// <returns>The displayed text.</returns>
    public override string ToString()
    {
        return displayedText;
    }
}