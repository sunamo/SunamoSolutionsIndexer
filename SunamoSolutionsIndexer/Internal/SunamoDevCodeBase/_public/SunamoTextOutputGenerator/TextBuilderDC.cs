namespace SunamoSolutionsIndexer.Internal._public.SunamoTextOutputGenerator;

public class TextBuilderDC
{
    private static Type type = typeof(TextBuilderDC);
    private bool _canUndo = false;
    private int _lastIndex = -1;
    private string _lastText = "";
    public StringBuilder stringBuilder = null!;
    public string prependEveryNoWhite { get; set; } = string.Empty;

    public List<string> list { get; set; } = null!;
    private bool _useList = false;

    public void Clear()
    {
        if (_useList)
        {
            list.Clear();
        }
        else
        {
            stringBuilder.Clear();
        }
    }

    public static TextBuilderDC Create(bool useList = false)
    {
        return new TextBuilderDC(useList);
    }

    public TextBuilderDC(bool useList = false)
    {
        _useList = useList;
        if (useList)
        {
            list = new List<string>();
        }
        else
        {
            stringBuilder = new StringBuilder();
        }
    }

    public bool CanUndo
    {
        get
        {
            if (_useList)
            {
                return false;
            }
            return _canUndo;
        }
        set
        {
            _canUndo = value;
            if (!value)
            {
                _lastIndex = -1;
                _lastText = "";
            }
        }
    }

    private void UndoIsNotAllowed(string what)
    {
        ThrowEx.IsNotAllowed(what);
    }

    public void Undo()
    {
        if (_useList)
        {
            UndoIsNotAllowed("Undo");
        }
        if (_lastIndex != -1)
        {
            stringBuilder.Remove(_lastIndex, _lastText.Length);
        }
    }

    public void Append(string text)
    {
        if (_useList)
        {
            if (list.Count > 0)
            {
                list[list.Count - 1] += text;
            }
            else
            {
                list.Add(text);
            }
        }
        else
        {
            SetUndo(text);
            stringBuilder.Append(prependEveryNoWhite);
            stringBuilder.Append(text);
        }
    }

    private void SetUndo(string text)
    {
        if (_useList)
        {
            UndoIsNotAllowed("SetUndo");
        }
        if (CanUndo)
        {
            _lastIndex = stringBuilder.Length;
            _lastText = text;
        }
    }

    public void Append(object text)
    {
        string textString = text.ToString()!;
        SetUndo(textString);
        Append(textString);
    }

    public void AppendLine()
    {
        Append(Environment.NewLine);
    }

    public void AppendLine(string text)
    {
        if (_useList)
        {
            list.Add(prependEveryNoWhite + text);
        }
        else
        {
            SetUndo(text);
            stringBuilder.Append(prependEveryNoWhite + text + Environment.NewLine);
        }
    }

    public override string ToString()
    {
        if (_useList)
        {
            return string.Join(Environment.NewLine, list);
        }
        else
        {
            return stringBuilder.ToString();
        }
    }
}
