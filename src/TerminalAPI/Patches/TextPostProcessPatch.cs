namespace TerminalAPI.Patches;

[HarmonyPatch(typeof(Terminal), "TextPostProcess")]
public class TextPostProcessPatch
{
    [HarmonyPrefix]
    public static void Prefix(ref string modifiedDisplayText)
    {
        modifiedDisplayText = modifiedDisplayText.TrimStart('\n', ' ');
        if (modifiedDisplayText.EndsWith('\n'))
            return;

        modifiedDisplayText += "\n";
    }
}
