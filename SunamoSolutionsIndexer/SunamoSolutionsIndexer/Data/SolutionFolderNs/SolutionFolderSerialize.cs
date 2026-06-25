namespace SunamoSolutionsIndexer.Data.SolutionFolderNs;

public class SolutionFolderSerialize : IListBoxHelperItem, ISolutionFolderSerialize
{
    public override string ToString()
    {
        return DisplayedText;
    }

    public static Type Type { get; } = typeof(SolutionFolderSerialize);

    private string _DisplayedText = "";

    // Gets or sets the displayed text in listbox, e.g., 2013/PHP Projects/PHPWebSite.
    // Is assigned in FoldersWithSolutions.
    public string DisplayedText
    {
        get => _DisplayedText;
        set => _DisplayedText = value;
    }

    // Gets or sets the full path to folder (internal backing field).
    public string _FullPathFolder = "";

    // Gets or sets the solution name (internal backing field).
    public string _NameSolution = "";

    // Gets or sets the project folder.
    // Defaultly null.
    // Is filled up in SolutionsIndexerHelper.GetProjectFolderAndSlnPath.
    // Scripts_Projects and so.
    public string projectFolder { get; set; } = null!;

    // Gets or sets the solution full path.
    // Is not full path to sln folder, for this reason there's _FullPathFolder.
    // Is filled up in AllProjectsSearchHelper.GetProjectFolderAndSlnPath.
    // _Uap/apps - relative path to solution folder from Project folder.
    public string slnFullPath { get; set; } = null!;

    // Gets or sets the full path to folder (e.g., C:\Documents\vs\sunamo\).
    // Path to folder must have backslash at the end, like folder variables in all my applications.
    // Full path to solution, must be in separate variable and can't be computed from DisplayedText,
    // because there are special folders that may be in C:\Mona and not in documents.
    public string FullPathFolder
    {
        get => _FullPathFolder;
        set
        {
            _FullPathFolder = value;
            _NameSolution = Path.GetFileName(value.TrimEnd('\\'));
            if (SolutionsIndexerSettings.IgnorePartAfterUnderscore)
            {
                int lastUnderscoreIndex = _NameSolution.LastIndexOf('_');
                if (lastUnderscoreIndex != -1)
                {
                    _NameSolution = _NameSolution.Substring(0, lastUnderscoreIndex);
                }
            }
        }
    }

    // Gets the final solution name, e.g., PHPWebSite.
    // If contains hierarchy (as _Uap, won't be included).
    public string NameSolution => _NameSolution;

    public string RunOne => FullPathFolder;

    public string ShortName => _NameSolution;

    public string LongName => _FullPathFolder;
}