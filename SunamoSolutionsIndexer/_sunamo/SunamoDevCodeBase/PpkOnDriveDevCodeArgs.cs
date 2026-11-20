namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

/// <summary>
/// Arguments of a list persisted in a file.
/// </summary>
public class PpkOnDriveDevCodeArgs
{
    /// <summary>
    /// Gets or sets the file.
    /// </summary>
    public string File { get; set; } = null!;

    // Originally was false but think that true is better
    // Its not important because still I'm using old ctor interface and it will set to false if needed
    /// <summary>
    /// Gets or sets the load.
    /// </summary>
    public bool Load { get; set; } = true;

    /// <summary>
    /// Gets or sets the load changes from drive.
    /// </summary>
    public bool LoadChangesFromDrive { get; set; } = true;
    /// <summary>
    /// Gets or sets the save.
    /// </summary>
    public bool Save { get; set; } = true;
}