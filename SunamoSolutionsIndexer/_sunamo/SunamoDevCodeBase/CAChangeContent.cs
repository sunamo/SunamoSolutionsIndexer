namespace SunamoSolutionsIndexer._sunamo;

/// <summary>
/// Changes content of collections according to the arguments.
/// </summary>
internal class CAChangeContent
{
    /// <summary>
    /// Removes null or empty elements when requested by the arguments.
    /// </summary>
    private static void RemoveNullOrEmpty(ChangeContentArgsDC args, List<string> list)
    {
        if (args != null)
        {
            if (args.RemoveNull)
            {
                list.Remove(null!);
            }
            if (args.RemoveEmpty)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (list[i].Trim() == string.Empty)
                    {
                        list.RemoveAt(i);
                    }
                }
            }
        }
    }

    // Direct edit - Changes content of list using provided function with 0 additional parameters
    // If not every element fulfills pattern, it is good to remove null (or values returned if can't be changed) from result
    /// <summary>
    /// Applies the changes configured in the arguments to the list.
    /// </summary>
    internal static List<string> ChangeContent0(ChangeContentArgsDC args, List<string> list, Func<string, string> func)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = func.Invoke(list[i]);
        }
        RemoveNullOrEmpty(args, list);
        return list;
    }
}
