namespace SunamoDevCode._sunamo;

internal class TF
{
    internal static async Task<List<string>?> ReadAllLines(string filePath)
    {
        return (await FileAsync.ReadAllLinesAsync(filePath)).ToList();
    }

    internal static async Task<string?> ReadAllText(string filePath)
    {
        return await FileAsync.ReadAllTextAsync(filePath);
    }

    internal static async Task WriteAllLines(string filePath, List<string> lines)
    {
        await FileAsync.WriteAllLinesAsync(filePath, lines);
    }

    internal static async Task WriteAllText(string filePath, string content)
    {
        await FileAsync.WriteAllTextAsync(filePath, content);
    }
}
