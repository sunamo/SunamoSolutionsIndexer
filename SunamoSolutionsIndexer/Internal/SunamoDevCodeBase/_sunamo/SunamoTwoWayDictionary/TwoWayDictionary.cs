namespace SunamoDevCode._sunamo.SunamoTwoWayDictionary;

internal class TwoWayDictionary<T, U> where T : notnull where U : notnull
{
    internal Dictionary<T, U> FirstToSecond { get; set; } = null!;
    internal Dictionary<U, T> SecondToFirst { get; set; } = null!;

    internal TwoWayDictionary(int capacity)
    {
        FirstToSecond = new Dictionary<T, U>(capacity);
        SecondToFirst = new Dictionary<U, T>(capacity);
    }

    internal TwoWayDictionary()
    {
        FirstToSecond = new Dictionary<T, U>();
        SecondToFirst = new Dictionary<U, T>();
    }

    internal void Add(T key, U value)
    {
        FirstToSecond.Add(key, value);
        SecondToFirst.Add(value, key);
    }
}