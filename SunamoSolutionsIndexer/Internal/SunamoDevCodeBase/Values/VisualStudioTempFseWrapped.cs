namespace SunamoDevCode.Values;

internal class VisualStudioTempFseWrapped
{
    #region 1) To delete
    public static List<string> FoldersInSolutionToDelete = null!;

    public static List<string> FoldersInProjectToDelete = null!;

    public static List<string> FoldersAnywhereToDelete = null!;
    #endregion

    #region 2) To keep
    public static List<string> FoldersInSolutionToKeep = null!;

    public static List<string> FoldersInProjectToKeep = null!;

    public static List<string> FoldersAnywhereToKeep = null!;
    #endregion

    #region 3) Downloaded
    public static List<string> FoldersInSolutionDownloaded = null!;

    public static List<string> FoldersInProjectDownloaded = null!;

    public static List<string> FoldersAnywhereDownloaded = null!;
    #endregion

    public static List<string> FilesWeb = null!;

    public static List<string> Aggregate = null!;

    public static bool IsToIndexed(string path)
    {
        if (SH.ContainsAtLeastOne(path, Aggregate))
        {
            return false;
        }
        return true;
    }

    static VisualStudioTempFseWrapped()
    {
        #region 1) To delete
        FoldersInSolutionToDelete = CA.WrapWith(VisualStudioTempFse.FoldersInSolutionToDelete, "\"");
        FoldersInProjectToDelete = CA.WrapWith(VisualStudioTempFse.FoldersInProjectToDelete, "\"");
        FoldersAnywhereToDelete = CA.WrapWith(VisualStudioTempFse.FoldersAnywhereToDelete, "\"");
        #endregion

        #region 2) To keep
        FoldersInSolutionToKeep = CA.WrapWith(VisualStudioTempFse.FoldersInSolutionToKeep, "\"");
        FoldersInProjectToKeep = CA.WrapWith(VisualStudioTempFse.FoldersInProjectToKeep, "\"");
        FoldersAnywhereToKeep = CA.WrapWith(VisualStudioTempFse.FoldersAnywhereToKeep, "\"");
        #endregion

        #region 3) Downloaded
        FoldersInSolutionDownloaded = CA.WrapWith(VisualStudioTempFse.FoldersInSolutionDownloaded, "\"");
        FoldersInProjectDownloaded = CA.WrapWith(VisualStudioTempFse.FoldersInProjectDownloaded, "\"");
        FoldersAnywhereDownloaded = CA.WrapWith(VisualStudioTempFse.FoldersAnywhereDownloaded, "\"");
        #endregion

        FilesWeb = CA.WrapWith(VisualStudioTempFse.FilesWeb, "\"");
        Aggregate = CA.JoinIList(FoldersInSolutionToDelete,
FoldersInProjectToDelete,
FoldersInSolutionDownloaded,
FoldersAnywhereToDelete,
FoldersInSolutionToKeep,
FoldersInProjectToKeep,
FoldersInProjectDownloaded, FilesWeb);
    }
}
