namespace SunamoSolutionsIndexer;

/// <summary>
/// Represents the folders with solutions list type.
/// </summary>
public class FoldersWithSolutionsList : List<FoldersWithSolutions>
{
    /// <summary>
    /// Add.
    /// </summary>
    /// <param name="foldersWithSolutions">The folders with solutions.</param>
    public new void Add(FoldersWithSolutions foldersWithSolutions)
    {
        base.Add(foldersWithSolutions);
    }
}
