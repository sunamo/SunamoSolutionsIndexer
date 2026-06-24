namespace SunamoSolutionsIndexer;

public class Ignored
{
    public static bool IsIgnored(string path)
    {
        if (path == null) return true;
        return path.Contains(StartWith.Archived) || path.Contains(StartWith.Uap) || path.Contains(StartWith.Mixin) ||
               path.EndsWith(EndsWith.VcxProj);
    }

    public class StartWith
    {
        public const string Uap = @"\_Uap\";
        public const string Archived = @"_Archived";
        public const string Mixin = @"_Mixin";
    }

    public class EndsWith
    {
        public const string VcxProj = ".vcxproj";
    }
}
