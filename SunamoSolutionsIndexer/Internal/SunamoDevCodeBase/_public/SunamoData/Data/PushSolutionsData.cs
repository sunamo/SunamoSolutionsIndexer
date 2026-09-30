namespace SunamoSolutionsIndexer.Internal._public.SunamoData.Data;

public class PushSolutionsData
{
    public bool mergeAndFetch = false;
    public bool addGitignore = false;
    public List<string>? onlyThese = null;
    public bool? cs = null;
    public GitTypesOfMessages checkForGit = GitTypesOfMessages.error | GitTypesOfMessages.fatal;

    public void Set(bool mergeAndFetch, bool addGitignore = false)
    {
        this.mergeAndFetch = mergeAndFetch;
        this.addGitignore = addGitignore;
    }
}
