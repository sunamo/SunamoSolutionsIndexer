namespace SunamoDevCode._public.SunamoData.Data;

internal class FromToList
{
    public List<FromToDC> Ranges { get; set; } = new();

    public bool IsInRange(int value)
    {
        foreach (var item in Ranges)
            if (value < item.to && value > item.from)
                return true;
        return false;
    }
}
