namespace SunamoDevCode.SunamoSolutionsIndexer.Data.SolutionFolderNs;

/// <summary>
/// Solution folder with file information.
/// </summary>
public class SolutionFolderWithFiles : SolutionFolder
{
    /// <summary>
    /// Gets or sets the list of all files in the solution.
    /// </summary>
    public List<string> Files { get; set; } = null!;

    /// <summary>
    /// Gets or sets the dictionary of files grouped by extension (without dot).
    /// </summary>
    public Dictionary<string, List<string>> FilesOfExtension { get; set; } = null!;

    #region Filled in CheckSize()

    /// <summary>
    /// Gets or sets the dictionary mapping file index to file size.
    /// </summary>
    public Dictionary<int, long> FilesAndSizes { get; set; } = null!;

    /// <summary>
    /// Gets or sets the dictionary of total size by extension type.
    /// </summary>
    public Dictionary<TypeOfExtensionDC, long> SizeOfExtensionTypes { get; set; } = null!;

    /// <summary>
    /// Gets or sets the dictionary of total size by extension (all extensions are lowercase and without dot).
    /// </summary>
    public Dictionary<string, long> SizeOfExtension { get; set; } = null!;

    /// <summary>
    /// Gets or sets the overall size of all files.
    /// </summary>
    public long OverallSize { get; set; } = 0;

    #endregion

    /// <summary>
    /// Gets or sets the dictionary of FileInfoLite objects grouped by extension.
    /// Is filled in method CreateFileInfoLiteObjects.
    /// </summary>
    public Dictionary<string, List<FileInfoLiteDC>> FileInfoLiteOfExtension { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionFolderWithFiles"/> class.
    /// </summary>
    /// <param name="solutionFolder">Source solution folder.</param>
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
    /// Checks and calculates the total size of files.
    /// </summary>
    public void CheckSize()
    {
        // Method body is currently commented out in original implementation
    }

    /// <summary>
    /// Creates FileInfoLite objects for the specified extension.
    /// </summary>
    /// <param name="extensionWithoutDot">Extension without dot.</param>
    /// <param name="item">File path.</param>
    public void CreateFileInfoLiteObjects(string extensionWithoutDot, string item)
    {
        var fileInfoLite = FileInfoLiteDC.GetFIL(item);
        if (FileInfoLiteOfExtension.ContainsKey(extensionWithoutDot))
        {
            FileInfoLiteOfExtension[extensionWithoutDot].Add(fileInfoLite);
        }
        else
        {
            List<FileInfoLiteDC> list = new List<FileInfoLiteDC>();
            list.Add(fileInfoLite);
            FileInfoLiteOfExtension.Add(extensionWithoutDot, list);
        }
    }
}