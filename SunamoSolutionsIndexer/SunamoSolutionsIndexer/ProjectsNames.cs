namespace SunamoSolutionsIndexer;

/// <summary>
/// Represents the projects names type.
/// </summary>
public class ProjectsNames
{
    /// <summary>
    /// The sunamo web constant.
    /// </summary>
    public const string SunamoWeb = "sunamo.web";
    /// <summary>
    /// The platform independent nu get packages constant.
    /// </summary>
    public const string PlatformIndependentNuGetPackages = "PlatformIndependentNuGetPackages";
    /// <summary>
    /// The sunamo constant.
    /// </summary>
    public const string Sunamo = "sunamo";
    /// <summary>
    /// The credentials without dep constant.
    /// </summary>
    public const string CredentialsWithoutDep = "CredentialsWithoutDep";

    /// <summary>
    /// The all.
    /// </summary>
    public static List<string> All = new List<string>([SunamoWeb, PlatformIndependentNuGetPackages, Sunamo, CredentialsWithoutDep]);
}
