namespace SunamoDevCode._sunamo.SunamoCollectionsChangeContent;

internal class CAChangeContent
{
    private static void RemoveNullOrEmpty(ChangeContentArgsDC args, List<string> list)
    {
        if (args != null)
        {
            if (args.RemoveNull)
            {
                list.Remove(null!);
            }
            if (args.RemoveEmpty)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (list[i].Trim() == string.Empty)
                    {
                        list.RemoveAt(i);
                    }
                }
            }
        }
    }

    // Direct edit - Changes content of list using provided function with 0 additional parameters
    // If not every element fulfills pattern, it is good to remove null (or values returned if can't be changed) from result
    internal static List<string> ChangeContent0(ChangeContentArgsDC args, List<string> list, Func<string, string> func)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = func.Invoke(list[i]);
        }
        RemoveNullOrEmpty(args, list);
        return list;
    }

    // Direct edit - Changes content of list using provided function with 1 additional parameter
    internal static List<string> ChangeContent1(ChangeContentArgsDC args, List<string> list, Func<string, string, string> func, string argument1)
    {
        var result = ChangeContent<string>(args, list, func, argument1);
        return result;
    }

    #region Switch first and second argument
    internal static List<string> ChangeContentSwitch12<Arg1>(List<string> list, Func<Arg1, string, string> func, Arg1 argument)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = func.Invoke(argument, list[i]);
        }
        return list;
    }

    // Direct edit input collection
    // Changes content of list using provided function with 1 additional parameter
    internal static List<string> ChangeContent<Arg1>(ChangeContentArgsDC args, List<string> list, Func<string, Arg1, string> func, Arg1 argument, Func<Arg1, string, string>? funcSwitch12 = null)
    {
        args ??= new();
        if (args.SwitchFirstAndSecondArg)
        {
            list = ChangeContentSwitch12<Arg1>(list, funcSwitch12!, argument);
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = func.Invoke(list[i], argument);
            }
        }
        RemoveNullOrEmpty(args, list);
        return list;
    }
    #endregion

    #region ChangeContent for easy copy
    // Direct edit - Changes content of list using provided function with 2 additional parameters
    internal static List<string> ChangeContent<Arg1, Arg2>(ChangeContentArgsDC args, List<string> list, Func<string, Arg1, Arg2, string> func, Arg1 argument1, Arg2 argument2)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = func.Invoke(list[i], argument1, argument2);
        }
        RemoveNullOrEmpty(args, list);
        return list;
    }
    #endregion
}
