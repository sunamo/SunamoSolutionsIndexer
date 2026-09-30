namespace SunamoDevCode._sunamo.SunamoConverters.Converts;

internal class ConvertPascalConvention
{
    // Converts string to PascalCase convention
    // Will include numbers
    // hello world = HelloWorld
    // Hello world = HelloWorld
    // helloWorld = HelloWorld
    internal static string ToConvention(string input)
    {
        var stringBuilder = new StringBuilder();
        bool shouldCapitalizeNext = false;
        foreach (char character in input)
        {
            if (shouldCapitalizeNext)
            {
                if (char.IsUpper(character))
                {
                    shouldCapitalizeNext = false;
                    stringBuilder.Append(character);
                    continue;
                }
                else if (char.IsLower(character))
                {
                    shouldCapitalizeNext = false;
                    stringBuilder.Append(char.ToUpper(character));
                    continue;
                }
                else if (char.IsDigit(character))
                {
                    shouldCapitalizeNext = true;
                    stringBuilder.Append(character);
                    continue;
                }
                else
                {
                    continue;
                }
            }
            if (char.IsUpper(character))
            {
                stringBuilder.Append(character);
            }
            else if (char.IsLower(character))
            {
                stringBuilder.Append(character);
            }
            else if (char.IsDigit(character))
            {
                stringBuilder.Append(character);
            }
            else
            {
                shouldCapitalizeNext = true;
            }
        }
        var result = stringBuilder.ToString().Trim();
        var resultBuilder = new StringBuilder(result);
        resultBuilder[0] = char.ToUpper(resultBuilder[0]);
        return result;
    }
}
