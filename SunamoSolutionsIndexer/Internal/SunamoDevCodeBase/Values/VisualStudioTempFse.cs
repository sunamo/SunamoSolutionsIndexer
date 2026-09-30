namespace SunamoDevCode.Values;

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

    public const string GitignoreFile = ".gitignore";

    #region 1) To delete
    public static List<string> FoldersInSolutionToDelete = new List<string>([".vs", "_UpgradeReport_Files", "obj", "Backup*", "bin", "obj", "TestResults", "MigrationBackup"]);
    public static List<string> FoldersInProjectToDelete = new List<string>([".vs", "obj", "bin", "BundleArtifacts"]);
    public static List<string> FoldersAnywhereToDelete = new List<string> { };

    public static ExtensionSortedCollection FilesInSolutionToDelete = new ExtensionSortedCollection("UpgradeLog*.htm", "UpgradeLog*.htm", "*.suo");
    public static ExtensionSortedCollection FilesInProjectToDelete = new ExtensionSortedCollection("*.user");
    public static ExtensionSortedCollection FilesAnywhereToDelete = new ExtensionSortedCollection("Thumbs.db", "*.TMP", "*.tmp");
    #endregion

    #region 2) To keep
    public static List<string> FoldersInSolutionToKeep = new List<string> { GitFolderName };
    public static List<string> FoldersInProjectToKeep = new List<string> { "AppPackages", "Assets", "BundleArtifacts", "Fonts", "MultilingualResources", "Properties", "Service References", "screenshots", "Import Schema Logs", "Stored Procedures", "XMLSchemaCollections", "User Defined Types" };
    public static List<string> FoldersAnywhereToKeep = new List<string> { };

    public static List<string> FilesInSolutionToKeep = new List<string> { };
    public static List<string> FilesInProjectToKeep = new List<string> { "*.suo" };
    public static List<string> FilesAnywhereToKeep = new List<string> { "*_TemporaryKey.pfx", "*.png", "*.jpg", "*.bmp", "*.ico", "*.dll", "README.md", "*.resx", "*.mdf", "*.ldf", "project.json", "project.lock.json", "*.nuget.targets" };
    #endregion

    #region 3) Downloaded
    public static List<string> FoldersInSolutionDownloaded = new List<string> { "packages", GitFolderName, "node_modules", "x64", ".vs", ".vscode", "lib", ".idea", ".nuget", ".svn", ".*" };
    public static List<string> FoldersInProjectDownloaded = new List<string> { };
    public static List<string> FoldersAnywhereDownloaded = new List<string> { };

    // next all files starting with dot - ".gitignore", ".browserslistrc"
    // also all .json
    public static ExtensionSortedCollection FilesInSolutionDownloaded = new ExtensionSortedCollection(".gitattributes", GitignoreFile, "package-lock.json", "README.md");
    public static List<string> FilesInProjectDownloaded = new List<string> { };
    public static List<string> FilesAnywhereDownloaded = new List<string> { };
    #endregion

    public static List<string> FilesWeb = new List<string>() {  "img",
"ts",
"js",
"css",
"Scripts",
"bower_components"};

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
