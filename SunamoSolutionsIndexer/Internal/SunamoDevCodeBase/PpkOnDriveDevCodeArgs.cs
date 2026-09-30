namespace SunamoSolutionsIndexer.Internal;

public class PpkOnDriveDevCodeArgs
{
    public string File { get; set; } = null!;

    // Originally was false but think that true is better
    // Its not important because still I'm using old ctor interface and it will set to false if needed
    public bool Load { get; set; } = true;

    public bool LoadChangesFromDrive { get; set; } = true;
    public bool Save { get; set; } = true;
}
