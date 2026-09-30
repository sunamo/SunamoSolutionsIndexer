namespace SunamoDevCode._sunamo.SunamoString;

internal class SHNotTranslateAble
{
    internal static string DecodeSlashEncodedString(string value)
    {
        // was added ; after 1,2 line and  after 2,3
        // keep as was writte
        value = SHReplace.ReplaceAll(value, "\\", "\\\\");
        value = SHReplace.ReplaceAll(value, "\"", "\\\"");
        value = SHReplace.ReplaceAll(value, "\'", "\\\'");
        return value;
    }
}