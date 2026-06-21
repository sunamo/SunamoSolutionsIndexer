namespace SunamoDevCode.SunamoSolutionsIndexer;

/// <summary>
/// Project name constants
/// </summary>
public class ProjectsNames
{
    /// <summary>
    /// Project name for sunamo.web.
    /// </summary>
    public const string SunamoWeb = "sunamo.web";
    /// <summary>
    /// Project name for PlatformIndependentNuGetPackages.
    /// </summary>
    public const string PlatformIndependentNuGetPackages = "PlatformIndependentNuGetPackages";
    /// <summary>
    /// Project name for sunamo.
    /// </summary>
    public const string Sunamo = "sunamo";
    /// <summary>
    /// Project name for CredentialsWithoutDep.
    /// </summary>
    public const string CredentialsWithoutDep = "CredentialsWithoutDep";

    /// <summary>
    /// List of all known project names.
    /// </summary>
    public static List<string> All = new List<string>([SunamoWeb, PlatformIndependentNuGetPackages, Sunamo, CredentialsWithoutDep]);
}