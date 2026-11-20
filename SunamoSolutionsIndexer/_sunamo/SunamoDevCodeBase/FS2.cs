namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

/// <summary>
/// Helpers for working with file system paths.
/// </summary>
internal partial class FS
{

    /// <summary>
    /// Returns only file names of the paths.
    /// </summary>
    internal static List<string> OnlyNamesNoDirectEdit(String[] filePaths)
    {
        var list = filePaths.ToList();
        return OnlyNamesNoDirectEdit(list);
    }

    // No direct edit
    // Returns with extension
    // WARNING: Unlike the same method in sunamo, this method returns a completely new array and does not modify A1
    /// <summary>
    /// Returns only file names of the paths.
    /// </summary>
    internal static List<string> OnlyNamesNoDirectEdit(List<string> filePaths)
    {
        var fileNames = new List<string>(filePaths.Count);
        for (int i = 0; i < filePaths.Count; i++)
        {
            fileNames.Add(Path.GetFileName(filePaths[i]));
        }

        return fileNames;
    }
}