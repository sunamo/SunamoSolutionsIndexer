namespace SunamoSolutionsIndexer._sunamo;

/// <summary>
/// Arguments for changing content of a collection.
/// </summary>
internal class ChangeContentArgsDC
{
    internal bool RemoveNull { get; set; } = false;
    internal bool RemoveEmpty { get; set; } = false;
    internal bool SwitchFirstAndSecondArg { get; set; } = false;
}