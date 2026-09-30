namespace SunamoDevCode._sunamo.SunamoExtensions;

internal static class XmlNodeListExtensions
{
    #region For easy copy from XmlNodeListExtensions.cs
    internal static XmlNode First(this XmlNodeList nodeList, string nodeName)
    {
        foreach (XmlNode item in nodeList)
            if (item.Name == nodeName)
                return item;
        return null!;
    }

    internal static List<XmlNode> WithName(this XmlNodeList nodeList, string nodeName)
    {
        var result = new List<XmlNode>();
        foreach (XmlNode item in nodeList)
            if (item.Name == nodeName)
                result.Add(item);
        return result;
    }
    #endregion
}