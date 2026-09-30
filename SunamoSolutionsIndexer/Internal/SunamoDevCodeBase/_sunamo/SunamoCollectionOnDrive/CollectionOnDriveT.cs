namespace SunamoDevCode._sunamo.SunamoCollectionOnDrive;

internal sealed class CollectionOnDriveT<T>(ILogger logger) : CollectionOnDriveBase<T>(logger) where T : IParserDC
{
    internal async override Task Load(bool isRemovingDuplicates)
    {
        if (File.Exists(args.path))
        {
            var lineNumber = 0;
            foreach (var line in SHGetLines.GetLines(await FileAsync.ReadAllTextAsync(args.path)))
            {
                var instance = (T?)Activator.CreateInstance(typeof(T));
                ThrowEx.IsNull(nameof(instance), instance);
                instance!.Parse(line);
                await AddWithSave(instance);
                lineNumber++;
            }
        }
    }
}
