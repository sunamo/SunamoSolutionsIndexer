namespace SunamoDevCode._sunamo.SunamoStringReplace;

internal class SHReplace
{
    internal static string ReplaceWithIndex(string text, string searchValue, string replacement, ref int foundIndex)
    {
        if (foundIndex == -1)
        {
            foundIndex = text.IndexOf(searchValue);
            if (foundIndex != -1)
            {
                text = text.Remove(foundIndex, searchValue.Length);
                text = text.Insert(foundIndex, replacement);
            }
        }

        return text;
    }

    internal static string ReplaceAll(string text, string replacement, params string[] searchValues)
    {
        foreach (var item in searchValues)
        {
            if (string.IsNullOrEmpty(item))
            {
                return text;
            }
        }

        foreach (var item in searchValues)
        {
            text = text.Replace(item, replacement);
        }
        return text;
    }

    internal static string ReplaceOnce(string input, string pattern, string replacement)
    {
        return new Regex(pattern).Replace(input, replacement, 1);
    }
}
