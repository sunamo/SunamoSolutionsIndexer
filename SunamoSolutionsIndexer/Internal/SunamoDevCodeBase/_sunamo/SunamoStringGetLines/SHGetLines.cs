namespace SunamoDevCode._sunamo.SunamoStringGetLines;

internal class SHGetLines
{
    internal static List<string> GetLines(string text)
    {
        var lines = text.Split(new string[] { "\r\n", "\n\r" }, StringSplitOptions.None).ToList();
        SplitByUnixNewline(lines);
        return lines;
    }

    private static void SplitByUnixNewline(List<string> lines)
    {
        SplitBy(lines, "\r");
        SplitBy(lines, "\n");
    }

    private static void SplitBy(List<string> lines, string delimiter)
    {
        for (int i = lines.Count - 1; i >= 0; i--)
        {
            if (delimiter == "\r")
            {
                var windowsNewlineParts = lines[i].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                var reverseNewlineParts = lines[i].Split(new string[] { "\n\r" }, StringSplitOptions.None);

                if (windowsNewlineParts.Length > 1)
                {
                    ThrowEx.Custom("cannot contain any \r\n, pass already split by this pattern");
                }
                else if (reverseNewlineParts.Length > 1)
                {
                    ThrowEx.Custom("cannot contain any \n\r, pass already split by this pattern");
                }
            }

            var splitParts = lines[i].Split(new string[] { delimiter }, StringSplitOptions.None);

            if (splitParts.Length > 1)
            {
                InsertOnIndex(lines, splitParts.ToList(), i);
            }
        }
    }

    private static void InsertOnIndex(List<string> lines, List<string> splitLines, int index)
    {
        splitLines.Reverse();

        lines.RemoveAt(index);

        foreach (var item in splitLines)
        {
            lines.Insert(index, item);
        }
    }
}
