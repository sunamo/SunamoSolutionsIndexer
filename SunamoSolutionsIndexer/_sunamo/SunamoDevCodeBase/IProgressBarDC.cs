namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

/// <summary>
/// Progress bar.
/// </summary>
public interface IProgressBarDC
{
    /// <summary>
    /// Gets or sets the is registered.
    /// </summary>
    bool IsRegistered { get; set; }
    /// <summary>
    /// Gets or sets the write only dividable by.
    /// </summary>
    int WriteOnlyDividableBy { get; set; }
    /// <summary>
    /// Init.
    /// </summary>
    /// <param name="pc">The pc.</param>
    /// <returns>The init result.</returns>
    void Init(IPercentCalculatorDC pc);
    /// <summary>
    /// Init.
    /// </summary>
    /// <param name="pc">The pc.</param>
    /// <returns>The init result.</returns>
    void Init(IPercentCalculatorDC pc, bool isNotUt);
    /// <summary>
    /// Done one.
    /// </summary>
    /// <param name="pc">The pc.</param>
    /// <returns>The done one result.</returns>
    void DoneOne(object asyncResult);
    /// <summary>
    /// Done one.
    /// </summary>
    /// <param name="pc">The pc.</param>
    /// <returns>The done one result.</returns>
    void DoneOne();
    /// <summary>
    /// Done one.
    /// </summary>
    /// <param name="pc">The pc.</param>
    /// <returns>The done one result.</returns>
    void DoneOne(int count);
    /// <summary>
    /// Start.
    /// </summary>
    /// <param name="pc">The pc.</param>
    /// <returns>The start result.</returns>
    void Start(int totalCount);
    /// <summary>
    /// Done.
    /// </summary>
    /// <param name="pc">The pc.</param>
    void Done();
}