namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

/// <summary>
/// Generic helpers for working with collections.
/// </summary>
internal class CAG
{

    /// <summary>
    /// Returns elements that occur more than once in the list.
    /// </summary>
    internal static List<T> GetDuplicities<T>(List<T> list)
    {
        return GetDuplicities<T>(list, out _);
    }

    /// <summary>
    /// Returns elements that occur more than once in the list.
    /// </summary>
    internal static List<T> GetDuplicities<T>(List<T> list, out List<T> alreadyProcessed)
    {
        alreadyProcessed = new List<T>(list.Count);
        var duplicated = new CollectionWithoutDuplicatesDC<T>();
        foreach (var currentItem in list)
        {
            if (alreadyProcessed.Contains(currentItem))
            {
                duplicated.Add(currentItem);
            }
            else
            {
                alreadyProcessed.Add(currentItem);
            }
        }
        return duplicated.Collection;
    }
}