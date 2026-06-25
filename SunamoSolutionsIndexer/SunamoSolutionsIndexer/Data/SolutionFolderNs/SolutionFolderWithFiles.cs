namespace SunamoSolutionsIndexer.Data.SolutionFolderNs;

public class SolutionFolderWithFiles : SolutionFolder
{
    public List<string> Files { get; set; } = null!;

    public Dictionary<string, List<string>> FilesOfExtension { get; set; } = null!;

    #region Filled in CheckSize()

    public Dictionary<int, long> FilesAndSizes { get; set; } = null!;

    public Dictionary<TypeOfExtensionDC, long> SizeOfExtensionTypes { get; set; } = null!;

    // Gets or sets the dictionary of total size by extension (all extensions are lowercase and without dot).
    public Dictionary<string, long> SizeOfExtension { get; set; } = null!;

    public long OverallSize { get; set; } = 0;

    #endregion

    // Gets or sets the dictionary of FileInfoLite objects grouped by extension.
    // Is filled in method CreateFileInfoLiteObjects.
    public Dictionary<string, List<FileInfoLiteDC>> FileInfoLiteOfExtension { get; set; } = null!;

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

    public void CheckSize()
    {
        // Method body is currently commented out in original implementation
    }

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