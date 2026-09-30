namespace SunamoDevCode._sunamo;

internal class FSGetFolders
{
    internal static List<string> GetFoldersEveryFolderWhichContainsFiles(ILogger logger, string directory, string pattern, SearchOption searchOption)
    {
        try
        {
            var folders = Directory.GetDirectories(directory, "*", searchOption);
            var result = new List<string>();
            foreach (var folder in folders)
            {
                var files = Directory.GetFiles(folder, pattern, searchOption).ToList();
                if (files.Count != 0) result.Add(folder);
            }
            result = result.ConvertAll(folderPath => folderPath + "\\");
            return result;

        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new List<string>();
        }
    }

    internal static IEnumerable<string> GetFolders(string path) => Directory.GetDirectories(path);
}
