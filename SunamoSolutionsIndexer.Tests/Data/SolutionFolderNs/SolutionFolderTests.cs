// variables names: ok
namespace SunamoDevCode.Tests.Data.SolutionFolderNs;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SunamoDevCode.SunamoSolutionsIndexer;

/// <summary>
/// Tests for SolutionFolder functionality including exe to release conversion.
/// </summary>
public class SolutionFolderTests //: TestsBase
{
    ILogger logger = NullLogger.Instance;

    /// <summary>
    /// Tests the ExeToRelease method with a sample console application.
    /// </summary>
    [Fact]
    public void ExeToReleaseTest()
    {
        var appWithoutProjectDistinction = "ConsoleApp1";
        var projectDistinction = "";

        FoldersWithSolutions.InsertIntoFwss(logger, @"E:\vs", null!);

        var app = SolutionsIndexerHelper.SolutionWithName(appWithoutProjectDistinction);
        app!.ExeToRelease(app, projectDistinction, true);


    }
}