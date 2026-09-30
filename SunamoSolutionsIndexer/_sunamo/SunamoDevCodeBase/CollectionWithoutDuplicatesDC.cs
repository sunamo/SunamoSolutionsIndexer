namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

/// <summary>
/// List that ignores duplicated values.
/// </summary>
internal class CollectionWithoutDuplicatesDC<T> : CollectionWithoutDuplicatesBaseDC<T>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public CollectionWithoutDuplicatesDC() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public CollectionWithoutDuplicatesDC(int count) : base(count)
    {
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public CollectionWithoutDuplicatesDC(IList<T> list) : base(list)
    {
    }

    /// <summary>
    /// Adds the value and returns its index.
    /// </summary>
    public override int AddWithIndex(T value)
    {
        if (IsComparingByString())
        {
            if (Contains(value).GetValueOrDefault())
            {

            }
            else
            {
                Add(value);
                return Collection.Count - 1;
            }
        }
        int index = Collection.IndexOf(value);
        if (index == -1)
        {
            Add(value);
            return Collection.Count - 1;
        }
        return index;
    }

    /// <summary>
    /// Checks whether the value is already present.
    /// </summary>
    public override bool? Contains(T value)
    {
        if (IsComparingByString())
        {
            ItemString = value!.ToString()!;
            return StringRepresentations.Contains(ItemString);
        }
        else
        {
            if (!Collection.Contains(value))
            {
                if (EqualityComparer<T>.Default.Equals(value, default(T)))
                {
                    return null;
                }
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Returns index of the value or -1.
    /// </summary>
    public override int IndexOf(T value)
    {
        if (IsComparingByString())
        {
            return StringRepresentations.IndexOf(value!.ToString()!);
        }
        int index = Collection.IndexOf(value);
        if (index == -1)
        {
            Collection.Add(value);
            return Collection.Count - 1;
        }
        return index;
    }

    /// <summary>
    /// Determines whether the values are compared by their string representation.
    /// </summary>
    protected override bool IsComparingByString() => AllowNull.HasValue && AllowNull.Value;
}