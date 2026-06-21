namespace SunamoSolutionsIndexer.Interfaces;

/// <summary>
/// Interface for serializable solution folder data
/// </summary>
public interface ISolutionFolderSerialize
{
    /// <summary>
    /// Gets or sets the text displayed in UI for this solution folder.
    /// </summary>
    string DisplayedText { get; set; }
    /// <summary>
    /// Gets or sets the full path to the solution folder on disk.
    /// </summary>
    string FullPathFolder { get; set; }
    /// <summary>
    /// Gets the long name of the solution (typically includes parent folder).
    /// </summary>
    string LongName { get; }
    /// <summary>
    /// Gets the solution name (folder name).
    /// </summary>
    string NameSolution { get; }
    /// <summary>
    /// Gets the command to run/launch this solution.
    /// </summary>
    string RunOne { get; }
    /// <summary>
    /// Gets the short name of the solution.
    /// </summary>
    string ShortName { get; }

    /// <summary>
    /// Returns a string representation of the solution folder.
    /// </summary>
    /// <returns>String representation.</returns>
    string ToString();
}