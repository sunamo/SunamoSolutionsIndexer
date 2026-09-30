namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

/// <summary>
/// Builder of git bash commands.
/// </summary>
public partial class GitBashBuilder : IGitBashBuilder
{
    /// <summary>
    /// Appends git pull.
    /// </summary>
    public void Pull()
    {
        Git("pull");
        AppendLine();
    }

    // 11-9 the repoUrl attribute has been removed because it is fully replaceable with args
    /// <summary>
    /// Appends git clone.
    /// </summary>
    public void Clone(string args)
    {
        Git("clone " + args);
        AppendLine();
    }

    /// <summary>
    /// Appends git commit.
    /// </summary>
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

    /// <summary>
    /// Appends git push.
    /// </summary>
    public void Push(bool force)
    {
        Git("push");
        if (force)
        {
            Append("-f");
        }

        AppendLine();
    }

    /// <summary>
    /// Appends git push.
    /// </summary>
    public void Push(string arg)
    {
        Git("push");
        Append(arg);
        AppendLine();
    }

    /// <summary>
    /// Fills the list.
    /// </summary>
    public void Init()
    {
        Git("init");
        AppendLine();
    }

    /// <summary>
    /// Adds the value.
    /// </summary>
    public void Add(string filePath)
    {
        Git("add");
        Append(filePath);
        AppendLine();
    }

    /// <summary>
    /// Appends git config.
    /// </summary>
    public void Config(string configOption)
    {
        Git("config");
        Append(configOption);
        AppendLine();
    }

    /// <summary>
    /// Appends git clean.
    /// </summary>
    public void Clean(string cleanOptions)
    {
        Git("clean");
        Arg(cleanOptions);
        AppendLine();
    }

    /// <summary>
    /// Appends the git command.
    /// </summary>
    private void Git(string remainingCommand)
    {
        if (remainingCommand[remainingCommand.Length - 1] != ' ')
        {
            remainingCommand += " ";
        }

        StringBuilder.Append((GitForDebug ? "GitForDebug " : "git ") + remainingCommand);
    }

    /// <summary>
    /// Appends the argument.
    /// </summary>
    private void Arg(string argument)
    {
        Append("-" + argument);
    }

    /// <summary>
    /// Appends git remote.
    /// </summary>
    public void Remote(string arg)
    {
        Git("remote");
        Append(arg);
        AppendLine();
    }

    /// <summary>
    /// Appends git status.
    /// </summary>
    public void Status()
    {
        Git("status");
        AppendLine();
    }

    /// <summary>
    /// Appends git fetch.
    /// </summary>
    public void Fetch(string remoteName = "")
    {
        Git("fetch " + remoteName);
        AppendLine();
    }

    /// <summary>
    /// Appends git merge.
    /// </summary>
    public void Merge(string branchName)
    {
        Git("merge " + branchName);
        AppendLine();
    }

    /// <summary>
    /// Adds a new remote.
    /// </summary>
    public void AddNewRemote(string remoteUrl)
    {
        Remote("add origin " + remoteUrl);
        Fetch("origin");
        Checkout("-b master --track origin/master");
        AppendLine("vsGitIgnoreGitHub");
        AppendLine("gaacipuu");
    }

    /// <summary>
    /// Appends git checkout.
    /// </summary>
    public void Checkout(string arg)
    {
        Git("checkout");
        AppendLine(arg);
    }
    public TextBuilderDC StringBuilder { get; set; } = null!;
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public GitBashBuilder(TextBuilderDC stringBuilder)
    {
        this.StringBuilder = stringBuilder;
    }

    public bool GitForDebug { get; set; } = false;
    public List<string> Commands { get => SHGetLines.GetLines(ToString()); }
}