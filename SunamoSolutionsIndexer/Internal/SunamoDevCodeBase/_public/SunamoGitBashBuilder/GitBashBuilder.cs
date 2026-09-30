namespace SunamoSolutionsIndexer.Internal._public.SunamoGitBashBuilder;

public partial class GitBashBuilder : IGitBashBuilder
{
    public void Pull()
    {
        Git("pull");
        AppendLine();
    }

    // 11-9 the repoUrl attribute has been removed because it is fully replaceable with args
    public void Clone(string args)
    {
        Git("clone " + args);
        AppendLine();
    }

    public void Commit(bool addAllUntrackedFiles, string commitMessage)
    {
        ThrowEx.IsNullOrWhitespace("commitMessage", commitMessage);
        Git("commit ");
        if (addAllUntrackedFiles)
        {
            Append("-a");
        }

        if (!string.IsNullOrWhiteSpace(commitMessage))
        {
            Append("-m " + SH.WrapWithQm(commitMessage));
        }

        AppendLine();
    }

    public void Push(bool force)
    {
        Git("push");
        if (force)
        {
            Append("-f");
        }

        AppendLine();
    }

    public void Push(string arg)
    {
        Git("push");
        Append(arg);
        AppendLine();
    }

    public void Init()
    {
        Git("init");
        AppendLine();
    }

    public void Add(string filePath)
    {
        Git("add");
        Append(filePath);
        AppendLine();
    }

    public void Config(string configOption)
    {
        Git("config");
        Append(configOption);
        AppendLine();
    }

    public void Clean(string cleanOptions)
    {
        Git("clean");
        Arg(cleanOptions);
        AppendLine();
    }

    public static string GitStatic(StringBuilder sb, string remainCommand)
    {
        sb.Append("git " + remainCommand);
        return sb.ToString();
    }

    private void Git(string remainingCommand)
    {
        if (remainingCommand[remainingCommand.Length - 1] != ' ')
        {
            remainingCommand += " ";
        }

        StringBuilder.Append((GitForDebug ? "GitForDebug " : "git ") + remainingCommand);
    }

    private void Arg(string argument)
    {
        Append("-" + argument);
    }

    public void Comment(string commentText)
    {
        AppendLine("#" + commentText);
    }

    public void MultilineComment(List<string> lines)
    {
        AppendLine("<#");
        foreach (var item in lines)
        {
            AppendLine(item);
        }

        AppendLine("#>");
    }

    public void Remote(string arg)
    {
        Git("remote");
        Append(arg);
        AppendLine();
    }

    public void Status()
    {
        Git("status");
        AppendLine();
    }

    public void Fetch(string remoteName = "")
    {
        Git("fetch " + remoteName);
        AppendLine();
    }

    public void Merge(string branchName)
    {
        Git("merge " + branchName);
        AppendLine();
    }

    public void AddNewRemote(string remoteUrl)
    {
        Remote("add origin " + remoteUrl);
        Fetch("origin");
        Checkout("-b master --track origin/master");
        AppendLine("vsGitIgnoreGitHub");
        AppendLine("gaacipuu");
    }

    public void Checkout(string arg)
    {
        Git("checkout");
        AppendLine(arg);
    }

    private static Type type = typeof(GitBashBuilder);
    public TextBuilderDC StringBuilder { get; set; } = null!;
    public GitBashBuilder(TextBuilderDC stringBuilder)
    {
        this.StringBuilder = stringBuilder;
    }

    public bool GitForDebug { get; set; } = false;
    public List<string> Commands { get => SHGetLines.GetLines(ToString()); }

    public static string CreateGitAddForFiles(StringBuilder sb, List<string> linesFiles)
    {
        return CreateGitCommandForFiles("add", sb, linesFiles);
    }

    public static string GenerateCommandForGit( /*object tlb,*/string solution, List<string> linesFiles, out bool anyError, string searchOnlyWithExtension, string command, string basePathIfA2SolutionsWontExistsOnFilesystem)
    {
        var filesToCommit = GitBashBuilder.PrepareFilesToSimpleGitFormat( /*tlb,*/solution, linesFiles, out anyError, searchOnlyWithExtension, basePathIfA2SolutionsWontExistsOnFilesystem);
        if (filesToCommit == null || filesToCommit.Count == 0)
        {
            return "";
        }

        string result = GitBashBuilder.CreateGitCommandForFiles(command, new StringBuilder(), filesToCommit);
        return result;
    }

    public static string CheckoutWithExtension(string folder, string typedExt, List<string> files, string basePathIfA2SolutionsWontExistsOnFilesystem, TextBuilderDC ci)
    {
        ThrowEx.IsNull("typedExt", typedExt);
        GitBashBuilder bashBuilder = new GitBashBuilder(ci);
        bool anyError = false;
        var filesToCommit = GitBashBuilder.PrepareFilesToSimpleGitFormat( /*null,*/folder, files, out anyError, typedExt, basePathIfA2SolutionsWontExistsOnFilesystem);
        if (filesToCommit == null)
        {
        }

        string result = GitBashBuilder.GenerateCommandForGit( /*null,*/folder, files, out anyError, typedExt, "checkout", basePathIfA2SolutionsWontExistsOnFilesystem);
        return result;
    }
}
