namespace SunamoDevCode;

internal abstract class GeneratorCodeAbstract
{
    // Use ToString() instead of public access
    protected string Final = "";

    protected InstantSB sb = new(" ");
    public XmlDoc xmlDoc;

    public GeneratorCodeAbstract()
    {
        xmlDoc = new XmlDoc(sb);
    }

    public void AddTab2(int tabCount, string text)
    {
        sb.AddItem(AddTab(tabCount, text));
    }

    // EN: Ends a brace and adds a new line. After calling this method for elements like methods, properties, or constructors, it's recommended to also call sb.AppendLine().
    // CZ: Ukončí složenou závorku a přidá nový řádek. Za voláním této metody pokud ukončuje nějaký celek jako jsou metody, vlastnosti nebo konstruktor je vhodné volat ještě sb.AppendLine().
    public void EndBrace(int tabCount)
    {
        //sb.AppendLine();
        AddTab(tabCount);
        //sb.AppendLine();
        sb.AppendLine("}");
    }

    // EN: Starts a brace. This is the only method here that adds a new line at the beginning.
    // CZ: Přidá nový řádek, složenou závorku. Je to jediná zdejší metoda která na začátku přidává nový řádek.
    public void StartBrace(int tabCount)
    {
        // Line always ending previous command
        //sb.AppendLine();
        AddTab(tabCount);
        sb.AppendLine("{");
        //sb.AppendLine();
    }

    public void StartParenthesis()
    {
        sb.AddItem("(");
    }

    public void EndParenthesis()
    {
        sb.AddItem(")");
    }

    public void AppendLine()
    {
        sb.AppendLine();
    }

    // EN: Appends a formatted line with tabs
    // CZ: Přidá formátovaný řádek s tabulátory
    public void AppendLine(int tabCount, string format, params object[] args)
    {
        if (args.Length != 0)
            sb.AppendLine(AddTab(tabCount, string.Format(format, args)));
        else
            sb.AppendLine(AddTab(tabCount, format));
    }

    // EN: Appends formatted text with tabs (without newline)
    // CZ: Přidá formátovaný text s tabulátory (bez nového řádku)
    public void Append(int tabCount, string format, params object[] args)
    {
        if (args.Length != 0)
            sb.AddItem(AddTab(tabCount, string.Format(format, args)));
        else
            sb.AddItem(AddTab(tabCount, format));
    }

    // EN: Assigns a value to a property/field, converting it to string
    // CZ: Přiřadí hodnotu vlastnosti/poli, převádí na string
    public void AssignValue(int tabCount, string objectName, string variableName, object value, bool addToHyphens)
    {
        string valueString;
        if (value.GetType() == typeof(bool))
            valueString = value.ToString()!.ToLower();
        else
            valueString = value.ToString()!;
        AssignValue(tabCount, objectName, variableName, valueString, addToHyphens);
    }

    // EN: Returns the generated code and resets the string builder
    // CZ: Vrátí vygenerovaný kód a resetuje string builder
    public override string ToString()
    {
        var result = sb.ToString();
        sb = new InstantSB(" ");
        return result;
    }

    public void AddTab(int tabCount)
    {
        //tabCount += 1;
        for (var i = 0; i < tabCount; i++) sb.AddRaw("\t");
    }

    // EN: Adds tabs to each line of the text
    // CZ: Přidá tabulátory na začátek každého řádku textu
    public static string AddTab(int tabCount, string text)
    {
        var lines = SHGetLines.GetLines(text);
        for (var i = 0; i < lines.Count; i++)
        {
            lines[i] = lines[i].Trim();
            for (var tabIndex = 0; tabIndex < tabCount; tabIndex++) lines[i] = "\t" + lines[i];
        }

        var result = string.Join(Environment.NewLine, lines);
        return result;
    }
}
