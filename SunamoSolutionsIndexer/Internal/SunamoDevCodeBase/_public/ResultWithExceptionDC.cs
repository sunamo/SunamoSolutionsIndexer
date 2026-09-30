namespace SunamoSolutionsIndexer.Internal._public;

public class ResultWithExceptionDC<T>
{
    public T Data { get; set; } = default!;

    public string Exc { get; set; } = null!;

    public ResultWithExceptionDC(T data)
    {
        Data = data;
    }

    public ResultWithExceptionDC(string exc)
    {
        this.Exc = exc;
    }

    public ResultWithExceptionDC(Exception exc)
    {
        this.Exc = Exceptions.TextOfExceptions(exc);
    }

    public ResultWithExceptionDC()
    {
    }
}
