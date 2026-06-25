namespace SunamoSolutionsIndexer.Data.Project;

// TODO: Merge with SolutionInListBox
public class ProjectFolderSerialize
{
    private SolutionFolderSerialize solution;
    private string nameProject;

    // Is filled in constructor.
    private string displayedText;

    public SolutionFolderSerialize Solution => solution;

    public string NameProject => nameProject;

    public ProjectFolderSerialize(SolutionFolderSerialize solution, string nameProject)
    {
        this.solution = solution;
        this.nameProject = nameProject;
        displayedText = SolutionsIndexerHelper.GetDisplayedSolutionName(Path.Combine(solution.FullPathFolder, nameProject));
    }

    public override string ToString()
    {
        return displayedText;
    }
}