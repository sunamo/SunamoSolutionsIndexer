namespace SunamoSolutionsIndexer.Data.SolutionFolderNs;

/// <summary>
/// Serializable solution folder data.
/// </summary>
public class SolutionFolderSerialize : IListBoxHelperItem, ISolutionFolderSerialize
{
    /// <summary>
    /// Returns the displayed text for this solution folder.
    /// </summary>
    /// <returns>Displayed text.</returns>
    public override string ToString()
    {
        return DisplayedText;
    }

    /// <summary>
    /// Gets the type of SolutionFolderSerialize class.
    /// </summary>
    public static Type Type { get; } = typeof(SolutionFolderSerialize);

    private string _DisplayedText = "";

    /// <summary>
    /// Gets or sets the displayed text in listbox, e.g., 2013/PHP Projects/PHPWebSite.
    /// Is assigned in FoldersWithSolutions.
    /// </summary>
    public string DisplayedText
    {
        get => _DisplayedText;
        set => _DisplayedText = value;
    }

    /// <summary>
    /// Gets or sets the full path to folder (internal backing field).
    /// </summary>
    public string _FullPathFolder = "";

    /// <summary>
    /// Gets or sets the solution name (internal backing field).
    /// </summary>
    public string _NameSolution = "";

    /// <summary>
    /// Gets or sets the project folder.
    /// Defaultly null.
    /// Is filled up in SolutionsIndexerHelper.GetProjectFolderAndSlnPath.
    /// Scripts_Projects and so.
    /// </summary>
    public string projectFolder { get; set; } = null!;

    /// <summary>
    /// Gets or sets the solution full path.
    /// Is not full path to sln folder, for this reason there's _FullPathFolder.
    /// Is filled up in AllProjectsSearchHelper.GetProjectFolderAndSlnPath.
    /// _Uap/apps - relative path to solution folder from Project folder.
    /// </summary>
    public string slnFullPath { get; set; } = null!;

    /// <summary>
    /// Gets or sets the full path to folder (e.g., C:\Documents\vs\sunamo\).
    /// Path to folder must have backslash at the end, like folder variables in all my applications.
    /// Full path to solution, must be in separate variable and can't be computed from DisplayedText,
    /// because there are special folders that may be in C:\Mona and not in documents.
    /// </summary>
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

    /// <summary>
    /// Gets the final solution name, e.g., PHPWebSite.
    /// If contains hierarchy (as _Uap, won't be included).
    /// </summary>
    public string NameSolution => _NameSolution;

    /// <summary>
    /// Gets the full path to folder for RunOne operation.
    /// </summary>
    public string RunOne => FullPathFolder;

    /// <summary>
    /// Gets the short name (solution name).
    /// </summary>
    public string ShortName => _NameSolution;

    /// <summary>
    /// Gets the long name (full path to folder).
    /// </summary>
    public string LongName => _FullPathFolder;
}