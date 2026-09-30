namespace SunamoSolutionsIndexer._sunamo;

/// <summary>
/// Lightweight description of a file.
/// </summary>
public class FileInfoLiteDC
{
    public string? Path { get; set; } = null;

    public string? Name { get; set; } = null;

    public long Size { get; set; } = 0;

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
