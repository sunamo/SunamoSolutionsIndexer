namespace SunamoSolutionsIndexer.Data.SolutionFoldersNs;

/// <summary>
/// Represents the solution folders serialize type.
/// </summary>
public class SolutionFoldersSerialize
{
    // List of serializable solution folders.
    /// <summary>
    /// The solution folders.
    /// </summary>
    public List<SolutionFolderSerialize> SolutionFolders = new List<SolutionFolderSerialize>();

    // Inserts a solution folder at specified index, maintaining max 10 items
    /// <summary>
    /// Insert.
    /// </summary>
    /// <typeparam name="SolutionFolderSerialize">The type SolutionFolderSerialize.</typeparam>
    /// <returns>The insert result.</returns>
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
    /// The updated.
    /// </summary>
    public event Action<List<SolutionFolderSerialize>>? Updated;

    /// <summary>
    /// Remove with displayed text.
    /// </summary>
    /// <param name="displayedText">The displayed text.</param>
    /// <returns>The remove with displayed text result.</returns>
    public void RemoveWithDisplayedText(string displayedText)
    {
        SolutionFolders.RemoveAll(folder => folder.DisplayedText == displayedText);
    }

    /// <summary>
    /// Update.
    /// </summary>
    /// <param name="displayedText">The displayed text.</param>
    public void Update()
    {
        Updated?.Invoke(SolutionFolders);
    }

    // Gets solution folders by name with optional error handling
    // If isMissingAllowed is false and solution can't be found, saves exception in result
    // Otherwise saves null in result Data
    /// <summary>
    /// Initializes a new instance of the get with name class.
    /// </summary>
    /// <param name="SolutionFolders">The solution folders.</param>
    public ResultWithExceptionDC<SolutionFoldersSerialize> GetWithName(List<string> solutionNamesToFind, bool isMissingAllowed)
    {
        var result = new ResultWithExceptionDC<SolutionFoldersSerialize>();
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
    /// Remove with name.
    /// </summary>
    /// <param name="solutionNamesToRemove">The solution names to remove.</param>
    /// <returns>The remove with name result.</returns>
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