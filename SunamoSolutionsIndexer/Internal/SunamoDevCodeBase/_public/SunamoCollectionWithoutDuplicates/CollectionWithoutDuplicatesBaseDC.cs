namespace SunamoDevCode._public.SunamoCollectionWithoutDuplicates;

internal abstract class CollectionWithoutDuplicatesBaseDC<T>
{
    public List<T> Collection { get; set; } = null!;

    public List<string> StringRepresentations { get; set; } = null!;

    private bool? _allowNull = false;

    public bool? AllowNull
    {
        get => _allowNull;
        set
        {
            _allowNull = value;
            if (value.HasValue && value.Value)
            {
                StringRepresentations = new List<string>(InitialCapacity);
            }
        }
    }

    public static bool BreakOnConstruction { get; set; } = false;

    private int InitialCapacity { get; set; } = 10000;

    public CollectionWithoutDuplicatesBaseDC()
    {
        if (BreakOnConstruction)
        {
            System.Diagnostics.Debugger.Break();
        }
        Collection = new List<T>();
    }

    public CollectionWithoutDuplicatesBaseDC(int capacity)
    {
        this.InitialCapacity = capacity;
        Collection = new List<T>(capacity);
    }

    public CollectionWithoutDuplicatesBaseDC(IList<T> initialList)
    {
        Collection = new List<T>(initialList.ToList());
    }

    public bool Add(T value)
    {
        bool result = false;
        var containsResult = Contains(value);
        if (containsResult.HasValue)
        {
            if (!containsResult.Value)
            {
                Collection.Add(value);
                result = true;
            }
        }
        else
        {
            if (!AllowNull.HasValue)
            {
                Collection.Add(value);
                result = true;
            }
        }
        if (result)
        {
            if (IsComparingByString())
            {
                StringRepresentations.Add(ItemString);
            }
        }
        return result;
    }

    protected abstract bool IsComparingByString();

    protected string ItemString { get; set; } = null!;

    public abstract bool? Contains(T value);

    public abstract int AddWithIndex(T value);

    public abstract int IndexOf(T value);

    private List<T> WasNotAdded { get; set; } = new List<T>();

    public List<T> AddRange(IList<T> list)
    {
        WasNotAdded.Clear();
        foreach (var item in list)
        {
            if (!Add(item))
            {
                WasNotAdded.Add(item);
            }
        }
        return WasNotAdded;
    }

    public string DumpAsString(string operation, object dumpAsStringHeaderArgs)
    {
        throw new Exception("Cannot be here because DumpListAsStringOneLine was moved to sunamo and will stay there");
    }
}
