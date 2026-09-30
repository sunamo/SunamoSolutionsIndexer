namespace SunamoDevCode._sunamo.SunamoConverters.Converts;

internal static class ConvertTypeShortcutFullName
{
    private const string SystemDot = "System.";

    internal static string ToShortcut(string fullName)
        => ToShortcut(fullName, true);

    internal static string ToShortcut(string fullName, bool throwExceptionWhenNotBasicType)
    {
        if (!fullName.StartsWith(SystemDot))
        {
            fullName = SystemDot + fullName;
        }
        switch (fullName)
        {
            #region Basic Type Mappings
            case "System.String":
                return "string";
            case "System.Int32":
                return "int";
            case "System.Boolean":
                return "bool";
            case "System.Single":
                return "float";
            case "System.DateTime":
                return "DateTime";
            case "System.Double":
                return "double";
            case "System.Decimal":
                return "decimal";
            case "System.Char":
                return "char";
            case "System.Byte":
                return "byte";
            case "System.SByte":
                return "sbyte";
            case "System.Int16":
                return "short";
            case "System.Int64":
                return "long";
            case "System.UInt16":
                return "ushort";
            case "System.UInt32":
                return "uint";
            case "System.UInt64":
                return "ulong";
                #endregion
        }
        if (throwExceptionWhenNotBasicType)
        {
            throw new Exception("Unsupported type");
        }
        return fullName;
    }
}
