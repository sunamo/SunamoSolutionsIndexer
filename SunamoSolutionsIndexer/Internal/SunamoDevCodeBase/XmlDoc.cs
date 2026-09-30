namespace SunamoDevCode;

internal class XmlDoc
{
    private readonly InstantSB _stringBuilder;

    public XmlDoc(InstantSB stringBuilder)
    {
        _stringBuilder = stringBuilder;
    }

    public void SummaryStart()
    {
        Prefix("<summary>");
    }

    private void Prefix(string text)
    {
        text = text.Replace("\r", "");
        var lines = text.Split('\n');

        foreach (var line in lines) _stringBuilder.AppendLine("/// " + line);
    }

    public void SummaryEnd(bool appendLine = true)
    {
        Prefix("</summary>");
        if (appendLine) _stringBuilder.AppendLine();
    }

    public void Raw(string text)
    {
        Prefix(text);
    }
}
