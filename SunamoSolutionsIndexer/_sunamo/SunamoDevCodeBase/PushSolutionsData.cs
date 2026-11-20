namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

/// <summary>
/// Data for pushing solutions.
/// </summary>
public class PushSolutionsData
{
    /// <summary>
    /// The merge and fetch.
    /// </summary>
    public bool mergeAndFetch = false;
    /// <summary>
    /// The add gitignore.
    /// </summary>
    public bool addGitignore = false;
    /// <summary>
    /// The only these.
    /// </summary>
    public List<string>? onlyThese = null;
    /// <summary>
    /// The cs.
    /// </summary>
    public bool? cs = null;
    /// <summary>
    /// The check for git.
    /// </summary>
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