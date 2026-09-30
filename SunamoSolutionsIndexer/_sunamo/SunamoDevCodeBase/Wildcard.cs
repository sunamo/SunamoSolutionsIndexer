namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

// Represents a wildcard running on the System.Text.RegularExpressions engine.
/// <summary>
/// Regex created from a wildcard pattern.
/// </summary>
internal class Wildcard : Regex
{

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    internal Wildcard(string pattern)
    : base(WildcardToRegex(pattern))
    {
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    internal Wildcard(string pattern, RegexOptions options)
    : base(WildcardToRegex(pattern), options)
    {
    }

    /// <summary>
    /// Converts the wildcard pattern to a regular expression.
    /// </summary>
    internal static string WildcardToRegex(string pattern) =>
        "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
}