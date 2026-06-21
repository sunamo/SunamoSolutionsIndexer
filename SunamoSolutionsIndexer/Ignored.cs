namespace SunamoDevCode;

/// <summary>
/// Determines whether project paths should be ignored based on naming conventions.
/// </summary>
public class Ignored
{
    /// <summary>
    /// Returns true if the specified path should be ignored (archived, UAP, mixin, or vcxproj).
    /// </summary>
    /// <param name="path">File path to check.</param>
    /// <returns>True if the path should be ignored.</returns>
    public static bool IsIgnored(string path)
    {
        if (path == null) return true;
        return path.Contains(StartWith.Archived) || path.Contains(StartWith.Uap) || path.Contains(StartWith.Mixin) ||
               path.EndsWith(EndsWith.VcxProj);
    }

    /// <summary>
    /// Path prefixes that indicate ignored projects.
    /// </summary>
    public class StartWith
    {
        /// <summary>
        /// UAP (Universal App Platform) project folder marker.
        /// </summary>
        public const string Uap = @"\_Uap\";
        /// <summary>
        /// Archived project folder marker.
        /// </summary>
        public const string Archived = @"_Archived";
        /// <summary>
        /// Mixin project folder marker.
        /// </summary>
        public const string Mixin = @"_Mixin";
    }

    /// <summary>
    /// File extensions that indicate ignored projects.
    /// </summary>
    public class EndsWith
    {
        /// <summary>
        /// Visual C++ project file extension.
        /// </summary>
        public const string VcxProj = ".vcxproj";
    }
}