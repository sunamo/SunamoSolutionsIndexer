namespace SunamoSolutionsIndexer;

public class ProjectsNames
{
    public const string sunamoWeb = "sunamo.web";
    public const string sunamoWithoutLocalDep = "sunamoWithoutLocalDep";
    public const string sunamo = "sunamo";
    public const string CredentialsWithoutDep = "CredentialsWithoutDep";

    public static List<string> All = CAG.ToList<string>(sunamoWeb, sunamoWithoutLocalDep, sunamo, CredentialsWithoutDep);
}
