namespace SunamoDevCode._public;

// EN: Base arguments class for getting files
// CZ: Základní třída argumentů pro získávání souborů
internal class GetFilesBaseArgsDC /*: GetFoldersEveryFolderArgs - do not inherit - read comment above*/
{
    internal bool FollowJunctions { get; set; } = false;
    internal Func<string, bool>? IsJunctionPoint { get; set; } = null;
    internal bool TrimA1AndLeadingBs { get; set; } = false;
}
