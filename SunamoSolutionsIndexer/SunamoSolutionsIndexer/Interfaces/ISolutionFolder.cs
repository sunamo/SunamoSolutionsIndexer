namespace SunamoSolutionsIndexer;

public interface ISolutionFolder
{
    List<string> ProjectsGetCsprojs { get; set; }
    List<string> ProjectsInSolution { get; set; }
    ProjectsTypes TypeProjectFolder { get; set; }

    string? ExeToRelease(SolutionFolder sln, string projectDistinction, bool isStandaloneSlnForProject, bool isAddingProtectedWhenSelling = false, bool isPublishing = false);
    bool HaveGitFolder();
    string ToString();
    void UpdateModules(ILogger logger, PpkOnDriveDC toSelling);
}
