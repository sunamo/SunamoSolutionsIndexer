namespace SunamoSolutionsIndexer.Interfaces;

/// <summary>
/// Represents the i solution folder serialize type.
/// </summary>
public interface ISolutionFolderSerialize
{
    /// <summary>
    /// Gets or sets the displayed text.
    /// </summary>
    string DisplayedText { get; set; }
    /// <summary>
    /// Gets or sets the full path folder.
    /// </summary>
    string FullPathFolder { get; set; }
    /// <summary>
    /// Gets the long name.
    /// </summary>
    string LongName { get; }
    /// <summary>
    /// Gets the name solution.
    /// </summary>
    string NameSolution { get; }
    /// <summary>
    /// Gets the run one.
    /// </summary>
    string RunOne { get; }
    /// <summary>
    /// Gets the short name.
    /// </summary>
    string ShortName { get; }

    /// <summary>
    /// To string.
    /// </summary>
    /// <returns>The to string result.</returns>
    string ToString();
}