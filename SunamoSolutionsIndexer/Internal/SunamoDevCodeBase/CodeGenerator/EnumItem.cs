namespace SunamoDevCode.CodeGenerator;

internal class EnumItem
{
    public string Hex { get; set; } = "";

    public Dictionary<string, string>? Attributes { get; set; } = null;

    public string Name { get; set; } = "";

    public string Comment { get; set; } = string.Empty;
}
