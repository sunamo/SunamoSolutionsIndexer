namespace SunamoSolutionsIndexer.Interfaces;

public interface ISolutionFolder
{
    List<string> projectsGetCsprojs { get; set; }
    List<string> projectsInSolution { get; set; }
    ProjectsTypes typeProjectFolder { get; set; }

    string ExeToRelease(SolutionFolder sln, string projectDistinction, bool standaloneSlnForProject, bool addProtectedWhenSelling = false, bool publish = false);
    bool HaveGitFolder();
    string ToString();
    void UpdateModules(PpkOnDrive toSelling);
}
