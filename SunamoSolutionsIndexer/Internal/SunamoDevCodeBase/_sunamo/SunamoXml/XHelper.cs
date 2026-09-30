namespace SunamoDevCode._sunamo.SunamoXml;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal partial class XHelper
{
    internal static Dictionary<string, string> ns = new Dictionary<string, string>();
    internal static IList<XElement> GetElementsOfNameWithAttrContains(XElement group, string tag, string attr, string value /*, bool caseSensitive = false*/)
    {
        return GetElementsOfNameWithAttrWorker(group, tag, attr, value /*, true, caseSensitive*/);
    }

    internal static List<XElement> GetElementsOfNameWithAttrWorker(System.Xml.Linq.XElement xElement, string tag, string attr, string value /*, bool enoughIsContainsAttribute, bool caseSensitive*/)
    {
        var vr = new List<XElement>();
        var e = XHelper.GetElementsOfNameRecursive(xElement, tag);
        foreach (XElement item in e)
        {
            var attrValue = XHelper.Attr(item, attr);
            if (attrValue!.Contains(value) /*SH.ContainsBoolBool(attrValue, value, enoughIsContainsAttribute, caseSensitive)*/)
            {
                vr.Add(item);
            }
        }

        return vr;
    }

    internal static List<XElement> GetElementsOfNameRecursive(XElement node, string nazev)
    {
        var vr = new List<XElement>();
        if (nazev.Contains(":"))
        {
            var(p, z) = SH.GetPartsByLocationNoOut(nazev, ':');
            p = XHelper.ns[p];
            foreach (XElement item in node.DescendantsAndSelf())
            {
                if (item.Name.LocalName == z && item.Name.NamespaceName == p)
                {
                    vr.Add(item);
                }
            }
        }
        else
        {
            foreach (XElement item in node.DescendantsAndSelf())
            {
                if (item.Name.LocalName == nazev)
                {
                    vr.Add(item);
                }
            }
        }

        return vr;
    }

    internal static void AddXmlNamespaces(XmlNamespaceManager nsmgr)
    {
        foreach (string item in nsmgr)
        {
            // Jaky je typ item, at nemusim pouzivat slovnik
            var value = nsmgr.LookupNamespace(item);
            if (!ns.ContainsKey(item))
            {
                ns.Add(item, value!);
            }
        }
    }

    internal static XElement? GetElementOfNameWithAttr(XElement node, string nazev, string attr, string value)
    {
        if (nazev.Contains(":"))
        {
            var(p, z) = SH.GetPartsByLocationNoOut(nazev, ':');
            p = XHelper.ns[p];
            foreach (XElement item in node.Elements())
            {
                if (item.Name.LocalName == z && item.Name.NamespaceName == p)
                {
                    if (Attr(item, attr) == value)
                    {
                        return item;
                    }
                }
            }
        }
        else
        {
            foreach (XElement item in node.DescendantsAndSelf())
            {
                if (item.Name.LocalName == nazev)
                {
                    if (Attr(item, attr) == value)
                    {
                        return item;
                    }
                }
            }
        }

        return null;
    }

    internal static string? Attr(XElement item, string attr)
    {
        var xa = item.Attribute(XName.Get(attr));
        if (xa != null)
        {
            return xa.Value;
        }

        return null;
    }

    internal static XElement MakeAllElementsWithDefaultNs(XElement settings)
    {
        var ns2 = XHelper.ns[string.Empty];
        var toInsert = new List<object>();
        // shift ALL elements in the settings document into the target namespace
        foreach (XElement e in settings.DescendantsAndSelf())
        {
            //e.Name =  e.Name.LocalName;
            e.Name = XName.Get(e.Name.LocalName, ns2);
        }

        //foreach (var e in settings.Attributes())
        //{
        //    //e.Name = XName.Get(e.Name.LocalName, ns2);
        //    toInsert.Add(e);
        //}
        //t
        var vr = new XElement(XName.Get(settings.Name.LocalName, ns2), settings.Attributes(), settings.Descendants());
        return vr;
    }

    internal static bool IsRightTag(XElement xName, string nazev)
    {
        return IsRightTag(xName.Name, nazev);
    }

    internal static bool IsRightTag(XName xName, string nazev)
    {
        var(p, z) = SH.GetPartsByLocationNoOut(nazev, ':');
        p = XHelper.ns[p];
        if (xName.LocalName == z && xName.NamespaceName == p)
        {
            return true;
        }

        return false;
    }

    internal static List<XElement> GetElementsOfName(XElement node, string nazev)
    {
        var result = new List<XElement>();
        if (nazev.Contains(":"))
        {
            foreach (XElement item in node.Elements())
            {
                if (IsRightTag(item, nazev))
                {
                    result.Add(item);
                }
            }
        }
        else
        {
            foreach (XElement item in node.Elements())
            {
                if (item.Name.LocalName == nazev)
                {
                    result.Add(item);
                }
            }
        }

        return result;
    }

    internal static bool IsRightTag(XElement xName, string localName, string namespaceName)
    {
        return IsRightTag(xName.Name, localName, namespaceName);
    }

    internal static bool IsRightTag(XName xName, string localName, string namespaceName)
    {
        if (xName.LocalName == localName && xName.NamespaceName == namespaceName)
        {
            return true;
        }

        return false;
    }

    internal static XElement? GetElementOfName(XContainer node, string nazev)
    {
        if (nazev.Contains(":"))
        {
            var(p, z) = SH.GetPartsByLocationNoOut(nazev, ':');
            p = XHelper.ns[p];
            foreach (XElement item in node.Elements())
            {
                if (IsRightTag(item, z, p))
                {
                }

                if (item.Name.LocalName == z && item.Name.NamespaceName == p)
                {
                    return item;
                }
            }
        }
        else
        {
            foreach (XElement item in node.Elements())
            {
                if (item.Name.LocalName == nazev)
                {
                    return item;
                }
            }
        }

        return null;
    }

    internal static 
    async Task<XDocument>
    CreateXDocument(string contentOrFn)
    {
        if (File.Exists(contentOrFn))
        {
            contentOrFn = 
            await
            FileAsync.ReadAllTextAsync(contentOrFn);
        }

        var enB = Encoding.UTF8.GetBytes(contentOrFn).ToList();
        XDocument? xd = null;
        using (MemoryStream oStream = new MemoryStream(enB.ToArray()))
        using (XmlReader oReader = XmlReader.Create(oStream))
        {
            xd = XDocument.Load(oReader);
        }

        return xd;
    }
}
