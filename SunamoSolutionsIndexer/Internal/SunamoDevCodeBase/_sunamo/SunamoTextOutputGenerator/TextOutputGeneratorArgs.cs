namespace SunamoDevCode._sunamo.SunamoTextOutputGenerator;

internal class TextOutputGeneratorArgs
{
    internal bool HeaderWrappedEmptyLines = true;
    internal bool InsertCount = false;
    internal string WhenNoEntries = "No entries";
    internal string Delimiter = Environment.NewLine;
    internal TextOutputGeneratorArgs()
    {
    }
    internal TextOutputGeneratorArgs(bool headerWrappedEmptyLines, bool insertCount)
    {
        this.HeaderWrappedEmptyLines = headerWrappedEmptyLines;
        this.InsertCount = insertCount;
    }
}