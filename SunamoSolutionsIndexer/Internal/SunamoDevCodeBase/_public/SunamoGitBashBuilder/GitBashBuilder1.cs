namespace SunamoSolutionsIndexer.Internal._public.SunamoGitBashBuilder;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class GitBashBuilder : IGitBashBuilder
{
    public static List<string>? PrepareFilesToSimpleGitFormat(string solution, List<string> linesFiles, out bool anyError, string searchOnlyWithExtension, string basePathIfA2SolutionsWontExistsOnFilesystem)
    {
        searchOnlyWithExtension = searchOnlyWithExtension.TrimStart('*');
        anyError = false;
        string? pathSearchForFiles = null;
        if (Directory.Exists(solution))
        {
            pathSearchForFiles = solution;
        }
        else
        {
            pathSearchForFiles = Path.Combine(basePathIfA2SolutionsWontExistsOnFilesystem, solution);
        }

        string pathRepository = pathSearchForFiles;
        if (solution == "sunamo.cz")
        {
            pathSearchForFiles += "\"" + solution;
        }

        FS.WithEndSlash(ref pathRepository);
        var files = Directory.GetFiles(pathSearchForFiles, "*.*", System.IO.SearchOption.AllDirectories).ToList();
        files = files.Where(d => !d.Contains(@"\.git\")).ToList();
        CA.Replace(linesFiles, solution, string.Empty);
        CAChangeContent.ChangeContent1(null!, linesFiles, SHParts.RemoveAfterFirst, "-");
        CA.Trim(linesFiles);
        CAChangeContent.ChangeContent1(null!, linesFiles, FS.AddExtensionIfDontHave, searchOnlyWithExtension);
        CAChangeContent.ChangeContent<bool>(null!, linesFiles, FS.Slash, true);
        CAChangeContent.ChangeContent1(null!, linesFiles, SHTrim.TrimStart, "/");
        var linesFilesOnlyFilename = FS.OnlyNamesNoDirectEdit(linesFiles);
        anyError = false;
        List<string> filesToCommit = new List<string>();
        Dictionary<string, List<string>> dictPsychicallyExistsFiles = FS.GetDictionaryByFileNameWithExtension(files);
        CA.Replace(files, "\"", "/");
        pathRepository = FS.Slash(pathRepository, false);
        for (int i = 0; i < linesFiles.Count; i++)
        {
            var item = linesFilesOnlyFilename[i];
            var itemWithoutTrim = linesFiles[i];
#region Directory\*
            if (item[item.Length - 1] == '*')
            {
                item = itemWithoutTrim.TrimEnd('*');
                string itemWithoutTrimBackslashed = Path.Combine(pathRepository, FS.Slash(item, false));
                if (Directory.Exists(itemWithoutTrimBackslashed))
                {
                    filesToCommit.Add(item + "*");
                }
                else
                {
                    anyError = true;
                }
            }
#endregion
#region *File - add all files without specify root directory
            else if (item[0] == '*')
            {
                string file = item.Substring(1);
                if (dictPsychicallyExistsFiles.ContainsKey(file))
                {
                    foreach (var item2 in dictPsychicallyExistsFiles[file])
                    {
                        filesToCommit.Add(FS.Slash(item2.Replace(pathRepository, string.Empty), true));
                    }
                }
            }
#endregion
#region Exactly defined file
            else
            {
                var fullPath = item;
                item = Path.GetFileName(item);
#region File isnt in dict => Dont exists
                if (!dictPsychicallyExistsFiles.ContainsKey(item))
                {
                    anyError = true;
                }
#endregion
                else
                {
                    string itemWithoutTrimBackslashed = Path.Combine(pathRepository, FS.Slash(itemWithoutTrim, false));
#region Add as relative file
                    if (itemWithoutTrim.Contains("/"))
                    {
                        if (File.Exists(itemWithoutTrimBackslashed))
                        {
                            filesToCommit.Add(itemWithoutTrim.Replace(pathRepository, string.Empty));
                        }
                        else
                        {
                            anyError = true;
                        }
                    }
#endregion
#region Add file in root of repository
                    else
                    {
                        if (dictPsychicallyExistsFiles[item].Count == 1)
                        {
                            filesToCommit.Add(FS.Slash(dictPsychicallyExistsFiles[item][0].Replace(pathRepository, string.Empty), true));
                        }
                        else
                        {
                            anyError = true;
                        }
                    }
#endregion
                }
            }
#endregion
        }

        if (anyError)
        {
            return null;
        }

        return filesToCommit;
    }

    public static string SomeErrorsOccured { get; set; } = "SomeErrorsOccured";
#pragma warning disable
    public static string CreateGitCommandForFiles(string command, StringBuilder sb, List<string> linesFiles)
    {
        return null;
    }

#pragma warning restore
    public void Cd(string key)
    {
        StringBuilder.AppendLine("cd " + SH.WrapWith(key, "\""));
    }

    public void Clear()
    {
        StringBuilder.Clear();
    }

    public void Append(string text)
    {
        StringBuilder.Append(text + " ");
    }

    public void AppendLine(string text)
    {
        StringBuilder.AppendLine(text);
    }

    public void AppendLine()
    {
        StringBuilder.AppendLine();
    }

    public override string ToString()
    {
        return StringBuilder.ToString();
    }
}
