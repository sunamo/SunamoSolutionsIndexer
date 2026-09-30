namespace SunamoDevCode._sunamo;

internal class DictionaryHelper
{
    // Metoda AppendLineOrCreate byla odstraněna - inlined v InWeb.cs:161
    internal static IList<string> GetIfExists(Dictionary<string, List<string>> filesInSolutionReal, string prefix,
       string extension, bool postfixWithA2)
    {
        if (filesInSolutionReal.ContainsKey(extension))
        {
            var result = filesInSolutionReal[extension];
            if (postfixWithA2)
            {
                if (!string.IsNullOrEmpty(extension)) result = CA.PostfixIfNotEnding(extension, result);
                CA.Prepend(prefix, result);
            }

            return result;
        }

        return new List<string>();
    }
    internal static void AddOrCreateIfDontExists<Key, Value>(Dictionary<Key, List<Value>> dictionary, Key key, Value value) where Key : notnull
    {
        if (dictionary.ContainsKey(key))
        {
            if (!dictionary[key].Contains(value))
            {
                dictionary[key].Add(value);
            }
        }
        else
        {
            List<Value> newList = new List<Value>();
            newList.Add(value);
            dictionary.Add(key, newList);
        }
    }

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

    #region AddOrCreate když key i value není list
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
    internal static void AddOrCreate<Key, Value>(IDictionary<Key, List<Value>> dictionary, Key key, Value value,
    bool withoutDuplicitiesInValue = false, Dictionary<Key, List<string>>? dictS = null) where Key : notnull
    {
        AddOrCreate<Key, Value, object>(dictionary, key, value, withoutDuplicitiesInValue, dictS);
    }
    #endregion

    internal static Dictionary<Key, Value> GetDictionary<Key, Value>(List<Key> keys, List<Value> values) where Key : notnull
    {
        ThrowEx.DifferentCountInLists("keys", keys.Count, "values", values.Count);
        Dictionary<Key, Value> result = new Dictionary<Key, Value>();
        for (int i = 0; i < keys.Count; i++)
        {
            result.Add(keys[i], values[i]);
        }
        return result;
    }

    internal static Value? GetFirstItemValue<Key, Value>(Dictionary<Key, Value> dict) where Key : notnull
    {
        foreach (var item in dict)
        {
            return item.Value;
        }

        return default(Value);
    }
}
