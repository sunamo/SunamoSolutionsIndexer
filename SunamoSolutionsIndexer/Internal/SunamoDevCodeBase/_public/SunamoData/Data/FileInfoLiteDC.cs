namespace SunamoSolutionsIndexer.Internal._public.SunamoData.Data;

public class FileInfoLiteDC
{
    public string? Path { get; set; } = null;

    public string? Name { get; set; } = null;

    public string FileName => Name!;

    public long Size { get; set; } = 0;

    public long Length => Size;

    public string? Directory { get; set; } = null;

    public FileInfoLiteDC()
    {
    }

    public FileInfoLiteDC(string directory, string fileName, long length)
    {
        this.Directory = directory;
        Name = fileName;
        Size = length;
    }

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

    public static FileInfoLiteDC GetFIL(string filePath)
    {
        var fileInfo = new FileInfo(filePath);
        return GetFIL(fileInfo);
    }
}
