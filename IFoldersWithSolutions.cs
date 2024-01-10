namespace SunamoSolutionsIndexer;

public interface IFoldersWithSolutionsInstance
{
    SolutionFolders Solutions(Repository r, bool loadAll = true, IList<string> skipThese = null, ProjectsTypes prioritize = ProjectsTypes.None);
}
