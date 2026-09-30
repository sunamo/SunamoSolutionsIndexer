namespace SunamoDevCode._sunamo.SunamoString;

internal class SH
{
    #region SH.FirstCharUpper


    #endregion
    internal static bool IsContained(string text, string pattern)
    {
        var (isNegated, actualPattern) = IsNegationTuple(pattern);
        pattern = actualPattern;

        if (isNegated && text.Contains(pattern))
            return false;
        if (!isNegated && !text.Contains(pattern)) return false;

        return true;
    }

    internal static (bool, string) IsNegationTuple(string pattern)
    {
        if (pattern[0] == '!')
        {
            pattern = pattern.Substring(1);
            return (true, pattern);
        }

        return (false, pattern);
    }

    internal static int OccurencesOfStringIn(string source, string searchText)
    {
        return source.Split(new[] { searchText }, StringSplitOptions.None).Length - 1;
    }

    internal static bool ContainsAtLeastOne(string text, List<string> list)
    {
        foreach (var item in list)
        {
            if (text.Contains(item))
            {
                return true;
            }
        }
        return false;
    }

    internal static string GetLineFromCharIndex(string content, List<string> lines, int characterIndex)
    {
        var lineIndex = GetLineIndexFromCharIndex(content, characterIndex);
        return lines[lineIndex];
    }

    // EN: Return index, therefore x-1
    // CZ: Vrátí index, proto x-1
    internal static int GetLineIndexFromCharIndex(string text, int characterPosition)
    {
        var lineNumber = text.Take(characterPosition).Count(character => character == '\n') + 1;
        return lineNumber - 1;
    }

    internal static string GetTextBetweenSimple(string text, string afterDelimiter, string beforeDelimiter, bool isThrowExceptionIfNotContains = true)
    {
        int foundIndex = int.MinValue;
        var result = GetTextBetween(text, afterDelimiter, beforeDelimiter, out foundIndex, 0, isThrowExceptionIfNotContains);
        return result!;
    }

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

    internal static string WhiteSpaceFromStart(string text)
    {
        StringBuilder stringBuilder = new();
        foreach (var item in text)
        {
            if (char.IsWhiteSpace(item))
            {
                stringBuilder.Append(item);
            }
            else
            {
                break;
            }
        }
        return stringBuilder.ToString();
    }

    internal static string PrefixIfNotStartedWith(string item, string http, bool skipWhitespaces = false)
    {
        string whitespaces = string.Empty;

        if (skipWhitespaces)
        {
            whitespaces = WhiteSpaceFromStart(item);
            item = item.Substring(whitespaces.Length);
        }

        if (!item.StartsWith(http))
        {
            return whitespaces + http + item;
        }

        return whitespaces + item;
    }

    internal static string WrapWith(string value, string wrapper)
    {
        return wrapper + value + wrapper;
    }

    internal static string WrapWithQm(string value)
    {
        var wrapper = "\"";
        return wrapper + value + wrapper;
    }

    internal static string WrapWithBs(string value)
    {
        var wrapper = "\\";
        return wrapper + value + wrapper;
    }

    #region SH.FirstCharUpper
    internal static string FirstCharUpper(ref string text)
    {
        text = FirstCharUpper(text);
        return text;
    }


    internal static string FirstCharUpper(string text)
    {
        if (text.Length == 1)
        {
            return text.ToUpper();
        }

        string rest = text.Substring(1);
        return text[0].ToString().ToUpper() + rest;
    }
    #endregion

    internal static bool MatchWildcard(string name, string mask)
    {
        return IsMatchRegex(name, mask, '?', '*');
    }

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


    internal static (string, string) GetPartsByLocationNoOut(string text, char delimiter)
    {
        GetPartsByLocation(out var before, out var after, text, delimiter);
        return (before, after);
    }

    internal static void GetPartsByLocation(out string before, out string after, string text, char delimiter)
    {
        int index = text.IndexOf(delimiter);
        GetPartsByLocation(out before, out after, text, index);
    }

    internal static void GetPartsByLocation(out string before, out string after, string text, int position)
    {
        if (position == -1)
        {
            before = text;
            after = "";
        }
        else
        {
            before = text.Substring(0, position);
            if (text.Length > position + 1)
            {
                after = text.Substring(position + 1);
            }
            else
            {
                after = string.Empty;
            }
        }
    }

    internal static List<int> ReturnOccurencesOfString(string text, string searchText)
    {

        List<int> results = new();
        for (int index = 0; index < (text.Length - searchText.Length) + 1; index++)
        {
            var substring = text.Substring(index, searchText.Length);
            ////////DebugLogger.Instance.WriteLine(substring);
            // non-breaking space. &nbsp; code 160
            // 32 space
            _ = substring[0];
            _ = searchText[0];
            if (substring == "")
            {
            }
            if (substring == searchText)
                results.Add(index);
        }
        return results;
    }


    internal static string TextWithoutDiacritic(string projName)
    {
        return projName.RemoveDiacritics();
    }

    internal static bool ContainsDiacritic(string target)
    {
        return target.HasDiacritics();
    }

    internal static string GetTextBetween(string content, string start, string end, bool throwExceptionIfNotContains = true)
    {
        return GetTextBetweenSimple(content, start, end, throwExceptionIfNotContains);
    }
}
