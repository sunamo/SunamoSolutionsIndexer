namespace SunamoSolutionsIndexer._sunamo;

/// <summary>
/// Progress bar.
/// </summary>
public interface IProgressBarDC
{
    bool IsRegistered { get; set; }
    int WriteOnlyDividableBy { get; set; }
    void Init(IPercentCalculatorDC pc);
    void Init(IPercentCalculatorDC pc, bool isNotUt);
    void DoneOne(object asyncResult);
    void DoneOne();
    void DoneOne(int count);
    void Start(int totalCount);
    void Done();
}
