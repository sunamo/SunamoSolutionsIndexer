namespace SunamoDevCode;

// Helper class for System.Windows controls shortcuts and names
internal static class SystemWindowsControls
{
    private static readonly Type type = typeof(SystemWindowsControls);
    private static bool s_initialized;
    private static readonly Dictionary<string, List<string>> s_controls = new();
    private static EmbeddedResourcesH? s_embeddedResourcesH;

    private static Dictionary<string, string>? controlsShortLong;

    public static void InitControlsShortLong()
    {
        Init();

        if (controlsShortLong is null)
        {
            controlsShortLong = new Dictionary<string, string>();

            foreach (var item in s_controls)
            foreach (var shortcut in item.Value)
                controlsShortLong.Add(shortcut, item.Key);
        }
    }

    public static void Init()
    {
        if (!s_initialized)
        {
            s_initialized = true;

            s_embeddedResourcesH = new EmbeddedResourcesH(type.Assembly, "SunamoDevCode");
        }
    }

    public static bool StartingWithShortcutOfControl(string text)
    {
        foreach (var item in s_controls)
        foreach (var shortcut in item.Value)
            if (shortcut.Length > 2)
                if (text.StartsWith(shortcut))
                    return true;

        return false;
    }

    public static bool IsShortcutOfControl(string text)
    {
        foreach (var item in s_controls)
        foreach (var shortcut in item.Value)
            if (shortcut == text)
                return true;

        return false;
    }

    public static bool IsNameOfControl(string text) => s_controls.ContainsKey(text);
}
