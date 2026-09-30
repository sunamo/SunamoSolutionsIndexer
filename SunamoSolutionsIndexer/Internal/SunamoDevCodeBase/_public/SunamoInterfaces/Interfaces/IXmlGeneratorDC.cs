namespace SunamoDevCode._public.SunamoInterfaces.Interfaces;

internal interface IXmlGeneratorDC
{
    void AppendLine();
    void EndComment();
    void Insert(int index, string text);
    int Length();
    void StartComment();
    void TerminateTag(string tagName);
    string ToString();
    void WriteCData(string innerCData);
    void WriteElement(string elementName, string inner);
    void WriteNonPairTag(string tagName);
    void WriteNonPairTagWith2Attrs(string tagName, string attr1Name, string attr1Value, string attr2Name, string attr2Value);
    void WriteNonPairTagWithAttr(string tagName, string attrName, string attrValue);
    void WriteNonPairTagWithAttrs(bool appendNull, string tagName, params string[] attributes);
    void WriteNonPairTagWithAttrs(string tag, List<string> args);
    void WriteNonPairTagWithAttrs(string tag, params string[] args);
    void WriteRaw(string rawContent);
    void WriteTag(string tagName);
    void WriteTagNamespaceManager(object rss, XmlNamespaceManager nsmgr, string namespaceUri, string prefix);
    void WriteTagNamespaceManager(string tagName, XmlNamespaceManager nsmgr, params string[] args);
    void WriteTagWith2Attrs(string tagName, string attr1Name, string attr1Value, string attr2Name, string attr2Value);
    void WriteTagWithAttr(string tag, string attributeName, string attributeValue, bool skipEmptyOrNull = false);
    void WriteTagWithAttrs(string tagName, List<string> attributes);
    void WriteTagWithAttrs(string tagName, params string[] attributes);
    void WriteTagWithAttrsCheckNull(string tagName, params string[] attributes);
    void WriteXmlDeclaration();
}
