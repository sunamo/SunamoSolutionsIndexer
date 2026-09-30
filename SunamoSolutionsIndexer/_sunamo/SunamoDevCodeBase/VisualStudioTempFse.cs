namespace SunamoSolutionsIndexer._sunamo;

/// <summary>
/// Temporary files and folders of Visual Studio.
/// </summary>
internal class VisualStudioTempFse
{
    public static List<string>? IgnoredForIndexing = null;

    //public static List<string> foldersInSolutionToDeleteWithWildcard = new List<string> { "Backup*" };
    // "packages" cannot be here - after every cleaning, which is most time very often, must open sunamo sln and compile all
    // bin je např. in _Docker\DockerFromBeginWebForms,Analyzer1\Analyzer1 \Analyzer1 \Analyzer1.Test \Analyzer1.Vsix

    //public static List<string> foldersInSolutionToDelete = null;

    //public AllProjectsSearchConsts()
    //{
    //    foldersInSolutionToDelete = foldersInSolutionToDeleteWithWildcard.Concat(foldersInSolutionToDeleteWoWildcard).ToArray();
    //}
    // Is in foldersInSolutionToKeep
    public const string GitFolderName = ".git";

    public static List<string> FoldersInSolutionToDelete = new List<string>([".vs", "_UpgradeReport_Files", "obj", "Backup*", "bin", "obj", "TestResults", "MigrationBackup"]);
    public static List<string> FoldersInProjectToDelete = new List<string>([".vs", "obj", "bin", "BundleArtifacts"]);
    public static List<string> FoldersAnywhereToDelete = new List<string> { };

    public static List<string> FoldersInSolutionToKeep = new List<string> { GitFolderName };
    public static List<string> FoldersInProjectToKeep = new List<string> { "AppPackages", "Assets", "BundleArtifacts", "Fonts", "MultilingualResources", "Properties", "Service References", "screenshots", "Import Schema Logs", "Stored Procedures", "XMLSchemaCollections", "User Defined Types" };

    public static List<string> FoldersInSolutionDownloaded = new List<string> { "packages", GitFolderName, "node_modules", "x64", ".vs", ".vscode", "lib", ".idea", ".nuget", ".svn", ".*" };
    public static List<string> FoldersInProjectDownloaded = new List<string> { };

    static VisualStudioTempFse()
    {
        IgnoredForIndexing = CA.JoinIList<string>(FoldersInSolutionToDelete,
FoldersInProjectToDelete,
FoldersInSolutionDownloaded,
FoldersAnywhereToDelete,
FoldersInSolutionToKeep,
FoldersInProjectToKeep,
FoldersInProjectDownloaded);
    }
}
