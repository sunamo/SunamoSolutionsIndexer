namespace SunamoDevCode._public;

internal class OutRefDC<T, U>
{
    public OutRefDC(T firstValue, U secondValue)
    {
        Item1 = firstValue;
        Item2 = secondValue;
    }
    public T Item1 { get; set; }
    public U Item2 { get; set; }
}
