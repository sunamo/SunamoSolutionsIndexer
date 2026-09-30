namespace SunamoDevCode;

internal class CSharpParser
{
    public const string PublicModifier = "public ";

    public const string StaticReadonlyModifier = "static readonly ";

    public static
        async Task
        RemoveConsts(string file, List<string> remove)
    {
        remove.Insert(0, null!);

        // EN: Inlined from CAIndexesWithNull.IndexesWithNull - gets indexes of null values in collection
        // CZ: Inlined from CAIndexesWithNull.IndexesWithNull - získává indexy null hodnot v kolekci
        List<int> nullIndexes = [];
        int index = 0;
        foreach (var item in remove)
        {
            if (item == null)
            {
                nullIndexes.Add(index);
            }
            index++;
        }

        var lines = SHGetLines.GetLines(
            await
                FileAsync.ReadAllTextAsync(file)).ToList();

        for (var i = lines.Count - 1; i >= 0; i--)
        {
            var text = lines[i].Trim();
            if (text.Contains(XmlLocalisationInterchangeFileFormatSunamo.Cs))
            {
                var key = XmlLocalisationInterchangeFileFormatSunamo.GetConstsFromLine(text);
                var keyIndex = remove.IndexOf(key);
                if (keyIndex != -1)
                {
                    lines.RemoveAt(i);
                    remove.RemoveAt(keyIndex);
                }
            }
        }

        await FileAsync.WriteAllLinesAsync(file, lines);
        if (remove.Count > 0)
        {
            throw new Exception("Cant be deleted in XlfKeys: " + string.Join(",", remove));
        }
    }

    public static List<string> ParseConsts(List<string> lines, out int first)
    {
        var keys = new List<string>();
        first = -1;
        for (var i = 0; i < lines.Count; i++)
        {
            var text = lines[i].Trim();
            if (text.Contains(XmlLocalisationInterchangeFileFormatSunamo.Cs))
            {
                if (first == -1) first = i;

                var key = XmlLocalisationInterchangeFileFormatSunamo.GetConstsFromLine(text);
                keys.Add(key);
            }
        }

        return keys;
    }

    public static List<string> ParseConstsAllLines(List<string> lines)
    {
        var keys = new List<string>();
        for (var i = 0; i < lines.Count; i++)
        {
            var text = lines[i].Trim();
            var key = XmlLocalisationInterchangeFileFormatSunamo.GetConstsFromLine(text);
            keys.Add(key);
        }

        return keys;
    }
}
