namespace SunamoDevCode._sunamo.SunamoThisApp;

internal class ThisApp
{
    internal static string EventLogName = null!;

    internal static string Name = null!;

    internal static bool Check;

    internal static void Success(string message, params string[] args)
    {
        SetStatus(TypeOfMessageShared.Success, message, args);
    }

    internal static void Info(string message, params string[] args)
    {
        SetStatus(TypeOfMessageShared.Information, message, args);
    }

    internal static void Error(string message, params string[] args)
    {
        SetStatus(TypeOfMessageShared.Error, message, args);
    }

    internal static void Appeal(string message, params string[] args)
    {
        SetStatus(TypeOfMessageShared.Appeal, message, args);
    }

    internal static void SetStatus(TypeOfMessageShared messageType, string status, params string[] args)
    {
        var formattedMessage = string.Format(status, args);
        if (formattedMessage.Trim() != string.Empty)
        {
            if (StatusSetted == null)
            {
                // For unit tests - no handler attached
            }
            else
            {
                StatusSetted(messageType, formattedMessage);
            }
        }
    }

    internal static event Action<TypeOfMessageShared, string>? StatusSetted;
}
