namespace SunamoSolutionsIndexer._sunamo;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
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
    // POZOR: Na rozdíl od stejné metody v sunamo tato metoda vrací úplně nové pole a nemodifikuje A1
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