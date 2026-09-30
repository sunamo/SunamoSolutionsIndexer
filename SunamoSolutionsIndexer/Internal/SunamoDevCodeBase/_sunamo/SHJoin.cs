namespace SunamoDevCode._sunamo;

internal class SHJoin
{
    internal static string JoinNL<T>(List<T> list)
    {
        var strings = list.ConvertAll(item => item!.ToString());
        return string.Join("\n", strings);
    }

    internal static string JoinNL(List<string> list) => string.Join("\n", list);
}
