namespace SunamoSolutionsIndexer;

/// <summary>
/// Helper class for VPS-related operations with DevCode solutions.
/// </summary>
public class VpsHelperDevCode
{
    private static PushSolutionsData pushSolutionsData = new PushSolutionsData();

    /// <summary>
    /// Gets the list of VPS new solutions.
    /// </summary>
    public static PpkOnDriveDC ListVpsNew { get; } = new PpkOnDriveDC(SolutionsIndexerPaths.listVpsNew!);

    /// <summary>
    /// Gets the list of SCZ Admin64 solutions.
    /// </summary>
    public static PpkOnDriveDC ListSczAdmin64 { get; } = new PpkOnDriveDC(SolutionsIndexerPaths.listSczAdmin64!);

    /// <summary>
    /// Pushes all solutions to VPS.
    /// </summary>
    /// <param name="psInvoke">PowerShell invoke function.</param>
    public static async Task PushAll(Func<List<string>, Task<List<List<string>>>> psInvoke)
    {
        pushSolutionsData.Set(false);
        await PushPullAll(psInvoke);
    }

    /// <summary>
    /// Pushes or pulls all solutions.
    /// </summary>
    /// <param name="psInvoke">PowerShell invoke function.</param>
    /// <returns>Git bash commands as string.</returns>
    private static async Task<string> PushPullAll(Func<List<string>, Task<List<List<string>>>> psInvoke)
    {
        bool release = true;
        string pushArgs = string.Empty;
        string commitMessage = Translate.FromKey(XlfKeys.BeforePublishingToVPS) + " " + DateTime.Today.ToShortDateString();

        var gitBashBuilder = new GitBashBuilder(new TextBuilderDC());
        var gitStatus = new GitBashBuilder(new TextBuilderDC());
        foreach (var item in ListVpsNew)
        {
            var solution = SolutionsIndexerHelper.SolutionWithName(item);
            var solutionPath = solution!.FullPathFolder;
            await GitHelper.PushSolution(release, gitBashBuilder, pushArgs, commitMessage, solutionPath, pushSolutionsData, gitStatus, psInvoke);
        }

        return gitBashBuilder.ToString();
    }

    /// <summary>
    /// Gets or sets the result of pull all operation.
    /// </summary>
    public static string? PullAllResult { get; set; } = null;

    /// <summary>
    /// Pulls all solutions from VPS.
    /// </summary>
    /// <returns>PowerShell pull commands as string.</returns>
    public static string PullAll()
    {
        List<string> paths = new List<string>();

        foreach (var item in ListVpsNew)
        {
            var solution = SolutionsIndexerHelper.SolutionWithName(item);

            if (solution != null)
            {
                paths.Add(solution.FullPathFolder);
            }
        }

        PullAllResult = GitHelper.PowershellForPull(paths);

        return PullAllResult;
    }
}