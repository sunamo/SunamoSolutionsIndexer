namespace SunamoDevCode._sunamo.SunamoEnumsHelper;

internal class EnumHelper
{
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
