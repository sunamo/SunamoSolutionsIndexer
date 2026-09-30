namespace SunamoDevCode.Helpers;

internal class TypeScriptHelper
{
    private static Dictionary<string, string> ___types = new Dictionary<string, string>();
    private static Dictionary<string, string> ___defaultValueForType = new Dictionary<string, string>();

    static TypeScriptHelper()
    {
        ___defaultValueForType.Add("string", "\"\"");
        ___defaultValueForType.Add("number", "0");
        ___defaultValueForType.Add("boolean", "false");
        ___defaultValueForType.Add("Date", "dt");
    }

    public static string Type(string typeName)
    {
        if (___types.ContainsKey(typeName))
        {
            return ___types[typeName];
        }

        return typeName;
    }

    public static string DefaultValueForType(string typeName, string prefixIfString = "", /*bool isArgNumber = false,*/ string nameArgMethod = "")
    {
        if (typeName.EndsWith("[]"))
        {
            return "[]";
        }

        if (___defaultValueForType.ContainsKey(typeName))
        {
            var result = ___defaultValueForType[typeName];
            if (typeName == "string")
            {
                result = result.Insert(1, prefixIfString);
                if (nameArgMethod != "")
                {
                    result += " + " + nameArgMethod;
                }
            }
            else if (typeName == "number")
            {
                if (nameArgMethod != "")
                {
                    result = "+" + nameArgMethod;
                }
            }

            return result;
        }

        ThrowEx.NotImplementedCase(typeName);
        return "";
    }


    // Do not use for interfaces unless explicitly allowed to have optional (?) modifiers
    public static Tuple<List<string>, List<string>> GetNamesAndTypes(List<string> list)
    {
        var __typesList = list.ToList();

        CAChangeContent.ChangeContent(new ChangeContentArgsDC { }, list, SHParts.RemoveAfterFirst, ':');
        CA.Trim(list);
        CA.TrimEnd(list, '?');
        CA.Trim(list);

        // Inlined from SHParts.KeepAfterFirst - keeps text after first occurrence of character
        Func<string, string, bool, string> keepAfterFirst = (searchQuery, after, isKeepingDelimiter) =>
        {
            var delimiterIndex = searchQuery.IndexOf(after);
            if (delimiterIndex != -1)
            {
                // TrimStart helper - removes string from beginning
                string result = searchQuery.Substring(delimiterIndex);
                while (result.StartsWith(after))
                {
                    result = result.Substring(after.Length);
                }
                searchQuery = result;
                if (isKeepingDelimiter)
                {
                    searchQuery = after + searchQuery;
                }
            }
            return searchQuery;
        };
        CAChangeContent.ChangeContent(new ChangeContentArgsDC { }, __typesList, keepAfterFirst, ":", false);
        for (int i = 0; i < __typesList.Count; i++)
        {
            var temp = __typesList[i];
            temp = temp.Trim();
            temp = temp.TrimEnd(';');
            temp = temp.Trim('2');
            __typesList[i] = temp;
        }

        return new Tuple<List<string>, List<string>>(list, __typesList);
    }
}
