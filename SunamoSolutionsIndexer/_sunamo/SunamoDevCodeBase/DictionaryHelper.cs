namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

/// <summary>
/// Helpers for working with dictionaries.
/// </summary>
internal class DictionaryHelper
{

    /// <summary>
    /// Adds the key or overwrites its value.
    /// </summary>
    internal static void AddOrSet<T1, T2>(IDictionary<T1, T2> dictionary, T1 key, T2 value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    /// <summary>
    /// Adds the value to the list under the key, creating the list when missing.
    /// </summary>
    internal static void AddOrCreate<Key, Value, ColType>(IDictionary<Key, List<Value>> dict, Key key, Value value,
    bool withoutDuplicitiesInValue = false, Dictionary<Key, List<string>>? dictS = null) where Key : notnull
    {
        var compWithString = false;
        if (dictS != null) compWithString = true;

        if (key is IList && typeof(ColType) != typeof(Object))
        {
            var keyE = key as IList<ColType>;
            var contains = false;
            foreach (var item in dict)
            {
                var keyD = item.Key as IList<ColType>;
                if (keyD!.SequenceEqual(keyE!)) contains = true;
            }

            if (contains)
            {
                foreach (var item in dict)
                {
                    var keyD = item.Key as IList<ColType>;
                    if (keyD!.SequenceEqual(keyE!))
                    {
                        if (withoutDuplicitiesInValue)
                            if (item.Value.Contains(value))
                                return;
                        item.Value.Add(value);
                    }
                }
            }
            else
            {
                List<Value> newList = new();
                newList.Add(value);
                dict.Add(key, newList);

                if (compWithString)
                {
                    List<string> newStringList = new();
                    newStringList.Add(value!.ToString()!);
                    dictS!.Add(key, newStringList);
                }
            }
        }
        else
        {
            var add = true;
            lock (dict)
            {
                if (dict.ContainsKey(key))
                {
                    if (withoutDuplicitiesInValue)
                    {
                        if (dict[key].Contains(value))
                            add = false;
                        else if (compWithString)
                            if (dictS![key].Contains(value!.ToString()!))
                                add = false;
                    }

                    if (add)
                    {
                        var val = dict[key];

                        if (val != null) val.Add(value);

                        if (compWithString)
                        {
                            var val2 = dictS![key];

                            if (val != null) val2.Add(value!.ToString()!);
                        }
                    }
                }
                else
                {
                    if (!dict.ContainsKey(key))
                    {
                        List<Value> newList = new();
                        newList.Add(value);
                        dict.Add(key, newList);
                    }
                    else
                    {
                        dict[key].Add(value);
                    }

                    if (compWithString)
                    {
                        if (!dictS!.ContainsKey(key))
                        {
                            List<string> newStringList = new();
                            newStringList.Add(value!.ToString()!);
                            dictS.Add(key, newStringList);
                        }
                        else
                        {
                            dictS[key].Add(value!.ToString()!);
                        }
                    }
                }
            }
        }
    }

    // Pokud A1 bude obsahovat skupinu pod názvem A2, vložím do této skupiny prvek A3
    // Jinak do A1 vytvořím novou skupinu s klíčem A2 s hodnotou A3
    /// <summary>
    /// Adds the value to the list under the key, creating the list when missing.
    /// </summary>
    internal static void AddOrCreate<Key, Value>(IDictionary<Key, List<Value>> dictionary, Key key, Value value,
    bool withoutDuplicitiesInValue = false, Dictionary<Key, List<string>>? dictS = null) where Key : notnull
    {
        AddOrCreate<Key, Value, object>(dictionary, key, value, withoutDuplicitiesInValue, dictS);
    }
}