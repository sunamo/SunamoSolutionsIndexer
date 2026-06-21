namespace SunamoDevCode;

/// <summary>
///     Must be in Win because use powershell
///     In shared cannot because win derife from shared.
///     If I have abstract layer for shared, then yes
/// </summary>
public class GitHelper
{
    //https://radekjancik.visualstudio.com/AllProjectsSearch.Cmd.Parallel/_git/AllProjectsSearch.Cmd.Parallel

    /// <summary>
    /// Base URL for Visual Studio git repository pattern 1
    /// https://radekjancik.visualstudio.com/_git/AllProjectsSearch
    /// </summary>
    private const string BaseUrlVisualStudioGit =
        "https://radekjancik.visualstudio.com/_git/";

    /// <summary>
    /// Base URL prefix for Visual Studio git repository pattern 2
    /// https://radekjancik@dev.azure.com/radekjancik/CodeProjects_Bobril/_git/CodeProjects_Bobril
    /// </summary>
    private const string BaseUrlVisualStudioPrefix =
        "https://radekjancik.visualstudio.com/";

    /// <summary>
    /// Git path suffix for pattern 2
    /// </summary>
    private const string GitPathSuffix = "/_git/";

    /// <summary>
    /// Base URL prefix for Visual Studio repository pattern 3
    /// https://radekjancik.visualstudio.com/AllProjectsSearch.ToNet5/_git/AllProjectsSearch.ToNet5
    /// </summary>
    private const string BaseUrlVisualStudioShort = "https://radekjancik.visualstudio.com/";

    /// <summary>
    /// Base URL for GitHub repositories
    /// https://github.com/sunamo/sunamo.git
    /// </summary>
    private const string BaseUrlGitHub = "https://github.com/sunamo/";

    /// <summary>
    /// Base URL for Azure DevOps git repositories
    /// https://dev.azure.com/radekjancik/_git/sunamo.webWithoutDep
    /// </summary>
    private const string BaseUrlAzureDevOps = "https://dev.azure.com/radekjancik/_git/";

    /// <summary>
    /// Base URL for Bitbucket repositories
    /// https://bitbucket.org/sunamo/1gp-gopay-master
    /// </summary>
    private const string BaseUrlBitbucket = @"https://bitbucket.org/sunamo/";

    /// <summary>
    /// Generates a PowerShell/Git bash script to pull updates from all specified folders.
    /// </summary>
    /// <param name="folders">List of repository folder paths to pull from.</param>
    /// <returns>Generated bash script content for pulling all repositories.</returns>
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

    /// <summary>
    /// Pushes changes for a solution to the remote repository, including status check and commit.
    /// </summary>
    /// <param name="release">Whether this is a release build push.</param>
    /// <param name="gitBashBuilder">Git bash builder for generating git commands.</param>
    /// <param name="pushArgs">Additional arguments for the push command.</param>
    /// <param name="commitMessage">Commit message to use.</param>
    /// <param name="fullPathFolder">Full path to the solution folder.</param>
    /// <param name="pushSolutionsData">Data about the push operation status.</param>
    /// <param name="gitStatus">Git bash builder for status commands.</param>
    /// <param name="psInvoke">Function to invoke PowerShell commands and return their output.</param>
    /// <returns>True if the push was successful.</returns>
    public static
#if ASYNC
        async Task<bool>
#else
    bool
#endif
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
#if ASYNC
                    await
#endif
                        psInvoke(gitStatus.Commands);

            var statusOutput = result[1];
            // If solution has changes
            var hasChanges = statusOutput.Where(line => line.Contains("nothing to commit")).Count() == 0;
            if (!hasChanges)
                foreach (var lineStatus in statusOutput)
                {
                    var statusLine = lineStatus.Trim();
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
                    var statusLine = lineStatus.Trim();
                    if (statusOutput.Contains("but the upstream is gone"))
                    {
                        hasChanges = true;
                        break;
                    }
                }

            // or/and is a git repository
            var isGitRepository =
                statusOutput.Where(line => line.Contains("not a git repository")).Count() ==
                0; // CA.ReturnWhichContains(, ).Count == 0;
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

    /// <summary>
    /// Extracts repository name from git origin URI
    /// </summary>
    /// <param name="originUri">Git origin URI to parse</param>
    /// <returns>Repository name extracted from the URI</returns>
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