namespace SunamoSolutionsIndexer._sunamo;

// Checking whether string is already contained.
/// <summary>
/// List of strings persisted in a file.
/// </summary>
public class PpkOnDriveDC : PpkOnDriveDevCodeBase<string>
{
    public bool RemoveDuplicates { get; set; } = false;

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public PpkOnDriveDC(PpkOnDriveDevCodeArgs args) : base(args)
    {
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public PpkOnDriveDC(string filePath, bool isLoading = true) : base(new PpkOnDriveDevCodeArgs { File = filePath, Load = isLoading })
    {
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public PpkOnDriveDC(string filePath, bool isLoading, bool isSaving) : base(new PpkOnDriveDevCodeArgs
    { File = filePath, Load = isLoading, Save = isSaving })
    {
    }

    public override
        async Task
        Load()
    {
        if (File.Exists(args.File))
        {
            AddRange(SHGetLines.GetLines(
                await
                    FileAsync.ReadAllTextAsync(args.File)));
            //CA.RemoveStringsEmpty2(this);
            if (RemoveDuplicates)
            {
                //CAG.RemoveDuplicitiesList<string>(this);
                var data = this.ToList();
                await Clear();
                data = data.Distinct().ToList();
                AddRange(data);
            }
        }
    }
}
