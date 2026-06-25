namespace SunamoSolutionsIndexer.Data.SolutionFoldersNs;

public class SolutionFolders : List<SolutionFolder>
{
    private Dictionary<string, SolutionFolder>? index = null;

    public SolutionFolders(IList<SolutionFolder> collection) : base(collection)
    {
    }

    public SolutionFolder this[string solutionName]
    {
        get => index![solutionName];
    }

    public void DoIndex()
    {
        index = new Dictionary<string, SolutionFolder>(Count);
        foreach (var item in this)
        {
            index.Add(item.NameSolution, item);
        }
    }

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