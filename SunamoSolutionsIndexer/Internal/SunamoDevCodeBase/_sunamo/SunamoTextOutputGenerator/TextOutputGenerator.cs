namespace SunamoDevCode._sunamo.SunamoTextOutputGenerator;

internal class TextOutputGenerator
{
    // EN: During NuGet conversion, I changed this to TextBuilderDC StringBuilder = TextBuilder.Create();
    // but that was probably a mistake, now in _sunamo I have Create() which returns null instead of using ctor
    // so I'm reverting it back.
    internal StringBuilder StringBuilder = new StringBuilder();

    #region Static texts
    #endregion
    #region Templates
    #endregion
    #region AppendLine
    internal void AppendLine(StringBuilder text)
    {
        StringBuilder.AppendLine(text.ToString());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Append(string text)
    {
        StringBuilder.Append(text);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void AppendLine(string text)
    {
        StringBuilder.AppendLine(text);
    }
    #endregion
    #region Other adding methods
    internal void Header(string headerText)
    {
        StringBuilder.AppendLine();
        AppendLine(headerText);
        StringBuilder.AppendLine();
    }
    #endregion
    public override string ToString()
    {
        var result = StringBuilder.ToString();
        return result;
    }
    #region List
    internal void ListSB(StringBuilder onlyStart, string headerText)
    {
        Header(headerText);
        AppendLine(onlyStart);
    }

    internal void List<Value>(IList<Value> items, string delimiter = "\r\n", string whenNoEntries = "")
    {
        if (items.Count == 0)
        {
            StringBuilder.AppendLine(whenNoEntries);
        }
        else
        {
            foreach (var item in items)
            {
                Append(item!.ToString() + delimiter);
            }
        }
    }

    internal void List(IList<string> items, string header)
    {
        List<string, string>(items, header, new TextOutputGeneratorArgs { HeaderWrappedEmptyLines = true, InsertCount = false });
    }

    internal void List<Header, Value>(IList<Value> items, Header header, TextOutputGeneratorArgs args) where Header : IEnumerable<char>
    {
        if (args.HeaderWrappedEmptyLines)
        {
            StringBuilder.AppendLine();
        }
        StringBuilder.AppendLine(header + ":");
        if (args.HeaderWrappedEmptyLines)
        {
            StringBuilder.AppendLine();
        }
        List(items, args.Delimiter, args.WhenNoEntries);
    }
    #endregion
    #region Paragraph
    internal void Paragraph(StringBuilder wrongNumberOfParts, string header)
    {
        string text = wrongNumberOfParts.ToString().Trim();
        Paragraph(text, header);
    }

    internal void Paragraph(string text, string header)
    {
        if (text != string.Empty)
        {
            StringBuilder.AppendLine(header + ":");
            StringBuilder.AppendLine(text);
            StringBuilder.AppendLine();
        }
    }
    #endregion
    #region Dictionary
    internal void Dictionary(Dictionary<string, List<string>> dictionary)
    {
        foreach (var item in dictionary)
        {
            List(item.Value, item.Key);
        }
    }
    #endregion
}
