namespace SunamoSolutionsIndexer;

/// <summary>
/// Represents the xml documents cache type.
/// </summary>
public class XmlDocumentsCache
{
    private const string Nullable = "<Nullable>enable</Nullable>";
    private const string DebugTypeNone = "<DebugType>none</DebugType>";
    /// <summary>
    /// The type.
    /// </summary>
    public static Type Type = typeof(XmlDocumentsCache);
    /// <summary>
    /// The cache.
    /// </summary>
    public static Dictionary<string, XmlDocument> Cache = new();

    // In key is csproj path
    // In value is absolute path of references (recursive)
    /// <summary>
    /// The project deps.
    /// </summary>
    public static Dictionary<string, List<string>> ProjectDeps = new();

    /// <summary>
    /// Delegate that builds the project dependency tree list.
    /// </summary>
    public static Func<string, Dictionary<string, XmlDocument>?,
            Task<List<string>>
        >
        BuildProjectsDependencyTreeList = null!;

    /// <summary>
    /// The nulled.
    /// </summary>
    public static int Nulled;
    /// <summary>
    /// The clpb.
    /// </summary>
    public static IProgressBarDC? Clpb = null;
    /// <summary>
    /// The cant be load with dict to avoid collection was changed but can with null.
    /// </summary>
    public static List<string> CantBeLoadWithDictToAvoidCollectionWasChangedButCanWithNull = new();

    // Cannot be called together with .Result! See https://stackoverflow.com/a/65820543/9327173 It causes a deadlock! I must
    // do via ThisApp.async_
    // Can return null during many situations
    // For example when ignored => must always checking for null
    /// <summary>
    /// Get.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>The get result.</returns>
    public static
        async Task<ResultWithExceptionDC<XmlDocument>>
        Get(string path)
    {
        // Having this here is pointless. It can never occur with lowercase
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
        // This used to be async. But it got stuck on the line with .ConfigureAwait(false). That is why there is only ReadAllText now
        string? xml = null;
        xml =
            // I do not get this. FubuCsProjFile and SunamoExceptions are both net7.0.
            // what a stupid error is this: This call site is reachable on all platforms. 'File.ReadAllTextAsyncAsync(string)' is only supported on: 'Windows' 7.0 and later.
            await
                FileAsync.ReadAllTextAsync(path);
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
            // This apparently threw an exception for me
            Cache.Add(path, doc);
        }
        catch
        {
            var index = Cache.Keys.ToList().IndexOf(path);
            Cache.Add(path, null!);
            Nulled++;
            return new ResultWithExceptionDC<XmlDocument>();
        }

        // I should do this outside Parallel
        if (BuildProjectsDependencyTreeList != null)
        {
            var list =
                await
                    BuildProjectsDependencyTreeList(path, null);
            ProjectDeps.Add(path, list);
        }

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
    /// Initializes a new instance of the build project deps class.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="null">The null.</param>
    public static Dictionary<string, XmlDocument> BuildProjectDeps()
    {
        var xd = new Dictionary<string, XmlDocument>();
        // All loaded XML documents into xd
        foreach (var item in Cache)
            // There are several problems here. xd has only 1 element, although XmlDocumentsCache.Cache has 41
            // ProjectDeps then has 41.
            if (item.Value != null)
                xd.Add(item.Key, item.Value);
        return xd;
    }

    /// <summary>
    /// Bad xml.
    /// </summary>
    /// <returns>The bad xml result.</returns>
    public static List<string> BadXml()
    {
        var withNull = Cache.Where(entry => entry.Value == null);
        var badXmlPaths = new List<string>();
        foreach (var item in withNull) badXmlPaths.Add(item.Key);
        return badXmlPaths;
    }

    /// <summary>
    /// Set.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="xmlContent">The xml content.</param>
    /// <param name="saveToFile">The save to file.</param>
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

    /// <summary>
    /// Set.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="document">The document.</param>
    /// <param name="saveToFile">The save to file.</param>
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
