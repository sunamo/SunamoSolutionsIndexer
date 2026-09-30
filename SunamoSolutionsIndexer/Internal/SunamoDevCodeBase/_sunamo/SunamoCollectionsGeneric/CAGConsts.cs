namespace SunamoDevCode._sunamo.SunamoCollectionsGeneric;

// This must be here because SunamoValues cannot inherit from SunamoCollectionGeneric - it would create a cycle dependency
internal class CAGConsts
{
    internal static List<T> ToList<T>(params T[] items)
        => items.ToList();
}
