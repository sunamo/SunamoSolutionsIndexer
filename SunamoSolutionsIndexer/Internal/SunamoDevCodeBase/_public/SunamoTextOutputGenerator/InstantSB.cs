namespace SunamoDevCode._public.SunamoTextOutputGenerator;

internal class InstantSB
{
    public StringBuilder StringBuilder { get; set; } = new StringBuilder();
    private string _tokensDelimiter;

    public InstantSB(string delimiter)
    {
        _tokensDelimiter = delimiter;
    }

    public int Length => StringBuilder.Length;

    public override string ToString()
    {
        string result = StringBuilder.ToString();
        return result;
    }

    public void AddItem(string value)
    {
        string text = value.ToString();
        if (text != _tokensDelimiter && text != "")
        {
            StringBuilder.Append(text + _tokensDelimiter);
        }
    }

    public void AddRaw(object content)
    {
        StringBuilder.Append(content.ToString());
    }

    public void AddItems(params string[] items)
    {
        foreach (var item in items)
        {
            AddItem(item);
        }
    }

    public void EndLine(object content)
    {
        string text = content.ToString()!;
        if (text != _tokensDelimiter && text != "")
        {
            StringBuilder.Append(text);
        }
    }

    public void AppendLine(string text)
    {
        EndLine(text + Environment.NewLine);
    }

    public void AppendLine()
    {
        EndLine(Environment.NewLine);
    }

    public void RemoveEndDelimiter()
    {
        StringBuilder.Remove(StringBuilder.Length - _tokensDelimiter.Length, _tokensDelimiter.Length);
    }

    public void Clear()
    {
        StringBuilder.Clear();
    }
}
