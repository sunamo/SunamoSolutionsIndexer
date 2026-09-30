namespace SunamoDevCode._public.SunamoData.Data;

// Must have always entered both from and to
// None of event could have unlimited time!
internal class FromToDC : FromToTSHDC<long>
{
    public static FromToDC Empty = new(true);

    public FromToDC()
    {
    }

    private FromToDC(bool empty)
    {
        this.empty = empty;
    }

    public FromToDC(long from, long to, FromToUseDC ftUse = FromToUseDC.DateTime)
    {
        this.from = from;
        this.to = to;
        this.ftUse = ftUse;
    }
}
