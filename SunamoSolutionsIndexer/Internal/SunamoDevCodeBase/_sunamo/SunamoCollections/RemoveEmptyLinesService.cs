namespace SunamoDevCode._sunamo.SunamoCollections;

internal class RemoveEmptyLinesService
{
    internal void RemoveEmptyLinesFromStartAndEnd(List<string> lines)
    {
        RemoveEmptyLinesToFirstNonEmpty(lines);
        RemoveEmptyLinesFromBack(lines);
    }

    internal void RemoveEmptyLinesToFirstNonEmpty(List<string> lines)
    {
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (line.Trim() == string.Empty)
            {
                lines.RemoveAt(i);
                i--;
            }
            else
            {
                break;
            }
        }
    }

    internal void RemoveEmptyLinesFromBack(List<string> lines)
    {
        for (var i = lines.Count - 1; i >= 0; i--)
        {
            var line = lines[i];
            if (line.Trim() == string.Empty)
                lines.RemoveAt(i);
            else
                break;
        }
    }
}
