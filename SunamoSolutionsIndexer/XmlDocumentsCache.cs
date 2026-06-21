namespace SunamoDevCode;

/// <summary>
/// Cache for XML documents (typically csproj files) to avoid repeated parsing.
/// </summary>
public class XmlDocumentsCache
{
    private const string Nullable = "<Nullable>enable</Nullable>";
    private const string DebugTypeNone = "<DebugType>none</DebugType>";
    /// <summary>
    /// Type reference for this class.
    /// </summary>
    public static Type Type = typeof(XmlDocumentsCache);
    /// <summary>
    /// Dictionary caching parsed XmlDocuments keyed by file path.
    /// </summary>
    public static Dictionary<string, XmlDocument> Cache = new();

    /// <summary>
    ///     In key is csproj path
    ///     In value is absolute path of references (recursive)
    /// </summary>
    public static Dictionary<string, List<string>> ProjectDeps = new();

    /// <summary>
    /// Delegate for building the project dependency tree from a csproj path and cache.
    /// </summary>
    public static Func<string, Dictionary<string, XmlDocument>?,
#if ASYNC
            Task<List<string>>
#else
List<string>
#endif
        >
        BuildProjectsDependencyTreeList = null!;

    /// <summary>
    /// Count of csproj files that were null (ignored or failed to load).
    /// </summary>
    public static int Nulled;
    /// <summary>
    /// Optional progress bar for displaying cache loading progress.
    /// </summary>
    public static IProgressBarDC? Clpb = null;
    /// <summary>
    /// List of paths that cannot be loaded with a dictionary to avoid collection-was-changed exceptions.
    /// </summary>
    public static List<string> CantBeLoadWithDictToAvoidCollectionWasChangedButCanWithNull = new();

    /// <summary>
    ///     Nemůže se volat společně s .Result! viz. https://stackoverflow.com/a/65820543/9327173 Způsobí to deadlock! Musím to
    ///     dělat přes ThisApp.async_
    ///     Can return null during many situations
    ///     For example when ignored => must always checking for null
    /// </summary>
    /// <param name="path">Path to the csproj file to load or retrieve from cache.</param>
    /// <returns>Result containing the parsed XmlDocument or an exception message.</returns>

    public static
#if ASYNC
        async Task<ResultWithExceptionDC<XmlDocument>>
#else
ResultWithException<XmlDocument>
#endif
        Get(string path)
    {
#if DEBUG
        if (path.EndsWith("duom.web.csproj"))
        {
        }
#endif
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
#if ASYNC
            // Tohle nechápu. FubuCsProjFile i SunamoExceptions jsou net7.0.
            // co to je za dementní chybu This call site is reachable on all platforms. 'File.ReadAllTextAsyncAsync(string)' is only supported on: 'Windows' 7.0 and later.
            await
#endif
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
#if ASYNC
                await
#endif
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

    /// <summary>
    /// Builds a filtered dictionary containing only successfully loaded XML documents from the cache.
    /// </summary>
    /// <returns>Dictionary of valid XML documents keyed by csproj path.</returns>
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

    /// <summary>
    /// Returns paths of projects with invalid XML.
    /// </summary>
    /// <returns>List of project paths with null XmlDocument values.</returns>
    public static List<string> BadXml()
    {
        var withNull = Cache.Where(entry => entry.Value == null);
        var badXmlPaths = new List<string>();
        foreach (var item in withNull) badXmlPaths.Add(item.Key);
        return badXmlPaths;
    }

    /// <summary>
    /// Sets or updates a cached XML document from raw XML content, optionally saving to disk.
    /// </summary>
    /// <param name="path">Path to the csproj file.</param>
    /// <param name="xmlContent">Raw XML content to parse.</param>
    /// <param name="saveToFile">Whether to also write the document to disk.</param>
    public static
#if ASYNC
        async Task
#else
void
#endif
        Set(string path, string xmlContent, bool saveToFile = false)
    {
        var xd = new XmlDocument();
        xd.PreserveWhitespace = true;
        xd.LoadXml(xmlContent);
#if ASYNC
        await
#endif
            Set(path, xd, saveToFile);
    }

    /// <summary>
    /// Sets or updates a cached XML document, optionally saving to disk.
    /// </summary>
    /// <param name="path">Path to the csproj file.</param>
    /// <param name="document">XmlDocument to cache.</param>
    /// <param name="saveToFile">Whether to also write the document to disk.</param>
    public static
#if ASYNC
        async Task
#else
void
#endif
        Set(string path, XmlDocument document, bool saveToFile = false)
    {
        if (saveToFile)
        {
            document.PreserveWhitespace = true;
#if ASYNC
            await
#endif
                FileAsync.WriteAllTextAsync(path, document.OuterXml);
        }

        DictionaryHelper.AddOrSet(Cache, path, document);
    }
}