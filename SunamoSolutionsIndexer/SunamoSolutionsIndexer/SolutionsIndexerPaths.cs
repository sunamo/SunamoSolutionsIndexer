// variables names: ok
// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode.SunamoSolutionsIndexer;

/// <summary>
/// Paths to data files used by the solutions indexer.
/// </summary>
public class SolutionsIndexerPaths
{
    /// <summary>
    /// Path to the VPS new solutions list file.
    /// </summary>
    public static readonly string? listVpsNew = null; // AppData.Instance.GetFile(AppFolders.Data, "SlnVpsNew.txt");
    /// <summary>
    /// Path to the SczAdmin64 solutions list file.
    /// </summary>
    public static readonly string? listSczAdmin64 = null; // AppData.Instance.GetFile(AppFolders.Data, "SlnSczAdmin64.txt");
}
