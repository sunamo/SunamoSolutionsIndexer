namespace SunamoSolutionsIndexer._sunamo;

/// <summary>
/// Helpers for working with strings.
/// </summary>
internal class SH
{

    /// <summary>
    /// Returns the text between the delimiters.
    /// </summary>
    internal static string GetTextBetweenSimple(string text, string afterDelimiter, string beforeDelimiter, bool isThrowExceptionIfNotContains = true)
    {
        int foundIndex = int.MinValue;
        var result = GetTextBetween(text, afterDelimiter, beforeDelimiter, out foundIndex, 0, isThrowExceptionIfNotContains);
        return result!;
    }

    /// <summary>
    /// Returns the text between the delimiters or null.
    /// </summary>
    internal static string? GetTextBetween(string text, string afterDelimiter, string beforeDelimiter, out int foundIndex, int startSearchingAt, bool isThrowExceptionIfNotContains = true)
    {
        string? result = null;
        foundIndex = text.IndexOf(afterDelimiter, startSearchingAt);
        int beforeIndex = text.IndexOf(beforeDelimiter, foundIndex + afterDelimiter.Length);
        bool isAfterFound = foundIndex != -1;
        bool isBeforeFound = beforeIndex != -1;
        if (isAfterFound && isBeforeFound)
        {
            foundIndex += afterDelimiter.Length;
            beforeIndex -= 1;
            // When I return between ( ), there must be +1
            var length = beforeIndex - foundIndex + 1;
            if (length < 1)
            {
                // EN: This was here before but logically it's nonsense
                // CZ: Takhle to tu bylo předtím ale logicky je to nesmysl
            }
            result = text.Substring(foundIndex, length).Trim();
        }
        else
        {
            if (isThrowExceptionIfNotContains)
            {
                ThrowEx.NotContains(text, afterDelimiter, beforeDelimiter);
            }
            else
            {
                // 24-1-21 return null instead of text
                return null;
                //result = text;
            }
        }

        return result;
    }

    /// <summary>
    /// Wraps the value(s) with the given prefix and suffix.
    /// </summary>
    internal static string WrapWith(string value, string wrapper)
    {
        return wrapper + value + wrapper;
    }

    /// <summary>
    /// Wraps the value with quotation marks.
    /// </summary>
    internal static string WrapWithQm(string value)
    {
        var wrapper = "\"";
        return wrapper + value + wrapper;
    }


    /// <summary>
    /// Converts the first character to upper case.
    /// </summary>
    internal static string FirstCharUpper(string text)
    {
        if (text.Length == 1)
        {
            return text.ToUpper();
        }

        string rest = text.Substring(1);
        return text[0].ToString().ToUpper() + rest;
    }

    /// <summary>
    /// Checks whether the name matches the wildcard mask.
    /// </summary>
    internal static bool MatchWildcard(string name, string mask)
    {
        return IsMatchRegex(name, mask, '?', '*');
    }

    /// <summary>
    /// Checks the text against the pattern with wildcards converted to regex.
    /// </summary>
    private static bool IsMatchRegex(string text, string pattern, char singleWildcard, char multipleWildcard)
    {
        // If I compared .vs with .vs, return false before
        if (text == pattern)
        {
            return true;
        }

        string escapedSingle = Regex.Escape(new string(singleWildcard, 1));
        string escapedMultiple = Regex.Escape(new string(multipleWildcard, 1));
        pattern = Regex.Escape(pattern);
        pattern = pattern.Replace(escapedSingle, ".");
        pattern = "^" + pattern.Replace(escapedMultiple, ".*") + "$";
        Regex regex = new(pattern);
        return regex.IsMatch(text);
    }


    /// <summary>
    /// Removes diacritics from the text.
    /// </summary>
    internal static string TextWithoutDiacritic(string projName)
    {
        return projName.RemoveDiacritics();
    }
}
