namespace SunamoSolutionsIndexer;

public interface IFoldersWithSolutions
{
    SolutionFolders Solutions(RepositoryLocal repository, bool isLoadingAll = true, IList<string>? skipThese = null, ProjectsTypes prioritize = ProjectsTypes.None);
}
