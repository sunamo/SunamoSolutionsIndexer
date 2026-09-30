namespace SunamoDevCode._public.SunamoCollectionWithoutDuplicates;

internal class CollectionWithoutDuplicatesDC<T> : CollectionWithoutDuplicatesBaseDC<T>
{
    public CollectionWithoutDuplicatesDC() : base()
    {
    }

    public CollectionWithoutDuplicatesDC(int count) : base(count)
    {
    }

    public CollectionWithoutDuplicatesDC(IList<T> list) : base(list)
    {
    }

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

    protected override bool IsComparingByString() => AllowNull.HasValue && AllowNull.Value;
}
