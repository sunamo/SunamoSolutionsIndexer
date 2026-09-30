namespace SunamoDevCode._sunamo.SunamoCollectionOnDrive;

internal abstract class CollectionOnDriveBase<T>(ILogger logger) : List<T>
{
    protected bool removeDuplicates = false;
    protected CollectionOnDriveArgs args = new();
    private bool isSaving;
    private FileSystemWatcher? watcher;
    internal async Task RemoveAll()
    {
        await ClearWithSave();
        await FileAsync.WriteAllTextAsync(args.path, string.Empty);
    }
    internal async Task RemoveWithSave(T element)
    {
        Remove(element);
        await Save();
    }
    internal async Task ClearWithSave()
    {
        Clear();
        await Save();
    }
    internal abstract Task Load(bool removeDuplicates);
    internal virtual void AddWithoutSave(T element)
    {
        if (logger == NullLogger.Instance)
        {
            ThrowEx.UseNonDummyCollection();
        }
        if (removeDuplicates)
        {
            if (!Contains(element))
            {
                Add(element);
            }
        }
        else
        {
            Add(element);
        }
    }
    internal virtual async Task<bool> AddWithSave(T? element)
    {
        if (logger == NullLogger.Instance)
        {
            ThrowEx.UseNonDummyCollection();
        }
        if (element is null)
        {
            throw new Exception($"{nameof(element)} is null");
        }
        var wasChanged = false;
        if (removeDuplicates)
        {
            if (!Contains(element))
            {
                var stringValue = element.ToString() ?? throw new Exception($"ToString of type ${element} cannot return null");
                if (stringValue.Trim() != string.Empty)
                {
                    Add(element);
                    wasChanged = true;
                }
            }
        }
        else
        {
            Add(element);
            wasChanged = true;
        }
        if (wasChanged)
        {
            await Save();
        }
        return wasChanged;
    }
    internal async Task Save()
    {
        isSaving = true;
        await FileAsync.WriteAllTextAsync(args.path, SHJoin.JoinNL(this));
        isSaving = false;
    }
    public override string ToString()
    {
        return SHJoin.JoinNL(this);
    }
    #region ctor
    internal void Init(CollectionOnDriveArgs arguments)
    {
        this.args = arguments;
        if (arguments.loadChangesFromDrive)
        {
            var parentDirectory = Path.GetDirectoryName(arguments.path);
            if (parentDirectory is null)
            {
                logger.LogWarning("FileSystemWatcher cannot be registered because null value");
                return;
            }
            else
            {
                watcher = new FileSystemWatcher
                {
                    Path = arguments.path
                };
                watcher.Changed += W_Changed;
            }
        }
    }
    private void W_Changed(object sender, FileSystemEventArgs e)
    {
        if (!isSaving)
            Load(removeDuplicates);
    }
    #endregion
}
