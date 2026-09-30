namespace SunamoDevCode._sunamo.SunamoStringSplit;

internal class SHSplit
{
    internal static List<string> Split(string text, params string[] delimiters)
    {
        return text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    internal static List<string>? SplitToParts(string what, int parts, string delimiter)
    {
        var text = Split(what.RemoveInvisibleChars(), delimiter);
        if (text.Count < parts)
        {
            // Pokud je pocet ziskanych partu mensi, vlozim do zbytku prazdne retezce
            if (text.Count > 0)
            {
                var paddedResult = new List<string>();
                for (var i = 0; i < parts; i++)
                    if (i < text.Count)
                        paddedResult.Add(text[i]);
                    else
                        paddedResult.Add("");
                return paddedResult;
                //return new string[] { text[0] };
            }

            return null;
        }

        if (text.Count == parts)
            // Pokud pocet ziskanych partu souhlasim presne, vratim jak je
            return text;
        // Pokud je pocet ziskanych partu vetsi nez kolik ma byt, pripojim ty co josu navic do zbytku
        parts--;
        var result = new List<string>();
        for (var i = 0; i < text.Count; i++)
            if (i < parts)
                result.Add(text[i]);
            else if (i == parts)
                result.Add(text[i] + delimiter);
            else if (i != text.Count - 1)
                result[parts] += text[i] + delimiter;
            else
                result[parts] += text[i];
        return result;
    }

    internal static List<string> SplitChar(string text, char delimiter)
    {
        return Split(StringSplitOptions.RemoveEmptyEntries, text, (new List<char>([delimiter]).ConvertAll(character => character.ToString()).ToArray()));
    }

    internal static List<string> Split(StringSplitOptions stringSplitOptions, string text, params string[] delimiter)
    {
        if (delimiter == null || delimiter.Length == 0)
        {
            throw new Exception("NoDelimiterDetermined");
        }
        var result = text.Split(delimiter, stringSplitOptions).ToList();
        CA.Trim(result);
        if (stringSplitOptions == StringSplitOptions.RemoveEmptyEntries)
        {
            result = result.Where(line => line.Trim() != string.Empty).ToList();
        }

        return result;
    }




    internal static List<string> SplitByWhiteSpaces(string text)
    {
        WhitespaceCharService whitespaceChar = new();
        return text.Split(whitespaceChar.WhiteSpaceChars.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    internal static Tuple<string, string> SplitFromReplaceManyFormat(string input)
    {
        StringBuilder to = new StringBuilder();
        StringBuilder from = new StringBuilder();

        if (input.Contains("->"))
        {
            var lines = SHGetLines.GetLines(input);

            lines = lines.ConvertAll(line => line.Trim());

            foreach (var item in lines)
            {
                var parts = SHSplit.Split(item, "->");
                from.AppendLine(parts[0]);
                to.AppendLine(parts[1]);
            }
        }
        else
        {
            from.AppendLine(input);
        }

        return new Tuple<string, string>(from.ToString(), to.ToString());
    }

    internal static Tuple<List<string>, List<string>> SplitFromReplaceManyFormatList(string input)
    {
        var temp = SplitFromReplaceManyFormat(input);
        return new Tuple<List<string>, List<string>>(SHGetLines.GetLines(temp.Item1), SHGetLines.GetLines(temp.Item2));
    }
}
