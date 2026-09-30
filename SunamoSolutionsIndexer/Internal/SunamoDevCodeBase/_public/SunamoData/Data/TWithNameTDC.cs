namespace SunamoDevCode._public.SunamoData.Data;

internal class TWithNameTDC<T>
{
    public string name = string.Empty;
    public T t = default!;

    public TWithNameTDC()
    {
    }

    public TWithNameTDC(string name, T t)
    {
        this.name = name;
        this.t = t;
    }

    public override string ToString() => name;

    public static TWithNameTDC<T> Get(string name) => new() { name = name };
}
