namespace SunamoSolutionsIndexer._sunamo;

/// <summary>
/// Helpers for working with enums.
/// </summary>
internal class EnumHelper
{
    /// <summary>
    /// Parses the text to the enum value, returns the default value when parsing fails.
    /// </summary>
    internal static T Parse<T>(string text, T defaultValue, bool isReturningDefaultIfNull = false)
        where T : struct
    {
        if (isReturningDefaultIfNull)
        {
            return defaultValue;
        }
        if (Enum.TryParse<T>(text, true, out var result))
        {
            return result;
        }

        return defaultValue;
    }
}
