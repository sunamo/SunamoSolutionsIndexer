namespace SunamoSolutionsIndexer._sunamo;
internal class FS
{
    internal static List<string> FoldersWithSubfolder(string solutionFolder, string folderName)
    {
        var subFolders = Directory.GetDirectories(solutionFolder, "*", SearchOption.AllDirectories);
        List<string> result = new List<string>();

        foreach (var item in subFolders)
        {
            /*
Zde mám chybu:
System.IO.DirectoryNotFoundException: 'Could not find a part of the path
            'E:\vs\Projects\sunamoWithoutLocalDep.net\Clients\node_modules\napi-wasm'.'

            to musí být nějaká <|>, protože zde se mi to má dostat jen při sunamo nebo swod
            nikoliv při sunamo.net
            */

            var subf = Directory.GetDirectories(item, folderName, SearchOption.TopDirectoryOnly).ToList();
            if (subf.Count == 1)
            {
                result.Add(item);
            }
        }

        return result;
    }
}
