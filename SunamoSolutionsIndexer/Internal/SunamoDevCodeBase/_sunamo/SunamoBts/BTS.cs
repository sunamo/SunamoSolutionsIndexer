namespace SunamoDevCode._sunamo.SunamoBts;

internal class BTS
{
    internal static string Replace(ref string text, bool isReplacingCommaForDot)
    {
        if (isReplacingCommaForDot) text = text.Replace(",", ".");

        return text;
    }

    internal static int LastInt = -1;
    internal static long LastLong = -1;
    internal static float LastFloat = -1;
    internal static double LastDouble = -1;

    internal static bool IsFloat(string text, bool isReplacing = false)
    {
        if (text == null) return false;

        Replace(ref text, isReplacing);
        return float.TryParse(text.Replace(",", "."), out LastFloat);
    }

    internal static bool IsInt(string text, bool isThrowingExceptionIfIsFloat = false, bool isReplacingCommaForDot = false)
    {
        if (text == null) return false;

        text = text.Replace(" ", "");
        Replace(ref text, isReplacingCommaForDot);

        var result = int.TryParse(text, out LastInt);
        if (!result)
            if (IsFloat(text))
                if (isThrowingExceptionIfIsFloat)
                    throw new Exception(text + " is float but is calling IsInt");

        return result;
    }

    internal static bool Is(bool first, bool isNegated)
    {
        if (isNegated)
        {
            return !first;
        }
        return first;
    }

    private const string Yes = "Yes";
    private const string No = "No";

    internal static string BoolToString(bool value, bool isLowerCase = false)
    {
        string result = value ? Yes : No;

        if (isLowerCase)
        {
            return result.ToLower();
        }
        return result;
    }

    internal static bool GetValueOfNullable(bool? value)
    {
        return value.GetValueOrDefault();
    }
}
