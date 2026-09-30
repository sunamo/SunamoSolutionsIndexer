namespace SunamoDevCode._sunamo.SunamoEmbeddedResources;

// Helper for embedded resources
// Requires assembly and default namespace.
// Content is referred like with ResourcesH - with fs path
internal class EmbeddedResourcesH
{
    internal static EmbeddedResourcesH? Instance = null;

    protected EmbeddedResourcesH()
    {

    }

    // Constructor for use in assembly like SunamoNTextCat
    // Parameter is name of project, therefore don't insert typeResourcesSunamo.Namespace
    internal EmbeddedResourcesH(Assembly entryAssembly, string defaultNamespace)
    {
        this._entryAssembly = entryAssembly;
        _defaultNamespace = defaultNamespace;
    }

    protected Assembly? _entryAssembly = null;
    protected string _defaultNamespace = null!;

    protected Assembly entryAssembly
    {
        get
        {
            if (_entryAssembly == null)
            {
                _entryAssembly = Assembly.GetEntryAssembly()!;
            }
            return _entryAssembly;
        }
    }
}
