namespace SunamoDevCode._sunamo.SunamoConverters.Converts;

internal class ConvertSnakeConvention
{
    static string Sanitize(string input)
    {
        var text = new StringBuilder(input.Replace("", "_").Replace("__", "_"));
        for (int i = text.Length - 1; i >= 0; i--)
        {
            var character = text[i];
            if (!char.IsLetter(character) && !char.IsDigit(character) && character != '_')
            {
                text = text.Remove(i, 1);
            }
        }
        return text.ToString();
    }

    internal static string ToConvention(string text)
    {
        var result = string.Concat(text.Select((character, index) => index > 0 && char.IsUpper(character) ? "_" + character.ToString() : character.ToString())).ToLower();
        return Sanitize(result);
    }
}
