namespace SunamoSolutionsIndexer._sunamo;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
/// <summary>
/// Builder of git bash commands.
/// </summary>
public partial class GitBashBuilder : IGitBashBuilder
{

    public static string SomeErrorsOccured { get; set; } = "SomeErrorsOccured";

#pragma warning restore
    /// <summary>
    /// Appends change of the current directory.
    /// </summary>
    public void Cd(string key)
    {
        StringBuilder.AppendLine("cd " + SH.WrapWith(key, "\""));
    }

    /// <summary>
    /// Clears the content.
    /// </summary>
    public void Clear()
    {
        StringBuilder.Clear();
    }

    /// <summary>
    /// Appends the text.
    /// </summary>
    public void Append(string text)
    {
        StringBuilder.Append(text + " ");
    }

    /// <summary>
    /// Appends the text and a new line.
    /// </summary>
    public void AppendLine(string text)
    {
        StringBuilder.AppendLine(text);
    }

    /// <summary>
    /// Appends the text and a new line.
    /// </summary>
    public void AppendLine()
    {
        StringBuilder.AppendLine();
    }

    /// <summary>
    /// Returns the generated text.
    /// </summary>
    public override string ToString()
    {
        return StringBuilder.ToString();
    }
}
