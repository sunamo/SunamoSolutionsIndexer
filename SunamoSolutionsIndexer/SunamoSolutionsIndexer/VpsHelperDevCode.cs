namespace SunamoSolutionsIndexer;

/// <summary>
/// Represents the vps helper dev code type.
/// </summary>
public class VpsHelperDevCode
{
    private static PushSolutionsData pushSolutionsData = new PushSolutionsData();

    /// <summary>
    /// Gets the list vps new.
    /// </summary>
    public static PpkOnDriveDC ListVpsNew { get; } = new PpkOnDriveDC(SolutionsIndexerPaths.listVpsNew!);

    /// <summary>
    /// Gets the list scz admin 64.
    /// </summary>
    public static PpkOnDriveDC ListSczAdmin64 { get; } = new PpkOnDriveDC(SolutionsIndexerPaths.listSczAdmin64!);

    /// <summary>
    /// Push all.
    /// </summary>
    /// <param name="SolutionsIndexerPaths.listSczAdmin64!">The solutions indexer paths list scz admin 64.</param>
    /// <returns>The push all result.</returns>
    public static async Task PushAll(Func<List<string>, Task<List<List<string>>>> psInvoke)
    {
        pushSolutionsData.Set(false);
        await PushPullAll(psInvoke);
    }

    private static async Task<string> PushPullAll(Func<List<string>, Task<List<List<string>>>> psInvoke)
    {
        bool release = true;
        string pushArgs = string.Empty;
        string commitMessage = "Before publishing to VPS" + " " + DateTime.Today.ToShortDateString();

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
    /// Gets or sets the pull all result.
    /// </summary>
    public static string? PullAllResult { get; set; } = null;

    /// <summary>
    /// Pull all.
    /// </summary>
    /// <returns>The pull all result.</returns>
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
