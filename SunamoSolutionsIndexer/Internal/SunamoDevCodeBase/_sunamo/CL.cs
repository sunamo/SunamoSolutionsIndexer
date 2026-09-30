namespace SunamoDevCode._sunamo;

internal class CL
{
    internal static string AskForEnter(string prompt, bool appendAfterEnter,
        string returnWhenIsNotNull)
    {
        if (returnWhenIsNotNull == null)
        {
            if (appendAfterEnter)
                prompt = FromKey("Enter") + " " + prompt + " ";
            prompt +=
                ". " + FromKey("ForExitEnter") + " -1. Is possible enter also nothing - just enter";
            return prompt;
        }
        return returnWhenIsNotNull;
    }

    private static string FromKey(string key)
    {
        return key;
    }

    internal static string UserMustTypeMultiLine(string prompt, params string[] anotherPossibleAftermOne)
    {
        string? line = null;
        Information(AskForEnter(prompt, true, ""));
        StringBuilder stringBuilder = new();
        while ((line = Console.ReadLine()) != null)
        {
            if (line == "-1") break;
            stringBuilder.AppendLine(line);
            if (anotherPossibleAftermOne.Contains(line)) break;
        }
        var result = stringBuilder.ToString().Trim();
        return result;
    }

    private static void Information(string value)
    {
        Console.WriteLine(value);
    }

    internal static void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
}
