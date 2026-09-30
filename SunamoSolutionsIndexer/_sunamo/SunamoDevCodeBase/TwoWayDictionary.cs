namespace SunamoSolutionsIndexer._sunamo;

/// <summary>
/// Dictionary that can be searched by key and by value.
/// </summary>
internal class TwoWayDictionary<T, U> where T : notnull where U : notnull
{
    internal Dictionary<T, U> FirstToSecond { get; set; } = null!;
    internal Dictionary<U, T> SecondToFirst { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    internal TwoWayDictionary(int capacity)
    {
        FirstToSecond = new Dictionary<T, U>(capacity);
        SecondToFirst = new Dictionary<U, T>(capacity);
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    internal TwoWayDictionary()
    {
        FirstToSecond = new Dictionary<T, U>();
        SecondToFirst = new Dictionary<U, T>();
    }

    /// <summary>
    /// Adds the value.
    /// </summary>
    internal void Add(T key, U value)
    {
        FirstToSecond.Add(key, value);
        SecondToFirst.Add(value, key);
    }
}