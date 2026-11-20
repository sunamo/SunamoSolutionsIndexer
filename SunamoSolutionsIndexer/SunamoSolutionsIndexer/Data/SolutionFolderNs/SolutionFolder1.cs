namespace SunamoSolutionsIndexer.Data.SolutionFolderNs;

public partial class SolutionFolder : SolutionFolderSerialize, ISolutionFolder
{
    public string? ExeToRelease(SolutionFolder solution, string projectDistinction, bool standaloneSlnForProject, bool addProtectedWhenSelling = false, bool publish = false)
    {
        var exeName = solution.NameSolution;
        var exeNameWithExt = exeName + AllExtensions.ExeExtension;

        // The built app can be either in the main checkout or in its "-claude" worktree
        // (the <Repo>-claude convention) - the one with the newer exe is taken.
        var roots = new[]
        {
            solution.FullPathFolder.TrimEnd('\\'),
            solution.FullPathFolder.TrimEnd('\\') + "-claude",
        };

        string? best = null;
        var bestTime = DateTime.MinValue;

        foreach (var root in roots)
        {
            var candidate = ExeToReleaseInRoot(root, exeName, exeNameWithExt, publish);

            if (candidate == null)
            {
                continue;
            }

            var writeTime = File.GetLastWriteTime(candidate);

            if (writeTime > bestTime)
            {
                bestTime = writeTime;
                best = candidate;
            }
        }

        return best;
    }

    private string? ExeToReleaseInRoot(string solutionFolder, string exeName, string exeNameWithExt, bool publish)
    {
        var projectFolderPath = FindProjectFolderPathByExeName(solutionFolder, exeName, exeNameWithExt);

        if (projectFolderPath == null)
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

        string? existingExeReleaseFolder = null;

        if (net7Path != null && Directory.Exists(net7Path))
        {
            var exePath = Path.Combine(net7Path, exeNameWithExt);
            existingExeReleaseFolder = File.Exists(exePath)
                ? net7Path
                : FindExistingFolderWithRightArchitecture(net7Path, exeNameWithExt);
        }

        if (existingExeReleaseFolder == null && net7WindowsPath != null && Directory.Exists(net7WindowsPath))
        {
            var exePath = Path.Combine(net7WindowsPath, exeNameWithExt);
            existingExeReleaseFolder = File.Exists(exePath)
                ? net7WindowsPath
                : FindExistingFolderWithRightArchitecture(net7WindowsPath, exeNameWithExt);
        }

        return existingExeReleaseFolder == null ? null : Path.Combine(existingExeReleaseFolder, exeNameWithExt);
    }

    // The project folder inside the solution usually shares its name with the repo (exeName), but not always - the repo
    // can be named differently than the product project inside it (e.g. repo "PerfectHome.Ava.Core",
    // project "PerfectHome.Ava"). When a direct match finds nothing, it searches the other subfolders.
    private string? FindProjectFolderPathByExeName(string solutionFolder, string exeName, string exeNameWithExt)
    {
        var direct = Path.Combine(solutionFolder, exeName);

        if (Directory.Exists(direct))
        {
            return direct;
        }

        List<string> candidates;

        try
        {
            candidates = Directory.GetDirectories(solutionFolder).ToList();
        }
        catch (Exception)
        {
            return null;
        }

        foreach (var candidate in candidates)
        {
            var name = Path.GetFileName(candidate.TrimEnd('\\'));

            if (name.StartsWith(".") || name.StartsWith("_") || name is "bin" or "obj")
            {
                continue;
            }

            var releaseRoot = Path.Combine(candidate, @"bin\Release");

            if (!Directory.Exists(releaseRoot))
            {
                continue;
            }

            if (Directory.GetFiles(releaseRoot, exeNameWithExt, SearchOption.AllDirectories).Length > 0)
            {
                return candidate;
            }
        }

        return null;
    }

    // The TFM folder can have any postfix after "-windows" (Windows API version, e.g.
    // "net10.0-windows7.0") or none - only the netX.Y number is compared, the postfix is ignored.
    private static readonly Regex NetVersionWindowsRegex = new(@"^net(?<v>\d+\.\d+)-windows(\d+(\.\d+)*)?$", RegexOptions.IgnoreCase);
    private static readonly Regex NetVersionPlainRegex = new(@"^net(?<v>\d+\.\d+)$", RegexOptions.IgnoreCase);

    private string? FindHighestAvailableNetVersion(string baseReleaseFolder, bool isWindows)
    {
        if (!Directory.Exists(baseReleaseFolder))
        {
            return null;
        }

        var pattern = isWindows ? NetVersionWindowsRegex : NetVersionPlainRegex;

        string? best = null;
        Version? bestVersion = null;

        foreach (var dir in Directory.GetDirectories(baseReleaseFolder))
        {
            var name = Path.GetFileName(dir.TrimEnd('\\'));
            var match = pattern.Match(name);

            if (!match.Success || !Version.TryParse(match.Groups["v"].Value, out var version))
            {
                continue;
            }

            if (bestVersion == null || version > bestVersion)
            {
                bestVersion = version;
                best = dir.TrimEnd('\\') + "\\";
            }
        }

        return best;
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
