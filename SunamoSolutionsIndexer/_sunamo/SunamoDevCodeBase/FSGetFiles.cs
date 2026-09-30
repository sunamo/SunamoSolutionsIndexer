namespace SunamoSolutionsIndexer._sunamo;

/// <summary>
/// Gets files from the file system.
/// </summary>
internal class FSGetFiles
{

#pragma warning disable
    /// <summary>
    /// Returns files of the folder matching the mask.
    /// </summary>
    internal static List<string> GetFiles(ILogger logger, string folder, string mask, SearchOption searchOption, GetFilesArgsDC? getFilesArgs = null)
#pragma warning restore
    {
        if (getFilesArgs != null)
        {
            ThrowEx.Custom("getFilesArgs is not null");
        }

        return Directory.GetFiles(folder, mask, searchOption).ToList();
    }
}