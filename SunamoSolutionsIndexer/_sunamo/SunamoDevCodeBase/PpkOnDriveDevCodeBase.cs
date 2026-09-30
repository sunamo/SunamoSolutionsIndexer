namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

/// <summary>
/// Base class of a list persisted in a file.
/// </summary>
public abstract class PpkOnDriveDevCodeBase<T> : List<T>
{

    protected PpkOnDriveDevCodeArgs args;


    private bool isSaving;

    // Must use FileSystemWatcher, not FileSystemWatcher because its in sunamo, not desktop
    private readonly FileSystemWatcher w = null!;

    /// <summary>
    /// Clears the content.
    /// </summary>
    public new async Task Clear()
    {
        base.Clear();
        await Save();
    }

    public abstract
        Task
        Load();

    /// <summary>
    /// Loads content of the file.
    /// </summary>
    private void Load(bool loadImmediately)
    {
        if (loadImmediately) Load();
    }

    /// <summary>
    /// Saves content to the file.
    /// </summary>
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

    /// <summary>
    /// Returns content to save.
    /// </summary>
    private string ReturnContent()
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in this) stringBuilder.AppendLine(item!.ToString());
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Returns the generated text.
    /// </summary>
    public override string ToString()
    {
        return ReturnContent();
    }


    /// <summary>
    /// Initializes a new instance.
    /// </summary>
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

    /// <summary>
    /// Reloads content when the file changes.
    /// </summary>
    private void W_Changed(object sender, FileSystemEventArgs e)
    {
        if (!isSaving) Load();
    }

}