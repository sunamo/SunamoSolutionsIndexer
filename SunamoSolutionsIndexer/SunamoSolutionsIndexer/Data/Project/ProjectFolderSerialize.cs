namespace SunamoSolutionsIndexer.Data.Project;

// TODO: Merge with SolutionInListBox
/// <summary>
/// Represents the project folder serialize type.
/// </summary>
public class ProjectFolderSerialize
{
    private SolutionFolderSerialize solution;
    private string nameProject;

    // Is filled in constructor.
    private string displayedText;

    /// <summary>
    /// Gets the solution.
    /// </summary>
    public SolutionFolderSerialize Solution => solution;

    /// <summary>
    /// Gets the name project.
    /// </summary>
    public string NameProject => nameProject;

    /// <summary>
    /// Project folder serialize.
    /// </summary>
    /// <param name="solution">The solution.</param>
    /// <param name="nameProject">The name project.</param>
    /// <returns>The project folder serialize result.</returns>
    public ProjectFolderSerialize(SolutionFolderSerialize solution, string nameProject)
    {
        this.solution = solution;
        this.nameProject = nameProject;
        displayedText = SolutionsIndexerHelper.GetDisplayedSolutionName(Path.Combine(solution.FullPathFolder, nameProject));
    }

    /// <summary>
    /// To string.
    /// </summary>
    /// <param name="nameProject)">The name project.</param>
    /// <returns>The to string result.</returns>
    public override string ToString()
    {
        return displayedText;
    }
}