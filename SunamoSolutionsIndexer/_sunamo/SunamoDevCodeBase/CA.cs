namespace SunamoSolutionsIndexer._sunamo;

/// <summary>
/// Helpers for working with collections.
/// </summary>
internal partial class CA
{

    /// <summary>
    /// Trims every element of the list.
    /// </summary>
    internal static List<string> Trim(List<string> list)
    {
        for (var i = 0; i < list.Count; i++)
            list[i] = list[i].Trim();
        return list;
    }
}
