namespace SunamoSolutionsIndexer._sunamo;

/// <summary>
/// Result of an operation with data or exception.
/// </summary>
public class ResultWithExceptionDC<T>
{
    public T Data { get; set; } = default!;

    public string Exc { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public ResultWithExceptionDC(T data)
    {
        Data = data;
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public ResultWithExceptionDC(string exc)
    {
        this.Exc = exc;
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public ResultWithExceptionDC(Exception exc)
    {
        this.Exc = Exceptions.TextOfExceptions(exc);
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public ResultWithExceptionDC()
    {
    }
}
