namespace SunamoSolutionsIndexer.Interfaces;

public interface ISolutionFolderSerialize
{
    string DisplayedText { get; set; }
    string FullPathFolder { get; set; }
    string LongName { get; }
    string NameSolution { get; }
    string RunOne { get; }
    string ShortName { get; }

    string ToString();
}