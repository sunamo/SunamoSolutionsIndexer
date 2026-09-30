namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

/// <summary>
/// Text builder that supports undo.
/// </summary>
public class TextBuilderDC
{
    private bool _canUndo = false;
    private int _lastIndex = -1;
    private string _lastText = "";
    public StringBuilder stringBuilder = null!;
    public string prependEveryNoWhite { get; set; } = string.Empty;

    public List<string> list { get; set; } = null!;
    private bool _useList = false;

    /// <summary>
    /// Clears the content.
    /// </summary>
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

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
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

    /// <summary>
    /// Reports that undo is not allowed.
    /// </summary>
    private void UndoIsNotAllowed(string what)
    {
        ThrowEx.IsNotAllowed(what);
    }

    /// <summary>
    /// Appends the text.
    /// </summary>
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

    /// <summary>
    /// Remembers the text for undo.
    /// </summary>
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

    /// <summary>
    /// Appends the text and a new line.
    /// </summary>
    public void AppendLine()
    {
        Append(Environment.NewLine);
    }

    /// <summary>
    /// Appends the text and a new line.
    /// </summary>
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

    /// <summary>
    /// Returns the generated text.
    /// </summary>
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