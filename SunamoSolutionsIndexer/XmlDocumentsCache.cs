namespace SunamoSolutionsIndexer;

public class XmlDocumentsCache
{
    private const string Nullable = "<Nullable>enable</Nullable>";
    private const string DebugTypeNone = "<DebugType>none</DebugType>";
    public static Type Type = typeof(XmlDocumentsCache);
    public static Dictionary<string, XmlDocument> Cache = new();

    // In key is csproj path
    // In value is absolute path of references (recursive)
    public static Dictionary<string, List<string>> ProjectDeps = new();

    public static Func<string, Dictionary<string, XmlDocument>?,
            Task<List<string>>
        >
        BuildProjectsDependencyTreeList = null!;

    public static int Nulled;
    public static IProgressBarDC? Clpb = null;
    public static List<string> CantBeLoadWithDictToAvoidCollectionWasChangedButCanWithNull = new();

    // Nemůže se volat společně s .Result! viz. https://stackoverflow.com/a/65820543/9327173 Způsobí to deadlock! Musím to
    // dělat přes ThisApp.async_
    // Can return null during many situations
    // For example when ignored => must always checking for null
    public static
        async Task<ResultWithExceptionDC<XmlDocument>>
        Get(string path)
    {
        // Tady to mít je píčovina. To se nemůže nikdy s malým vyskytnout
        //path = SH.FirstCharUpper(path);
        if (Cache.ContainsKey(path)) return new ResultWithExceptionDC<XmlDocument>(Cache[path]);
        if (Ignored.IsIgnored(path))
        {
            Cache.Add(path, null!);
            Nulled++;
            return new ResultWithExceptionDC<XmlDocument> { Exc = "csproj is ignored: " + path };
        }

        // Load the XML document
        var doc = new XmlDocument();
        // HACK: XmlStreamReader will fail if the file is encoded in UTF-8 but has <?xml version="1.0" encoding="utf-16"?>
        // To work around this, we load the XML content into a string and use XmlDocument.LoadXml() instead.
        // Zde bylo async. Ale na řádku s .ConfigureAwait(false) se to zaseklo. Proto je tu teď pouze ReadAllText
        //if (cache.ContainsKey(path))
        //{
        //    return cache[path];
        //}
        string? xml = null;
        //if (ThisApp.async_)
        //{
        xml =
            // Tohle nechápu. FubuCsProjFile i SunamoExceptions jsou net7.0.
            // co to je za dementní chybu This call site is reachable on all platforms. 'File.ReadAllTextAsyncAsync(string)' is only supported on: 'Windows' 7.0 and later.
            await
                FileAsync.ReadAllTextAsync(path);
        //}
        //else
        //{
        //    xml = FileAsync.ReadAllTextAsync(path);
        //}
        if (xml.Contains(GitConsts.startingHead))
        {
            Cache.Add(path, null!);
            Nulled++;
            return new ResultWithExceptionDC<XmlDocument>();
        }

        var save = false;
        if (xml.Contains(Nullable))
        {
            xml = xml.Replace(Nullable, string.Empty);
            save = true;
        }

        if (xml.Contains(DebugTypeNone))
        {
            xml = xml.Replace(DebugTypeNone, string.Empty);
            save = true;
        }

        if (save) await FileAsync.WriteAllTextAsync(path, xml);
        xml = FormatXml(xml);
        if (xml.StartsWith("Exception:")) return new ResultWithExceptionDC<XmlDocument>(xml);
        try
        {
            doc.PreserveWhitespace = true;
            doc.LoadXml(xml);
            // Zřejmě mi toto vyhazovalo výjimku
            //var ox = doc.OuterXml;
            //if (Cache.ContainsKey(path))
            //{
            //    return Cache[path];
            //}
            Cache.Add(path, doc);
        }
        catch
        {
            var index = Cache.Keys.ToList().IndexOf(path);
            Cache.Add(path, null!);
            Nulled++;
            //ThrowEx.NotValidXml(path, ex);
            return new ResultWithExceptionDC<XmlDocument>();
        }

        //lock (_lock)
        //{
        // Toto bych měl dělat mimo Parallel
        if (BuildProjectsDependencyTreeList != null)
        {
            var list =
                await
                    BuildProjectsDependencyTreeList(path, null);
            ProjectDeps.Add(path, list);
        }

        //}
        return new ResultWithExceptionDC<XmlDocument>(doc);
    }

    private static string FormatXml(string xml)
    {
        try
        {
            var doc = XDocument.Parse(xml);
            return doc.ToString();
        }
        catch (Exception)
        {
            // Handle and throw if fatal exception here; don't just ignore them
            return xml;
        }
    }

    public static Dictionary<string, XmlDocument> BuildProjectDeps()
    {
        var xd = new Dictionary<string, XmlDocument>();
        // Všechny načtené XML dokumenty do xd
        foreach (var item in Cache)
            // Zde je problémů několik. xd má pouhý 1 element, ačkoliv XmlDocumentsCache.Cache jich má 41
            // ProjectDeps má poté 41.
            if (item.Value != null)
                xd.Add(item.Key, item.Value);
        return xd;
    }

    public static List<string> BadXml()
    {
        var withNull = Cache.Where(entry => entry.Value == null);
        var badXmlPaths = new List<string>();
        foreach (var item in withNull) badXmlPaths.Add(item.Key);
        return badXmlPaths;
    }

    public static
        async Task
        Set(string path, string xmlContent, bool saveToFile = false)
    {
        var xd = new XmlDocument();
        xd.PreserveWhitespace = true;
        xd.LoadXml(xmlContent);
        await
            Set(path, xd, saveToFile);
    }

    public static
        async Task
        Set(string path, XmlDocument document, bool saveToFile = false)
    {
        if (saveToFile)
        {
            document.PreserveWhitespace = true;
            await
                FileAsync.WriteAllTextAsync(path, document.OuterXml);
        }

        DictionaryHelper.AddOrSet(Cache, path, document);
    }
}
