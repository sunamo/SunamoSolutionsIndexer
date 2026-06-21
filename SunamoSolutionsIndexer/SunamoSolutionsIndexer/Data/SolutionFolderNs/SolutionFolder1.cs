namespace SunamoDevCode.SunamoSolutionsIndexer.Data.SolutionFolderNs;

/// <summary>
/// Partial class containing additional methods for SolutionFolder.
/// </summary>
public partial class SolutionFolder : SolutionFolderSerialize, ISolutionFolder
{
    /// <summary>
    /// Gets the path to the executable to be released.
    /// </summary>
    /// <param name="solution">Solution folder.</param>
    /// <param name="projectDistinction">Project distinction (.Wpf, .Cmd, etc.).</param>
    /// <param name="standaloneSlnForProject">Whether to create standalone solution for project.</param>
    /// <param name="addProtectedWhenSelling">Whether to add protected when selling.</param>
    /// <param name="publish">Whether to use publish folder.</param>
    /// <returns>Path to the executable or null if not found.</returns>
    public string? ExeToRelease(SolutionFolder solution, string projectDistinction, bool standaloneSlnForProject, bool addProtectedWhenSelling = false, bool publish = false)
    {
        string? existingExeReleaseFolder = null;
        var solutionFolder = solution.FullPathFolder.TrimEnd('\\');
        var exeName = solution.NameSolution;
        string exeNameWithExt = exeName + AllExtensions.ExeExtension;
        var projectFolderPath = Path.Combine(solutionFolder, exeName);
        if (!Directory.Exists(projectFolderPath))
        {
            return null;
        }

        var baseReleaseFolder = Path.Combine(projectFolderPath, @"bin\Release\");

        var net7Path = FindHighestAvailableNetVersion(baseReleaseFolder, false);
        var net7WindowsPath = FindHighestAvailableNetVersion(baseReleaseFolder, true);

        if (publish)
        {
            if (net7Path != null) net7Path += "win-x64\\publish\\";
            if (net7WindowsPath != null) net7WindowsPath += "win-x64\\publish\\";
        }

        var isNet7Exists = net7Path != null && Directory.Exists(net7Path);
        var isNet7WindowsExists = net7WindowsPath != null && Directory.Exists(net7WindowsPath);
        string? exePath = null;

        if (isNet7Exists)
        {
            exePath = Path.Combine(net7Path!, exeName + ".exe");
            if (File.Exists(exePath))
            {
                existingExeReleaseFolder = net7Path;
            }
            else
            {
                existingExeReleaseFolder = FindExistingFolderWithRightArchitecture(net7Path!, exeNameWithExt);
            }
        }

        if (isNet7WindowsExists && existingExeReleaseFolder == null)
        {
            exePath = Path.Combine(net7WindowsPath!, exeName + ".exe");
            if (File.Exists(exePath))
            {
                existingExeReleaseFolder = net7WindowsPath;
            }
            else
            {
                existingExeReleaseFolder = FindExistingFolderWithRightArchitecture(net7WindowsPath!, exeNameWithExt);
            }
        }

        if (existingExeReleaseFolder == null)
        {
            return null;
        }

        var result = Path.Combine(existingExeReleaseFolder, exeNameWithExt);
        return result;
    }

    /// <summary>
    /// Finds the highest available .NET version folder starting from net15.0 and going down.
    /// </summary>
    /// <param name="baseReleaseFolder">Base release folder path.</param>
    /// <param name="isWindows">True for net*-windows, false for net*.</param>
    /// <returns>Full path to the found folder or null if none exists.</returns>
    private string? FindHighestAvailableNetVersion(string baseReleaseFolder, bool isWindows)
    {
        for (int version = 15; version >= 5; version--)
        {
            var netFolder = isWindows
                ? Path.Combine(baseReleaseFolder, $"net{version}.0-windows\\")
                : Path.Combine(baseReleaseFolder, $"net{version}.0\\");

            if (Directory.Exists(netFolder))
            {
                return netFolder;
            }
        }

        return null;
    }

    /// <summary>
    /// Finds existing folder with the right architecture (win-x64 or win-x86).
    /// </summary>
    /// <param name="basePath">Base path to search in.</param>
    /// <param name="exeNameWithExt">Executable name with extension.</param>
    /// <returns>Directory path if found, otherwise null.</returns>
    private string? FindExistingFolderWithRightArchitecture(string basePath, string exeNameWithExt)
    {
        // https://learn.microsoft.com/en-us/dotnet/core/rid-catalog
        var maybe = Path.Combine(basePath, "win-x64", exeNameWithExt);
        if (File.Exists(maybe))
        {
            return Path.GetDirectoryName(maybe);
        }

        maybe = Path.Combine(basePath, "win-x86", exeNameWithExt);
        if (File.Exists(maybe))
        {
            return Path.GetDirectoryName(maybe);
        }

        return null;
    }

    /// <summary>
    /// Checks whether the solution folder has a .git folder.
    /// </summary>
    /// <returns>True if .git folder exists, otherwise false.</returns>
    public bool HaveGitFolder()
    {
        var gitFolderPath = Path.Combine(FullPathFolder, VisualStudioTempFse.GitFolderName);
        bool exists = Directory.Exists(gitFolderPath);
        return exists;
    }
}