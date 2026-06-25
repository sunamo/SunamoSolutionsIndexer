namespace SunamoSolutionsIndexer.Data.SolutionFolderNs;

public partial class SolutionFolder : SolutionFolderSerialize, ISolutionFolder
{
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

    public bool HaveGitFolder()
    {
        var gitFolderPath = Path.Combine(FullPathFolder, VisualStudioTempFse.GitFolderName);
        bool exists = Directory.Exists(gitFolderPath);
        return exists;
    }
}