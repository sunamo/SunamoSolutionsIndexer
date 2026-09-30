namespace SunamoDevCode._sunamo.SunamoInterfaces.Interfaces;

internal interface IGitBashBuilder
{
    List<string> Commands { get; }
    void Add(string filePattern);
    void AddNewRemote(string remoteName);
    void Append(string text);
    void AppendLine();
    void AppendLine(string text);
    void Cd(string directory);
    void Checkout(string branchName);
    void Clean(string options);
    void Clear();
    void Clone(string repositoryUrl);
    void Commit(bool isAddingAllUntrackedFiles, string commitMessage);
    void Config(string configOption);
    void Fetch(string remoteName = "");
    void Init();
    void Merge(string branchName);
    void Pull();
    void Push(bool isForce);
    void Push(string remoteName);
    void Remote(string command);
    void Status();
    string ToString();
}