namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

/// <summary>
/// Data for pushing solutions.
/// </summary>
public class PushSolutionsData
{
    public bool mergeAndFetch = false;
    public bool addGitignore = false;
    public List<string>? onlyThese = null;
    public bool? cs = null;
    public GitTypesOfMessages checkForGit = GitTypesOfMessages.error | GitTypesOfMessages.fatal;

    /// <summary>
    /// Sets the values.
    /// </summary>
    public void Set(bool mergeAndFetch, bool addGitignore = false)
    {
        this.mergeAndFetch = mergeAndFetch;
        this.addGitignore = addGitignore;
    }
}