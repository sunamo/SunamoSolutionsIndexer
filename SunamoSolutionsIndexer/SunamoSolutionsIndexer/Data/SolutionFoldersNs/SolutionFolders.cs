namespace SunamoDevCode.SunamoSolutionsIndexer.Data.SolutionFoldersNs;

/// <summary>
/// Collection of solution folders with indexing support.
/// </summary>
public class SolutionFolders : List<SolutionFolder>
{
    private Dictionary<string, SolutionFolder>? index = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionFolders"/> class.
    /// </summary>
    /// <param name="collection">Collection of solution folders.</param>
    public SolutionFolders(IList<SolutionFolder> collection) : base(collection)
    {
    }

    /// <summary>
    /// Gets the solution folder by solution name.
    /// </summary>
    /// <param name="solutionName">Solution name to find.</param>
    /// <returns>Solution folder with the specified name.</returns>
    public SolutionFolder this[string solutionName]
    {
        get => index![solutionName];
    }

    /// <summary>
    /// Creates an index of solution folders by their solution name.
    /// </summary>
    public void DoIndex()
    {
        index = new Dictionary<string, SolutionFolder>(Count);
        foreach (var item in this)
        {
            index.Add(item.NameSolution, item);
        }
    }

    /// <summary>
    /// Checks if all solution names are unique and throws exception if duplicates are found.
    /// </summary>
    /// <param name="names">Optional list of names to check. If null, uses solution names from this collection.</param>
    public void IsAllNamesUnique(IList<string>? names = null)
    {
        if (names == null)
        {
            names = this.Select(solution => solution.NameSolution).ToList();
        }
        var duplicates = CAG.GetDuplicities(names.ToList());
        {
            ThrowEx.DuplicatedElements("d", duplicates);
        }
    }
}