
namespace SunamoSolutionsIndexer.Data.SolutionFolderNs;
using SunamoSolutionsIndexer.Interfaces;
using SunamoStringParts;


public partial class SolutionFolderSerialize : IListBoxHelperItem, ISolutionFolderSerialize
{
    public static Type type = typeof(SolutionFolderSerialize);

    string _displayedText = "";

    /// <summary>
    /// Is assingned in FoldersWithSolutions
    /// Zobrazovaný text v LB, například 2013/PHP Projects/PHPWebSite
    /// </summary>
    public string displayedText
    {
        get => _displayedText;
        set => _displayedText = value;
    }
    public string _fullPathFolder = "";
    public string _nameSolution = "";
    /// <summary>
    /// Defaultly null
    /// Is filled up in SolutionsIndexerHelper.GetProjectFolderAndSlnPath
    /// Scripts_Projects and so.
    /// </summary>
    public string projectFolder;
    /// <summary>
    /// Is not full path to sln folder, for these reason it's here _fullPathFolder.
    /// Is filled up in AllProjectsSearchHelper.GetProjectFolderAndSlnPath
    /// _Uap/apps
    /// relative path to solution folder from Project folder
    /// </summary>
    public string slnFullPath;
    /// <summary>
    /// C:\Documents\vs\sunamo\
    /// Jedná se o cestu ke složce, proto musí mít na konci backslash, tak jako proměnné složek ve všech mých aplikacích. 
    /// Proto všude dej veřejnou jen vlastnost která když proměnnou vrátí hodnotu bez backslash, vyhodí výjimku
    /// Plná cesta k řešení, musí to být v samostatné proměnné a nemůže se to počítat z displayedText, protože existují speciální složky, které třeba mohou být v C:\Mona a ne dokumenty
    /// </summary>
    public string fullPathFolder
    {
        get
        {
            return _fullPathFolder;
        }
        set
        {
            ThrowEx.CheckBackslashEnd(Exc.GetStackTrace(), value);
            _fullPathFolder = value;
            _nameSolution = Path.GetFileName(value.TrimEnd(AllChars.bs));
            if (SolutionsIndexerSettings.ignorePartAfterUnderscore)
            {
                _nameSolution = SHParts.RemoveAfterLast(_nameSolution, AllChars.lowbar);
            }
        }
    }
    /// <summary>
    /// Konečný název řešení, například PHPWebSite
    /// If contains hiearchy (as _Uap, won't be included)
    /// </summary>
    public string nameSolution
    {
        get
        {
            return _nameSolution;
        }
    }
    public string RunOne
    {
        get
        {
            return fullPathFolder;
        }
    }
    public string ShortName => _nameSolution;
    public string LongName => _fullPathFolder;
}
