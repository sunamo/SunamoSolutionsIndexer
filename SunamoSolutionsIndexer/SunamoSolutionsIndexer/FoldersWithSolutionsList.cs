namespace SunamoDevCode.SunamoSolutionsIndexer;

/// <summary>
/// List of FoldersWithSolutions instances
/// </summary>
public class FoldersWithSolutionsList : List<FoldersWithSolutions>
{
    /// <summary>
    /// Adds a FoldersWithSolutions instance to the list
    /// </summary>
    /// <param name="foldersWithSolutions">FoldersWithSolutions instance to add</param>
    public new void Add(FoldersWithSolutions foldersWithSolutions)
    {
        base.Add(foldersWithSolutions);
    }
}