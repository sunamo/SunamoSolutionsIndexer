namespace SunamoSolutionsIndexer;

public class VpsHelperDevCode
{
    private static PushSolutionsData pushSolutionsData = new PushSolutionsData();

    public static PpkOnDriveDC ListVpsNew { get; } = new PpkOnDriveDC(SolutionsIndexerPaths.listVpsNew!);

    public static PpkOnDriveDC ListSczAdmin64 { get; } = new PpkOnDriveDC(SolutionsIndexerPaths.listSczAdmin64!);

    public static async Task PushAll(Func<List<string>, Task<List<List<string>>>> psInvoke)
    {
        pushSolutionsData.Set(false);
        await PushPullAll(psInvoke);
    }

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

    public static string? PullAllResult { get; set; } = null;

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
