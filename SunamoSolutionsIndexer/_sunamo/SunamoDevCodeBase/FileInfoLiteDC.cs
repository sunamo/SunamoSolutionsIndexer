namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

/// <summary>
/// Lightweight description of a file.
/// </summary>
public class FileInfoLiteDC
{
    /// <summary>
    /// Gets or sets the path.
    /// </summary>
    public string? Path { get; set; } = null;

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string? Name { get; set; } = null;

    /// <summary>
    /// Gets or sets the size.
    /// </summary>
    public long Size { get; set; } = 0;

    /// <summary>
    /// Gets or sets the directory.
    /// </summary>
    public string? Directory { get; set; } = null;

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public FileInfoLiteDC()
    {
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public FileInfoLiteDC(string directory, string fileName, long length)
    {
        this.Directory = directory;
        Name = fileName;
        Size = length;
    }

    /// <summary>
    /// Creates the description of the file.
    /// </summary>
    public static FileInfoLiteDC GetFIL(FileInfo fileInfo)
    {
        var fileInfoLite = new FileInfoLiteDC
        {
            Name = fileInfo.Name,
            Path = fileInfo.FullName,
            Directory = fileInfo.DirectoryName!,
            Size = fileInfo.Length
        };
        return fileInfoLite;
    }

    /// <summary>
    /// Creates the description of the file.
    /// </summary>
    public static FileInfoLiteDC GetFIL(string filePath)
    {
        var fileInfo = new FileInfo(filePath);
        return GetFIL(fileInfo);
    }
}