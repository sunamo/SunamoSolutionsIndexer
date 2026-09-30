namespace SunamoDevCode._sunamo.SunamoFileSystem;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal partial class FS
{
    internal static void DeleteFoldersWhichNotContains(string rootPath, string folderPattern, IList<string> mustContainPatterns)
    {
        var folders = Directory.GetDirectories(rootPath, folderPattern, SearchOption.AllDirectories).ToList();
        for (int i = folders.Count - 1; i >= 0; i--)
        {
            if (CA.ReturnWhichContainsIndexes(folders[i], mustContainPatterns).Count != 0)
            {
                folders.RemoveAt(i);
            }
        }

        foreach (var folder in folders)
        {
        //FS.DeleteF
        }
    }

    internal static bool IsAbsolutePath(string path)
    {
        return !String.IsNullOrWhiteSpace(path) && path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) == -1 && Path.IsPathRooted(path) && !Path.GetPathRoot(path)!.Equals(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
    }

    internal static string GetAbsolutePath2(string relativePath, string baseDirectory)
    {
        var absolutePath = GetAbsolutePath(baseDirectory, relativePath);
        return Path.GetFullPath(absolutePath);
    }

    internal static void FileToDirectory(ref string path)
    {
        if (!path.EndsWith("\""))
            path = GetDirectoryName(path);
    }

    internal static string GetAbsolutePath(string baseDirectory, string relativePath)
    {
        FileToDirectory(ref baseDirectory);
        var currentDirectoryPrefix = "./";
        var parentDirectoryPrefix = "../";
        var parentDirectoryCount = 0;
        while (true)
            if (relativePath.StartsWith(currentDirectoryPrefix))
            {
                relativePath = relativePath.Substring(currentDirectoryPrefix.Length);
            }
            else if (relativePath.StartsWith(parentDirectoryPrefix))
            {
                parentDirectoryCount++;
                relativePath = relativePath.Substring(parentDirectoryPrefix.Length);
            }
            else
            {
                break;
            }

        var pathTokens = GetTokens(relativePath);
        pathTokens = pathTokens.Skip(parentDirectoryCount).ToList();
        pathTokens.Insert(0, baseDirectory);
        var result = Combine(pathTokens.ToArray());
        result = GetFullPath(result);
        return result;
    }

    internal static bool IsWindowsPathFormat(string argValue)
    {
        if (string.IsNullOrWhiteSpace(argValue))
            return false;
        var badFormat = false;
        if (argValue.Length < 3)
            return badFormat;
        if (!char.IsLetter(argValue[0]))
            badFormat = true;
        if (char.IsLetter(argValue[1]))
            badFormat = true;
        if (argValue.Length > 2)
            if (argValue[1] != '\\' && argValue[2] != '\\')
                badFormat = true;
        return !badFormat;
    }

    internal static string FirstCharUpper(ref string result)
    {
        if (IsWindowsPathFormat(result))
            result = SH.FirstCharUpper(result);
        return result;
    }

    internal static string FirstCharUpper(string text, bool isOnlyFirstChar = false)
    {
        if (text != null)
        {
            var substring = text.Substring(1);
            if (isOnlyFirstChar)
                substring = substring.ToLower();
            return text[0].ToString().ToUpper() + substring;
        }

        return null!;
    }

    internal static string GetFullPath(string path)
    {
        var result = Path.GetFullPath(path);
        FirstCharUpper(ref result);
        return result;
    }

    private static string CombineWorker(bool isFirstCharUpper, bool isFile, params string[] pathParts)
    {
        for (var i = 0; i < pathParts.Length; i++)
            pathParts[i] = pathParts[i].TrimStart('\\');
        var result = Path.Combine(pathParts);
        if (result[2] != '\\')
            result = result.Insert(2, "\"");
        if (isFirstCharUpper)
            result = SH.FirstCharUpper(ref result);
        else
            result = SH.FirstCharUpper(ref result);
        if (!isFile)
            // Cant return with end slash becuase is working also with files
            WithEndSlash(ref result);
        return result;
    }

    internal static string Combine(params string[] pathParts)
    {
        return CombineWorker(true, false, pathParts);
    }

    internal static List<string> GetTokens(string path)
    {
        var delimiter = "";
        if (path.Contains("\""))
            delimiter = "\"";
        else if (path.Contains("/"))
            delimiter = "/";
        else
        {
            ThrowEx.NotImplementedCase(path);
        }

        return SHSplit.Split(path, delimiter);
    }

    internal static List<string> OnlyNamesWithoutExtensionCopy(List<string> filePaths)
    {
        var result = new List<string>(filePaths.Count);
        for (var i = 0; i < filePaths.Count; i++)
            result.Add(Path.GetFileNameWithoutExtension(filePaths[i]));
        return result;
    }

    internal static string ReplaceDirectoryThrowExceptionIfFromDoesntExists(string path, string folderWithProjectsFolders, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        path = SH.FirstCharUpper(path);
        folderWithProjectsFolders = SH.FirstCharUpper(folderWithProjectsFolders);
        folderWithTemporaryMovedContentWithoutBackslash = SH.FirstCharUpper(folderWithTemporaryMovedContentWithoutBackslash);
        if (!ThrowEx.NotContains(path, folderWithProjectsFolders))
            // Here can never accomplish when exc was throwed
            return path;
        // Here can never accomplish when exc was throwed
        return path.Replace(folderWithProjectsFolders, folderWithTemporaryMovedContentWithoutBackslash);
    }

    internal static string MakeUncLongPath(ref string path)
    {
        if (!path.StartsWith(@"\\?\"))
        {
        // V ASP.net mi vrátilo u každé directory.exists false. Byl jsem pod ApplicationPoolIdentity v IIS a bylo nastaveno Full Control pro IIS AppPool\DefaultAppPool
        }

        return path;
    }

    internal static string MakeUncLongPath(string path)
    {
        return MakeUncLongPath(ref path);
    }
}
