namespace SunamoSolutionsIndexer.Data.SolutionFoldersNs;

public class SolutionFolders : List<SolutionFolder>
{
    Dictionary<string, SolutionFolder> index = null;

    public SolutionFolders(IList<SolutionFolder> collection) : base(collection)
    {
    }

    public SolutionFolder this[string dx]
    {
        get => index[dx];
        //set => users.Insert(index, value);
    }

    public void DoIndex()
    {
        index = new Dictionary<string, SolutionFolder>(Count);
        foreach (var item in this)
        {
            index.Add(item.nameSolution, item);
        }
    }

    public void IsAllNamesUnique(IList<string> names = null)
    {
        if (names == null)
        {
            names = this.Select(s => s.nameSolution).ToList();
        }
        var d = CAG.GetDuplicities(names.ToList());
        {
            ThrowEx.DuplicatedElements("d", d);
        }
    }
}
