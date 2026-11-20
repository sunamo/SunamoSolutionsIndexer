namespace SunamoSolutionsIndexer.Data.SolutionFolderNs;

/// <summary>
/// Represents the solution folder with files type.
/// </summary>
public class SolutionFolderWithFiles : SolutionFolder
{
    /// <summary>
    /// Gets or sets the files.
    /// </summary>
    public List<string> Files { get; set; } = null!;

    /// <summary>
    /// Gets or sets the files of extension.
    /// </summary>
    public Dictionary<string, List<string>> FilesOfExtension { get; set; } = null!;

    #region Filled in CheckSize()

    /// <summary>
    /// Gets or sets the files and sizes.
    /// </summary>
    public Dictionary<int, long> FilesAndSizes { get; set; } = null!;

    /// <summary>
    /// Gets or sets the size of extension types.
    /// </summary>
    public Dictionary<TypeOfExtensionDC, long> SizeOfExtensionTypes { get; set; } = null!;

    // Gets or sets the dictionary of total size by extension (all extensions are lowercase and without dot).
    /// <summary>
    /// Gets or sets the size of extension.
    /// </summary>
    public Dictionary<string, long> SizeOfExtension { get; set; } = null!;

    /// <summary>
    /// Gets or sets the overall size.
    /// </summary>
    public long OverallSize { get; set; } = 0;

    #endregion

    // Gets or sets the dictionary of FileInfoLite objects grouped by extension.
    // Is filled in method CreateFileInfoLiteObjects.
    /// <summary>
    /// Gets or sets the file info lite of extension.
    /// </summary>
    public Dictionary<string, List<FileInfoLiteDC>> FileInfoLiteOfExtension { get; set; } = null!;

    /// <summary>
    /// Solution folder with files.
    /// </summary>
    /// <param name="dot">The dot.</param>
    /// <returns>The solution folder with files result.</returns>
    public SolutionFolderWithFiles(SolutionFolder solutionFolder)
    {
        CountOfImages = solutionFolder.CountOfImages;
        DisplayedText = solutionFolder.DisplayedText;
        FullPathFolder = solutionFolder.FullPathFolder;
        NameSolutionWithoutDiacritic = solutionFolder.NameSolutionWithoutDiacritic;

        FilesAndSizes = new Dictionary<int, long>();
        SizeOfExtensionTypes = new Dictionary<TypeOfExtensionDC, long>();
        SizeOfExtension = new Dictionary<string, long>();

        Files = Directory.GetFiles(FullPathFolder, "*", SearchOption.AllDirectories).ToList();
        FilesOfExtension = new Dictionary<string, List<string>>();

        for (int i = 0; i < Files.Count; i++)
        {
            var item = Files[i];
            string extension = Path.GetExtension(item).TrimStart('.');

            DictionaryHelper.AddOrCreate(FilesOfExtension, extension, item);
        }
    }

    /// <summary>
    /// Check size.
    /// </summary>
    /// <returns>The check size result.</returns>
    public void CheckSize()
    {
        // Method body is currently commented out in original implementation
    }

    /// <summary>
    /// Create file info lite objects.
    /// </summary>
    /// <returns>The create file info lite objects result.</returns>
    public void CreateFileInfoLiteObjects(string extensionWithoutDot, string item)
    {
        var fileInfoLite = FileInfoLiteDC.GetFIL(item);
        if (FileInfoLiteOfExtension.ContainsKey(extensionWithoutDot))
        {
            FileInfoLiteOfExtension[extensionWithoutDot].Add(fileInfoLite);
        }
        else
        {
            var list = new List<FileInfoLiteDC>();
            list.Add(fileInfoLite);
            FileInfoLiteOfExtension.Add(extensionWithoutDot, list);
        }
    }
}