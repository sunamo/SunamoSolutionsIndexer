namespace SunamoSolutionsIndexer.Interfaces;

public interface ISolutionFolderSerialize
{
    string displayedText { get; set; }
    string fullPathFolder { get; set; }
    string LongName { get; }
    string nameSolution { get; }
    string RunOne { get; }
    string ShortName { get; }

    string ToString();
}
