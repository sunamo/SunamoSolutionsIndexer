namespace SunamoSolutionsIndexer.Data.SolutionFolderNs;

public class SolutionFolderWithFiles : SolutionFolder
{
    public List<string> files = null;
    /// <summary>
    /// Without dot
    /// </summary>
    public Dictionary<string, List<string>> filesOfExtension = null;
    #region Filled in CheckSize()
    public Dictionary<int, long> filesAndSizes = null;
    public Dictionary<TypeOfExtension, long> sizeOfExtensionTypes = null;
    /// <summary>
    /// All extensions is lower and without dot
    /// </summary>
    public Dictionary<string, long> sizeOfExtension = null;
    public long overallSize = 0;
    #endregion
    /// <summary>
    /// Is filled in method CreateFileInfoLiteObjects
    /// </summary>
    public Dictionary<string, List<FileInfoLite>> fileInfoLiteOfExtension = null;


    public SolutionFolderWithFiles(SolutionFolder sf)
    {
        countOfImages = sf.countOfImages;
        displayedText = sf.displayedText;
        fullPathFolder = sf.fullPathFolder;
        nameSolutionWithoutDiacritic = sf.nameSolutionWithoutDiacritic;

        filesAndSizes = new Dictionary<int, long>();
        sizeOfExtensionTypes = new Dictionary<TypeOfExtension, long>();
        sizeOfExtension = new Dictionary<string, long>();

        files = FS.GetFiles(fullPathFolder, AllStringsSE.asterisk, SearchOption.AllDirectories);
        filesOfExtension = new Dictionary<string, List<string>>();

        for (int i = 0; i < files.Count; i++)
        {
            var item = files[i];
            string ext = FS.GetExtension(item).TrimStart(AllCharsSE.dot);
            DictionaryHelper.AddOrCreate(filesOfExtension, ext, item);
        }


    }

    public void CheckSize()
    {
        for (int i = 0; i < files.Count; i++)
        {
            var item = files[i];
            long fs = FS.GetFileSize(item);
            overallSize += fs;
            filesAndSizes.Add(i, fs);

            string ext = FS.GetExtension(item).TrimStart(AllCharsSE.dot);
            TypeOfExtension extType = AllExtensionsHelper.FindTypeWithDot(ext);

            if (!sizeOfExtensionTypes.ContainsKey(extType))
            {
                sizeOfExtensionTypes.Add(extType, fs);
            }
            else
            {
                sizeOfExtensionTypes[extType] += fs;
            }

            if (!sizeOfExtension.ContainsKey(ext))
            {
                sizeOfExtension.Add(ext, fs);
            }
            else
            {
                sizeOfExtension[ext] += fs;
            }
        }

        displayedText += " (" + FS.GetSizeInAutoString(overallSize, ComputerSizeUnits.MB) + AllStringsSE.rb;
    }

    public void CreateFileInfoLiteObjects(string extensionWithoutDot, string item)
    {
        var fil = FileInfoLite.GetFIL(item);
        if (fileInfoLiteOfExtension.ContainsKey(extensionWithoutDot))
        {
            fileInfoLiteOfExtension[extensionWithoutDot].Add(fil);
        }
        else
        {
            List<FileInfoLite> l = new List<FileInfoLite>();
            l.Add(fil);
            fileInfoLiteOfExtension.Add(extensionWithoutDot, l);
        }
    }
}
