namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

// Base arguments class for getting files
/// <summary>
/// Base arguments for getting files.
/// </summary>
internal class GetFilesBaseArgsDC /*: GetFoldersEveryFolderArgs - do not inherit - read comment above*/
{
    internal bool FollowJunctions { get; set; } = false;
    internal Func<string, bool>? IsJunctionPoint { get; set; } = null;
    internal bool TrimA1AndLeadingBs { get; set; } = false;
}