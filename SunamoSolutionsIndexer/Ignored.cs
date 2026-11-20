namespace SunamoSolutionsIndexer;

/// <summary>
/// Represents the ignored type.
/// </summary>
public class Ignored
{
    /// <summary>
    /// Is ignored.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>The is ignored result.</returns>
    public static bool IsIgnored(string path)
    {
        if (path == null) return true;
        return path.Contains(StartWith.Archived) || path.Contains(StartWith.Uap) || path.Contains(StartWith.Mixin) ||
               path.EndsWith(EndsWith.VcxProj);
    }

    /// <summary>
    /// Represents the start with type.
    /// </summary>
    public class StartWith
    {
        /// <summary>
        /// The uap constant.
        /// </summary>
        public const string Uap = @"\_Uap\";
        /// <summary>
        /// The archived constant.
        /// </summary>
        public const string Archived = @"_Archived";
        /// <summary>
        /// The mixin constant.
        /// </summary>
        public const string Mixin = @"_Mixin";
    }

    /// <summary>
    /// Represents the ends with type.
    /// </summary>
    public class EndsWith
    {
        /// <summary>
        /// The vcx proj constant.
        /// </summary>
        public const string VcxProj = ".vcxproj";
    }
}
