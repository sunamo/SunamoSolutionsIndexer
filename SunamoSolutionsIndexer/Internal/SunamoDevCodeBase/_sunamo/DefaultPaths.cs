namespace SunamoDevCode._sunamo;

internal class DefaultPaths
{
    internal static bool IsIgnored(string path, string bpBb) => path.StartsWith(bpBb);
}
