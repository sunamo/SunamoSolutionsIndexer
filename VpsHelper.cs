namespace SunamoSolutionsIndexer;

public class VpsHelper
{
    public static bool IsVps
    {
        get
        {
            return VpsHelperSunamo.IsVps;
        }
    }

    public static string path
    {
        get => VpsHelperSunamo.path;
    }

    static PushSolutionsData pushSolutionsData = new PushSolutionsData();
    //public static PpkOnDrive list = new PpkOnDrive(AppData.ci.GetFile(AppFolders.Data, "SlnVps.txt"));
    //public static PpkOnDrive listMain = new PpkOnDrive(AppData.ci.GetFile(AppFolders.Data, "SlnVpsMain.txt"));
    public static PpkOnDrive listVpsNew = new PpkOnDrive(SolutionsIndexerPaths.listVpsNew);
    public static PpkOnDrive listSczAdmin64 = new PpkOnDrive(SolutionsIndexerPaths.listSczAdmin64);

    public static void PushAll()
    {
        pushSolutionsData.Set(false);
        PushPullAll();
    }

    private static void PushPullAll()
    {
        if (IsVps)
        {
            var folders = FS.GetFolders(path, SearchOption.TopDirectoryOnly);
            bool release = true;
            string pushArgs = string.Empty;
            string commitMessage = sess.i18n(XlfKeys.FromVPS) + " " + DateTime.Today.ToShortDateString();

            var gitBashBuilder = new GitBashBuilder();
            var gitStatus = new GitBashBuilder();

            foreach (var item in folders)
            {
                GitHelper.PushSolution(release, gitBashBuilder, pushArgs, commitMessage, item, pushSolutionsData, gitStatus);
            }

            ClipboardHelper.SetText(gitBashBuilder.ToString());
        }
        else
        {
            bool release = true;
            string pushArgs = string.Empty;
            string commitMessage = sess.i18n(XlfKeys.BeforePublishingToVPS) + " " + DateTime.Today.ToShortDateString();

            var gitBashBuilder = new GitBashBuilder();
            var gitStatus = new GitBashBuilder();
            foreach (var sln in listVpsNew)
            {
                var sln2 = SolutionsIndexerHelper.SolutionWithName(sln);
                var item = sln2.fullPathFolder;
                GitHelper.PushSolution(release, gitBashBuilder, pushArgs, commitMessage, item, pushSolutionsData, gitStatus);
            }

            ClipboardHelper.SetText(gitBashBuilder.ToString());
        }
    }

    public static string pullAllResult = null;

    public static void PullAll(List<string> forMore = null)
    {
        if (IsVps)
        {
            var folders = FS.GetFolders(path, SearchOption.TopDirectoryOnly);
            GitHelper.PowershellForPull(folders);
        }
        else
        {
            // pokračovat s přidáním forMore in close

            List<string> paths = new List<string>();

            foreach (var item in listVpsNew)
            {
                var sln = SolutionsIndexerHelper.SolutionWithName(item);

                if (sln != null)
                {
                    paths.Add(sln.fullPathFolder);
                }
                else
                {
                    ThisApp.Warning(item + " solution was not found");
                }
            }

            pullAllResult = GitHelper.PowershellForPull(paths);
        }
        ClipboardHelper.SetText(pullAllResult);
    }


}
