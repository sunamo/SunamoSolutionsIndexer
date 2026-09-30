namespace SunamoDevCode._sunamo.SunamoStringParts;

internal class SHParts
{
    internal static string RemoveAfterFirst(string text, string searchString)
    {
        int index = text.IndexOf(searchString);
        if (index == -1 || index == text.Length - 1)
        {
            return text;
        }

        string result = text.Remove(index);
        return result;
    }
    // Metoda KeepAfterFirst byla odstraněna - inlined v TypeScriptHelper.cs:79

    // Metoda RemoveAfterLast byla odstraněna - inlined v SolutionFolderSerialize.cs:57

    internal static string RemoveAfterFirst(string text, char searchChar)
    {
        int index = text.IndexOf(searchChar);
        return index == -1 || index == text.Length - 1 ? text : text.Substring(0, index);
    }

}
