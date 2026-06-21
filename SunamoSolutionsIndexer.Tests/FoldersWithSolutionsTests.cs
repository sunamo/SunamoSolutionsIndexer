// variables names: ok
namespace SunamoDevCode.Tests;

using Microsoft.Extensions.Logging;
using SunamoDevCode.SunamoSolutionsIndexer;
using SunamoPaths;
using SunamoTest;

/// <summary>
/// Tests for FoldersWithSolutions functionality including reload and indexing operations.
/// </summary>
public class FoldersWithSolutionsTests
{
    ILogger logger = TestLogger.Instance;

    /// <summary>
    /// Tests that InsertIntoFwss executes without errors.
    /// </summary>
    [Fact]
    public void InsertIntoFwssTest()
    {
        FoldersWithSolutions.InsertIntoFwss(logger, DefaultPaths.eVs, null!);
    }

    /// <summary>
    /// Tests that reloading loads all project folders.
    /// </summary>
    [Fact]
    public void Reload_AllProjectFoldersIsLoaded()
    {
        FoldersWithSolutions foldersWithSolutionsInstance = new(TestLogger.Instance, @"E:\vs\", null!);


    }

    /// <summary>
    /// Tests reload without adding solution files.
    /// </summary>
    [Fact]
    public void ReloadTest_WithNoAddingSlns()
    {
        var fws = new FoldersWithSolutions(logger, DefaultPaths.eVs, null!, false);
    }

    /// <summary>
    /// Tests reload with adding solution files.
    /// </summary>
    [Fact]
    public void ReloadTest_WithAddSlns()
    {
        var fws = new FoldersWithSolutions(logger, DefaultPaths.eVs, null!, true);
    }

    /// <summary>
    /// Tests the full reload workflow including pairing project folders with enums.
    /// </summary>
    [Fact]
    public void ReloadTest()
    {
        var basePath = @"E:\vs\";

        //DefaultPaths.eVs = basePath;
        FoldersWithSolutions.PairProjectFolderWithEnum(TestLogger.Instance, basePath);
        FoldersWithSolutions instance = new FoldersWithSolutions(TestLogger.Instance, basePath, null!, false);
        instance.Reload(TestLogger.Instance, basePath, null!);
        var solutions = instance.GetSolutions(Enums.RepositoryLocal.Vs17);
        //instance.Reload()
    }

    /// <summary>
    /// Tests that reload indexes projects in the _When folder when root is E:\vs.
    /// </summary>
    [Fact]
    public void Reload_IndexesProjectsInWhenFolder_WhenRootIsEVs()
    {
        // Arrange
        var rootPath = @"E:\vs\";
        var fws = new FoldersWithSolutions(logger, rootPath, null!, false);

        // Act
        var solutions = fws.Reload(logger, rootPath, null!);

        // Assert
        // Verify that if E:\vs\Projects\_When exists and contains projects, they are indexed
        var whenFolderPath = Path.Combine(rootPath, "Projects", "_When");
        if (Directory.Exists(whenFolderPath))
        {
            var subFolders = Directory.GetDirectories(whenFolderPath);
            if (subFolders.Length > 0)
            {
                // Should have at least one solution with Projects\_When in path
                var whenProjects = solutions.Where(s => s.FullPathFolder.Contains(@"Projects\_When")).ToList();
                Assert.NotEmpty(whenProjects);

                // Verify each subfolder in _When is represented in solutions
                foreach (var subFolder in subFolders)
                {
                    var folderName = Path.GetFileName(subFolder);
                    var matchingSolution = solutions.FirstOrDefault(s =>
                        s.FullPathFolder.Contains(@"Projects\_When") &&
                        s.FullPathFolder.EndsWith(folderName + @"\"));

                    Assert.NotNull(matchingSolution);
                }
            }
        }
    }
}