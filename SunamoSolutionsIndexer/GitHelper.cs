namespace SunamoSolutionsIndexer;

public class GitHelper
{
    //https://radekjancik.visualstudio.com/AllProjectsSearch.Cmd.Parallel/_git/AllProjectsSearch.Cmd.Parallel

    // Base URL for Visual Studio git repository pattern 1
    // https://radekjancik.visualstudio.com/_git/AllProjectsSearch
    private const string BaseUrlVisualStudioGit =
        "https://radekjancik.visualstudio.com/_git/";

    // Base URL prefix for Visual Studio git repository pattern 2
    // https://radekjancik@dev.azure.com/radekjancik/CodeProjects_Bobril/_git/CodeProjects_Bobril
    private const string BaseUrlVisualStudioPrefix =
        "https://radekjancik.visualstudio.com/";

    // Git path suffix for pattern 2
    private const string GitPathSuffix = "/_git/";

    // Base URL prefix for Visual Studio repository pattern 3
    // https://radekjancik.visualstudio.com/AllProjectsSearch.ToNet5/_git/AllProjectsSearch.ToNet5
    private const string BaseUrlVisualStudioShort = "https://radekjancik.visualstudio.com/";

    // Base URL for GitHub repositories
    // https://github.com/sunamo/sunamo.git
    private const string BaseUrlGitHub = "https://github.com/sunamo/";

    // Base URL for Azure DevOps git repositories
    // https://dev.azure.com/radekjancik/_git/sunamo.webWithoutDep
    private const string BaseUrlAzureDevOps = "https://dev.azure.com/radekjancik/_git/";

    // Base URL for Bitbucket repositories
    // https://bitbucket.org/sunamo/1gp-gopay-master
    private const string BaseUrlBitbucket = @"https://bitbucket.org/sunamo/";

    public static string PowershellForPull(List<string> folders)
    {
        var gitBashBuilder = new GitBashBuilder(new TextBuilderDC());
        foreach (var item in folders)
        {
            gitBashBuilder.Cd(item);
            gitBashBuilder.Pull();
        }

        var pullAllResult = gitBashBuilder.ToString();
        return pullAllResult;
    }

    public static
        async Task<bool>
        PushSolution(bool release, GitBashBuilder gitBashBuilder, string pushArgs, string commitMessage,
            string fullPathFolder, PushSolutionsData pushSolutionsData, GitBashBuilder gitStatus,
            Func<List<string>, Task<List<List<string>>>> psInvoke)
    {
        // 1. better solution is commented only getting files
        var countFiles = 0;
        if (release) countFiles = Directory.GetFiles(fullPathFolder, "*.*", SearchOption.AllDirectories).Length;

        if (fullPathFolder.Contains("SunamoCzAdmin"))
        {
        }

        if (countFiles > 0)
        {
            gitStatus.Clear();
            gitStatus.Cd(fullPathFolder);
            gitStatus.Status();

            var result = new List<List<string>>(new List<List<string>>([new List<string>(), new List<string>()]));
            // 2. or powershell
            if (release)
                result =
                    await
                        psInvoke(gitStatus.Commands);

            var statusOutput = result[1];
            // If solution has changes
            var hasChanges = !statusOutput.Any(line => line.Contains("nothing to commit"));
            if (!hasChanges)
                foreach (var lineStatus in statusOutput)
                {
                    _ = lineStatus.Trim();
                    if (statusOutput.Contains("modified:"))
                        if (statusOutput.Contains(".gitignore"))
                        {
                            hasChanges = true;
                            break;
                        }
                }

            if (!hasChanges)
                foreach (var lineStatus in statusOutput)
                {
                    //
                    _ = lineStatus.Trim();
                    if (statusOutput.Contains("but the upstream is gone"))
                    {
                        hasChanges = true;
                        break;
                    }
                }

            // or/and is a git repository
            var isGitRepository =
                !statusOutput.Any(line => line.Contains("not a git repository")); // CA.ReturnWhichContains(, ).Count == 0;
            if (hasChanges && isGitRepository)
            {
                gitBashBuilder.Cd(fullPathFolder);

                if (pushSolutionsData.mergeAndFetch) gitBashBuilder.Fetch();

                gitBashBuilder.Add("*");

                gitBashBuilder.Commit(false, commitMessage);

                if (pushSolutionsData.mergeAndFetch) gitBashBuilder.Merge("--allow-unrelated-histories");

                if (pushSolutionsData.addGitignore) gitBashBuilder.Add(".gitignore");

                gitBashBuilder.Push(pushArgs);

                gitBashBuilder.AppendLine();

                // Dont run, better is paste into powershell due to checking errors
                //var git = gitBashBuilder.Commands;
                //PowershellRunner.Instance.Invoke(git);

                return true;
            }
        }

        return false;
    }

    public static string NameOfRepoFromOriginUri(string originUri)
    {
        originUri = HttpUtility.UrlDecode(originUri);
        if (originUri.StartsWith(BaseUrlVisualStudioGit))
        {
            originUri = originUri.Replace(BaseUrlVisualStudioGit, string.Empty);
        }
        else if (originUri.StartsWith(BaseUrlVisualStudioPrefix))
        {
            originUri = SH.GetTextBetweenSimple(originUri, BaseUrlVisualStudioPrefix, GitPathSuffix);
        }
        else if (originUri.StartsWith(BaseUrlVisualStudioShort))
        {
            originUri = SH.GetTextBetweenSimple(originUri, BaseUrlVisualStudioShort, GitPathSuffix);
        }
        else if (originUri.StartsWith(BaseUrlGitHub))
        {
            originUri = originUri.Replace(BaseUrlGitHub, string.Empty);
            originUri = SHTrim.TrimEnd(originUri, ".git");
        }
        else if (originUri.StartsWith(BaseUrlAzureDevOps))
        {
            originUri = originUri.Replace(BaseUrlAzureDevOps, string.Empty);
        }
        else if (originUri.StartsWith(BaseUrlBitbucket))
        {
            originUri = originUri.Replace(BaseUrlBitbucket, string.Empty);
        }

        if (originUri.Contains("/")) throw new Exception(originUri + " - name of repo contains still /");

        return originUri;
    }
}
