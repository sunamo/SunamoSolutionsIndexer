namespace SunamoSolutionsIndexer.Data.SolutionFoldersNs;

/// <summary>
/// Represents the solution folders type.
/// </summary>
public class SolutionFolders : List<SolutionFolder>
{
    private Dictionary<string, SolutionFolder>? index = null;

    /// <summary>
    /// Initializes a new instance of the solution folders class.
    /// </summary>
    /// <param name="collection">The collection.</param>
    public SolutionFolders(IList<SolutionFolder> collection) : base(collection)
    {
    }

    /// <summary>
    /// The this string.
    /// </summary>
    public SolutionFolder this[string solutionName]
    {
        get => index![solutionName];
    }

    /// <summary>
    /// Do index.
    /// </summary>
    /// <returns>The do index result.</returns>
    public void DoIndex()
    {
        index = new Dictionary<string, SolutionFolder>(Count);
        foreach (var item in this)
        {
            index.Add(item.NameSolution, item);
        }
    }

    /// <summary>
    /// Is all names unique.
    /// </summary>
    /// <param name="names">The names.</param>
    /// <returns>The is all names unique result.</returns>
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