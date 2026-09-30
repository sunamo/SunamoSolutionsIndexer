namespace SunamoSolutionsIndexer._sunamo;

// TODO: Should this inherit from GetFoldersEveryFolderArgs? In vs2 it does
/// <summary>
/// Arguments for getting files.
/// </summary>
internal class GetFilesArgsDC : GetFilesBaseArgsDC
{
    // TODO: This class has issues - clean up what should and shouldn't be here
    internal bool TrimExt { get; set; } = false;
    internal new bool TrimA1AndLeadingBs { get; set; } = false;
    internal List<string> ExcludeFromLocationsContains { get; set; } = new List<string>();
    internal bool DontIncludeNewest { get; set; } = false;
    // EN: Method to exclude files, e.g. SunamoDevCodeHelper.RemoveTemporaryFilesVS
    // CZ: Metoda pro vyloučení souborů, např. SunamoDevCodeHelper.RemoveTemporaryFilesVS
    internal Action<List<string>>? ExcludeWithMethod { get; set; } = null;
    internal bool ByDateOfLastModifiedAsc { get; set; } = false;
    internal Func<string, DateTime?>? LastModifiedFromFunc { get; set; }
    // EN: Changed to false on 1-7-2020, still forgot to mention and method is problematic
    // CZ: Změněno na false dne 1-7-2020, stále zapomenuto zmínit a metoda je problematická
    internal bool UseMascFromExtension { get; set; } = false;
    internal bool Wildcard { get; set; } = false;
}
