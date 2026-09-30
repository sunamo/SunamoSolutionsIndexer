namespace SunamoSolutionsIndexer.Internal;

public abstract class PpkOnDriveDevCodeBase<T> : List<T>
{
    #region DPP

    protected PpkOnDriveDevCodeArgs args;

    #endregion

    private bool isSaving;

    // Must use FileSystemWatcher, not FileSystemWatcher because its in sunamo, not desktop
    private readonly FileSystemWatcher w = null!;

    public
        async Task
        RemoveAll()
    {
        await Clear();
        await
            FileAsync.WriteAllTextAsync(args.File, string.Empty);
    }

    public new async Task Remove(T value)
    {
        base.Remove(value);
        await Save();
    }

    public new async Task Clear()
    {
        base.Clear();
        await Save();
    }

    public abstract
        Task
        Load();

    public void AddWithoutSave(T value)
    {
        if (!Contains(value)) base.Add(value);
    }

    public async Task Add(IList<T> items)
    {
        foreach (var item in items) await Add(item);
    }

    public new async Task<bool> Add(T value)
    {
        var wasAdded = false;
        if (!Contains(value))
        {
            if (value!.ToString()!.Trim() != string.Empty)
            {
                base.Add(value);
                wasAdded = true;
            }
            // keep on false
        }

        // keep on false
        await Save();
        return wasAdded;
    }

    private void Load(bool loadImmediately)
    {
        if (loadImmediately) Load();
    }

    public async Task Save()
    {
        if (args.Save)
        {
            isSaving = true;
            var removedOrNotExists = false;
            //if (FS.ExistsFile(args.File))
            //{
            //    removedOrNotExists = FS.TryDeleteFile(args.File);
            //}
            if (removedOrNotExists)
            {
                string content;
                content = ReturnContent();
                await FileAsync.WriteAllTextAsync(args.File, content);
            }

            isSaving = false;
        }
    }

    private string ReturnContent()
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in this) stringBuilder.AppendLine(item!.ToString());
        return stringBuilder.ToString();
    }

    public override string ToString()
    {
        return ReturnContent();
    }

    #region base

    public PpkOnDriveDevCodeBase(PpkOnDriveDevCodeArgs args)
    {
        this.args = args;
        File.AppendAllText(args.File, "");
        //FS.CreateFileIfDoesntExists(args.File);
        Load(args.Load);
        if (args.LoadChangesFromDrive)
        {
            w = new FileSystemWatcher(Path.GetDirectoryName(args.File)!);
            w.Filter = args.File;
            w.Changed += W_Changed;
        }
    }

    private void W_Changed(object sender, FileSystemEventArgs e)
    {
        if (!isSaving) Load();
    }

    #endregion
}
