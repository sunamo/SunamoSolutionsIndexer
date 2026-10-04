namespace SunamoSolutionsIndexer.Tests;

public class GitHelperTests
{
    [Fact]
    public void NameOfRepoFromOriginUriTest()
    {
        string actual = GitHelper.NameOfRepoFromOriginUri(@"https://github.com/sunamo/PlatformIndependentNuGetPackages.git");
        Assert.Equal("PlatformIndependentNuGetPackages", actual);
    }
}