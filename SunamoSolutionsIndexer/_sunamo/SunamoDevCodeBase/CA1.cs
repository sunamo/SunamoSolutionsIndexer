namespace SunamoSolutionsIndexer._sunamo;

/// <summary>
/// Helpers for working with collections.
/// </summary>
internal partial class CA
{

    /// <summary>
    /// Joins several lists into one new list.
    /// </summary>
    internal static List<T> JoinIList<T>(params IList<T>[] lists)
    {
        var result = new List<T>();
        foreach (var list in lists)
        {
            foreach (var element in list)
            {
                result.Add((T)element);
            }
        }

        return result;
    }

    /// <summary>
    /// Ensures every path ends with a backslash.
    /// </summary>
    internal static List<string> EnsureBackslash(List<string> paths)
    {
        for (int i = 0; i < paths.Count; i++)
        {
            string path = paths[i];
            if (path[path.Length - 1] != '\\')
            {
                paths[i] = path + "\\";
            }
        }

        return paths;
    }

    /// <summary>
    /// Checks whether the list contains the element.
    /// </summary>
    internal static bool ContainsElement<T>(IList<T> list, T element)
    {
        if (list.Count == 0)
        {
            return false;
        }

        foreach (T item in list)
        {
            if (Comparer<T>.Equals(item, element))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Removes elements that match the wildcard mask.
    /// </summary>
    internal static void RemoveWildcard(List<string> list, string mask)
    {
        //https://stackoverflow.com/a/15275806
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (SH.MatchWildcard(list[i], mask))
            {
                list.RemoveAt(i);
            }
        }
    }
}
