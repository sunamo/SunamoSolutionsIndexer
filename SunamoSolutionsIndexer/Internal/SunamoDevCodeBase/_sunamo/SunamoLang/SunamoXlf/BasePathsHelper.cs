namespace SunamoDevCode._sunamo.SunamoLang.SunamoXlf;

internal class BasePathsHelper
{
    // EN: Don't use this anymore. Pass in parameters to keep it as simple as possible.

    internal static string? VsProjects = null;
    internal static string ActualPlatform = null!;
    internal static string Vs = null!;
    internal const string CRepos = @"C:\repos";
    // Base path for main machine (MB): E:\vs\
    internal const string BpMb = @"E:\vs\";
    // Base path for Q environment: C:\repos\_\
    internal const string BpQ = @"C:\repos\_\";
    // Base path for VPS: C:\_\
    internal const string BpVps = @"C:\_\";
    // Base path for BitBucket: D:\Documents\BitBucket\
    internal const string BpBb = @"D:\Documents\BitBucket\";
}