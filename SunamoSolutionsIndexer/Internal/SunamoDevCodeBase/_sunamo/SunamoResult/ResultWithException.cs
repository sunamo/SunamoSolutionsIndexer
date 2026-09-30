namespace SunamoDevCode._sunamo.SunamoResult;

internal class ResultWithException<T>
{
    internal T Data { get; set; } = default!;
    internal string? Exc { get; set; }

    internal ResultWithException(T data)
    {
        Data = data;
    }

    internal ResultWithException(string exc)
    {
        this.Exc = exc;
    }

    internal ResultWithException(Exception exc)
    {
        this.Exc = exc.Message;
    }

    internal ResultWithException()
    {
    }
}