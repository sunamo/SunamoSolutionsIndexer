namespace SunamoDevCode._sunamo.SunamoFileSystem;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal partial class FS
{
    internal static bool CopyMoveFilePrepare(ref string item, ref string fileTo, FileMoveCollisionOptionDC co)
    {
        //var fileTo = fileTo2.ToString();
        item = @"\\?\" + item;
        MakeUncLongPath(ref fileTo);
        CreateUpfoldersPsysicallyUnlessThere(fileTo);
        // Toto tu je důležité, nevím který kokot to zakomentoval
        if (File.Exists(fileTo))
        {
            if (co == FileMoveCollisionOptionDC.AddFileSize)
            {
                var newFn = InsertBetweenFileNameAndExtension(fileTo, " " + new FileInfo(item).Length);
                if (File.Exists(newFn))
                {
                    File.Delete(item);
                    return true;
                }

                fileTo = newFn;
            }
            else if (co == FileMoveCollisionOptionDC.AddSerie)
            {
                var serie = 1;
                while (true)
                {
                    var newFn = InsertBetweenFileNameAndExtension(fileTo, " (" + serie + ")");
                    if (!File.Exists(newFn))
                    {
                        fileTo = newFn;
                        break;
                    }

                    serie++;
                }
            }
            else if (co == FileMoveCollisionOptionDC.DiscardFrom)
            {
                // Cant delete from because then is file deleting
                if (DeleteFileMaybeLocked != null)
                    DeleteFileMaybeLocked(item);
                else
                    File.Delete(item);
            }
            else if (co == FileMoveCollisionOptionDC.Overwrite)
            {
                if (DeleteFileMaybeLocked != null)
                    DeleteFileMaybeLocked(fileTo);
                else
                    File.Delete(fileTo);
            }
            else if (co == FileMoveCollisionOptionDC.LeaveLarger)
            {
                var fsFrom = new FileInfo(item).Length;
                var fsTo = new FileInfo(fileTo).Length;
                if (fsFrom > fsTo)
                    File.Delete(fileTo);
                else //if (fsFrom < fsTo)
                    File.Delete(item);
            }
            else if (co == FileMoveCollisionOptionDC.DontManipulate)
            {
                if (File.Exists(fileTo))
                    return false;
            }
            else if (co == FileMoveCollisionOptionDC.ThrowEx)
            {
                ThrowEx.Custom($"Directory {fileTo} already exists");
            }
        }

        return true;
    }

    internal static Action<string>? DeleteFileMaybeLocked = null;
    internal static void MoveFile(string item, string fileTo, FileMoveCollisionOptionDC co)
    {
        if (CopyMoveFilePrepare(ref item, ref fileTo, co))
            try
            {
                item = MakeUncLongPath(item);
                fileTo = MakeUncLongPath(fileTo);
                if (co == FileMoveCollisionOptionDC.DontManipulate && File.Exists(fileTo))
                    return;
                File.Move(item, fileTo);
            }
            catch (Exception)
            {
            //ThisApp.Error(item + " : " + ex.Message);
            }
    }

    internal static Func<string, bool, List<Process>>? fileUtilWhoIsLocking = null;
    internal static void CopyFile(string sourceFile, string destinationFile, bool isTerminatingProcessIfInUse = false)
    {
        try
        {
            File.Copy(sourceFile, destinationFile, true);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("because it is being used by another process") && isTerminatingProcessIfInUse)
            {
                if (fileUtilWhoIsLocking != null)
                {
                    var processes = fileUtilWhoIsLocking(sourceFile, true);
                    foreach (var item in processes)
                        item.Kill();
                }

                // Používá se i ve web, musel bych tam includovat spoustu metod
                //PH.ShutdownProcessWhichOccupyFileHandleExe(sourceFile);
                try
                {
                    File.Copy(sourceFile, destinationFile, true);
                }
                catch (Exception)
                {
                // Pokud se to ani na druhý pokus nepodaří, tak už to jebu
                }
            }
            else
            {
                throw;
            }
        }
    }

    private static void MoveOrCopy(string sourceDirectory, string destinationDirectory, FileMoveCollisionOptionDC collisionOption, bool isMoving, string filePath)
    {
        var destinationFile = destinationDirectory + filePath.Substring(sourceDirectory.Length);
        if (isMoving)
            MoveFile(filePath, destinationFile, collisionOption);
        else
            CopyFile(filePath, destinationFile, collisionOption);
    }

    internal static void CopyFile(string item, string fileTo2, FileMoveCollisionOptionDC co)
    {
        var fileTo = fileTo2;
        if (CopyMoveFilePrepare(ref item, ref fileTo, co))
        {
            if (co == FileMoveCollisionOptionDC.DontManipulate && File.Exists(fileTo))
                return;
            File.Copy(item, fileTo);
        }
    }

    private static void CopyMoveAllFilesRecursively(ILogger logger, string sourceDirectory, string destinationDirectory, FileMoveCollisionOptionDC collisionOption, bool isMoving, string mustContain, SearchOption searchOption)
    {
        var files = FSGetFiles.GetFiles(logger, sourceDirectory, "*", searchOption);
        if (!string.IsNullOrEmpty(mustContain))
        {
            foreach (var item in files)
                if (SH.IsContained(item, mustContain))
                {
                    MoveOrCopy(sourceDirectory, destinationDirectory, collisionOption, isMoving, item);
                }
        }
        else
        {
            foreach (var item in files)
                MoveOrCopy(sourceDirectory, destinationDirectory, collisionOption, isMoving, item);
        }
    }

    internal static void MoveAllRecursivelyAndThenDirectory(ILogger logger, string sourceDirectory, string destinationDirectory, FileMoveCollisionOptionDC collisionOption)
    {
        CopyMoveAllFilesRecursively(logger, sourceDirectory, destinationDirectory, collisionOption, true, null!, SearchOption.TopDirectoryOnly);
        var directories = Directory.GetDirectories(sourceDirectory, "*", SearchOption.AllDirectories);
        for (var i = directories.Length - 1; i >= 0; i--)
            TryDeleteDirectory(directories[i]);
        TryDeleteDirectory(sourceDirectory);
    }

    internal static Dictionary<string, List<string>> GetDictionaryByExtension(ILogger logger, string folder, string mask, SearchOption searchOption)
    {
        var extDict = new Dictionary<string, List<string>>();
        foreach (var item in FSGetFiles.GetFiles(logger, folder, mask, searchOption))
        {
            var ext = Path.GetExtension(item);
            var fn = Path.GetFileNameWithoutExtension(item).ToLower();
            if (fn == string.Empty)
            {
                fn = ext;
                ext = "";
            }

            DictionaryHelper.AddOrCreate(extDict, ext, fn);
        }

        return extDict;
    }

    internal static string AddUpfoldersToRelativePath(int levelsUp, string file, char delimiter)
    {
        var jumpUp = ".." + delimiter;
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < levelsUp; i++)
            stringBuilder.Append(jumpUp);
        stringBuilder.Append(file);
        return stringBuilder.ToString();
    //return SHJoin.JoinTimes(i, jumpUp) + file;
    }

    internal static string MascFromExtension(string extension = "*")
    {
        if (char.IsLetterOrDigit(extension[0]))
        {
            // For wildcard, dot (simply non letters) include .
            extension = "." + extension;
        }

        if (!extension.StartsWith("*"))
        {
            extension = "*" + extension;
        }

        if (!extension.StartsWith("*.") && extension.StartsWith("."))
        {
            extension = "*." + extension;
        }

        return extension;
    }

    internal static void CreateUpfoldersPsysicallyUnlessThere(string path)
    {
        CreateFoldersPsysicallyUnlessThere(Path.GetDirectoryName(path)!);
    }

    internal static void CreateFoldersPsysicallyUnlessThere(string path)
    {
        ThrowEx.IsNullOrEmpty(nameof(path), path);
        //ThrowEx.IsNotWindowsPathFormat(nameof(path), path);
        if (Directory.Exists(path))
        {
            return;
        }

        var foldersToCreate = new List<string>
        {
            path
        };
        while (true)
        {
            path = Path.GetDirectoryName(path)!;
            if (Directory.Exists(path))
            {
                break;
            }

            foldersToCreate.Add(path);
        }

        foldersToCreate.Reverse();
        foreach (var folder in foldersToCreate)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
    }
}