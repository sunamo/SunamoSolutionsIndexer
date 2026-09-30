namespace SunamoDevCode._sunamo.SunamoCollectionsGeneric;

internal class CAG
{
    // Return what exists in both lists
    // Modify both firstList and secondList - keep only which is only in one
    internal static List<T> CompareList<T>(List<T> firstList, List<T> secondList) where T : IEquatable<T>
    {
        var existsInBoth = new List<T>();

        int foundIndex;

        for (int i = secondList.Count - 1; i >= 0; i--)
        {
            T currentItem = secondList[i];
            foundIndex = firstList.IndexOf(currentItem);

            if (foundIndex != -1)
            {
                existsInBoth.Add(currentItem);
                secondList.RemoveAt(i);
                firstList.RemoveAt(foundIndex);
            }
        }

        for (int i = firstList.Count - 1; i >= 0; i--)
        {
            T currentItem = firstList[i];
            foundIndex = secondList.IndexOf(currentItem);

            if (foundIndex != -1)
            {
                existsInBoth.Add(currentItem);
                firstList.RemoveAt(i);
                secondList.RemoveAt(foundIndex);
            }
        }

        return existsInBoth;
    }

    internal static List<T> GetDuplicities<T>(List<T> list)
    {
        return GetDuplicities<T>(list, out _);
    }

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

    // Direct edit - Remove duplicities from list
    // In return value is from every one instance
    // In foundedDuplicities is every duplicities (maybe the same more times)
    internal static List<T> RemoveDuplicitiesList<T>(IList<T> list, out List<T> foundedDuplicities)
    {
        foundedDuplicities = new List<T>();
        var uniqueItems = new List<T>();
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var currentItem = list[i];
            if (!uniqueItems.Contains(currentItem))
            {
                uniqueItems.Add(currentItem);
            }
            else
            {
                list.RemoveAt(i);
                foundedDuplicities.Add(currentItem);
            }
        }

        return uniqueItems;
    }

    internal static List<T> ToList<T>(params T[] items)
        => new List<T>(items);
}
