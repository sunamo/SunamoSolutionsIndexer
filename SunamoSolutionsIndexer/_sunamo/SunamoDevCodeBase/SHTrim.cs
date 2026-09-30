namespace SunamoSolutionsIndexer._sunamo;

/// <summary>
/// Trimming of strings.
/// </summary>
internal class SHTrim
{

    // EN: Method TrimLeadingNumbersAtStart was removed - inlined in ConstsManager.cs:110

    /// <summary>
    /// Trims the given characters or suffix from the end.
    /// </summary>
    internal static string TrimEnd(string text, string suffix)
    {
        while (text.EndsWith(suffix)) return text.Substring(0, text.Length - suffix.Length);

        return text;
    }

    // EN: Method TrimStartAndEnd was removed - inlined in XmlLocalisationInterchangeFileFormat2.cs:691

    /// <summary>
    /// Trims the prefix from the start of the text.
    /// </summary>
    internal static string TrimStart(string text, string prefix)
    {
        while (text.StartsWith(prefix))
        {
            text = text.Substring(prefix.Length);
        }

        return text;
    }

}
