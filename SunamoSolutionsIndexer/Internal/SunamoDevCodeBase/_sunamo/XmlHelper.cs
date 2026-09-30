namespace SunamoDevCode._sunamo;

internal class XmlHelper
{
    internal static IList<XmlNode> GetElementsOfName(XmlNode node, string name)
    {
        return node.ChildNodes.WithName(name);
    }

    internal static XmlNode GetElementOfName(XmlNode node, string name)
    {
        return node.ChildNodes.First(name);
    }

    internal static string InnerTextOfNode(XmlNode node)
    {
        return node.InnerText;
    }

    internal static void SetAttribute(XmlNode node, string attributeName, string attributeValue)
    {
        var element = (XmlElement)node;
        if (element != null)
        {
            element.SetAttribute(attributeName, attributeValue);
            return;
        }
        // Working only when attribute
        var attributeValueExisting = Attr(node, attributeName);
        if (attributeValueExisting == null)
        {
            var newAttribute = node.OwnerDocument!.CreateAttribute(attributeName);
            node.Attributes!.Append(newAttribute);
        }
        node.Attributes![attributeName]!.Value = attributeValue;
    }

    internal static string? Attr(XmlNode node, string attributeName)
    {
        var argument = GetAttributeWithName(node, attributeName);
        if (argument != null)
        {
            return argument.Value;
        }
        return null;
    }

    internal static XmlAttribute? FoundedNode = null;

    internal static XmlNode? GetAttributeWithName(XmlNode node, string attributeName)
    {
        foreach (XmlAttribute attribute in node.Attributes!)
        {
            if (attribute.Name == attributeName)
            {
                FoundedNode = attribute;
                return attribute;
            }
        }
        return null;
    }
}
