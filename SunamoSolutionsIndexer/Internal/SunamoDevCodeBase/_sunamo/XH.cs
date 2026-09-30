namespace SunamoDevCode._sunamo;

internal class XH
{
    internal static XmlNode ReturnXmlNode(string xml)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.PreserveWhitespace = true;
        xmlDocument.LoadXml(xml);
        return xmlDocument.FirstChild!;
    }
}
