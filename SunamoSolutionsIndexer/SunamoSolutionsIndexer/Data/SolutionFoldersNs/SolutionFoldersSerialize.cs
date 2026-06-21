namespace SunamoSolutionsIndexer.Data.SolutionFoldersNs;

/// <summary>
/// Serializable collection of solution folders
/// </summary>
public class SolutionFoldersSerialize
{
    /// <summary>
    /// List of serializable solution folders.
    /// </summary>
    public List<SolutionFolderSerialize> SolutionFolders = new List<SolutionFolderSerialize>();

    /// <summary>
    /// Inserts a solution folder at specified index, maintaining max 10 items
    /// </summary>
    /// <param name="index">Index to insert at</param>
    /// <param name="solutionFolder">Solution folder to insert</param>
    public void Insert(int index, SolutionFolderSerialize solutionFolder)
    {
        if (solutionFolder == null)
        {
            return;
        }

        for (int i = 0; i < SolutionFolders.Count; i++)
        {
            if (SolutionFolders[i].FullPathFolder == solutionFolder.FullPathFolder)
            {
                SolutionFolders.RemoveAt(i);
                break;
            }
        }
        SolutionFolders.Insert(index, solutionFolder);
        if (SolutionFolders.Count > 10)
        {
            for (int i = 10; i < SolutionFolders.Count; i++)
            {
                SolutionFolders.RemoveAt(10);
            }
        }
        Update();
    }

    /// <summary>
    /// Event raised when the solution folders collection is updated.
    /// </summary>
    public event Action<List<SolutionFolderSerialize>>? Updated;

    /// <summary>
    /// Removes all solution folders with specified displayed text
    /// </summary>
    /// <param name="displayedText">Displayed text to match</param>
    public void RemoveWithDisplayedText(string displayedText)
    {
        SolutionFolders.RemoveAll(folder => folder.DisplayedText == displayedText);
    }

    /// <summary>
    /// Raises the Updated event with the current solution folders.
    /// </summary>
    public void Update()
    {
        Updated?.Invoke(SolutionFolders);
    }

    /// <summary>
    /// Gets solution folders by name with optional error handling
    /// If isMissingAllowed is false and solution can't be found, saves exception in result
    /// Otherwise saves null in result Data
    /// </summary>
    /// <param name="solutionNamesToFind">Solution names to find</param>
    /// <param name="isMissingAllowed">If false, throws exception when solution not found</param>
    /// <returns>Result with found solution folders or exception</returns>
    public ResultWithExceptionDC<SolutionFoldersSerialize> GetWithName(List<string> solutionNamesToFind, bool isMissingAllowed)
    {
        ResultWithExceptionDC<SolutionFoldersSerialize> result = new ResultWithExceptionDC<SolutionFoldersSerialize>();
        result.Data = new SolutionFoldersSerialize();

        foreach (var solutionName in solutionNamesToFind)
        {
            SolutionFolderSerialize? solutionFolder = SolutionFolders.Find(folder =>
            {
                if (folder.NameSolution == solutionName)
                {
                    return true;
                }
                return false;
            });

            if (solutionFolder == null)
            {
                if (!isMissingAllowed)
                {
                    result.Exc = Exceptions.ElementCantBeFound("", "solutionNamesToFind", solutionName)!;
                }
            }
            else
            {
                result.Data.SolutionFolders.Add(solutionFolder);
            }
        }

        return result;
    }

    /// <summary>
    /// Removes solution folders with specified names
    /// </summary>
    /// <param name="solutionNamesToRemove">Solution names to remove</param>
    public void RemoveWithName(List<string> solutionNamesToRemove)
    {
        int index = -1;
        foreach (var solutionName in solutionNamesToRemove)
        {
            if ((index = SolutionFolders.FindIndex(folder => folder.NameSolution == solutionName)) != -1)
            {
                SolutionFolders.RemoveAt(index);
            }
        }
    }
}