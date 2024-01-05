using UnityEngine;

namespace TerminalAPI.Patches;

[HarmonyPatch(typeof(Terminal), "selectTextFieldDelayed")]
public static class SelectTextFieldPatch
{
    [HarmonyPrefix]
    public static bool Prefix() => false;

    [HarmonyPostfix]
    public static void Postfix(Terminal __instance, ref IEnumerator __result)
    {
        __result = Patch(__instance);
    }

    private static IEnumerator Patch(Terminal terminal)
    {
        yield return new WaitForSeconds(0.2f);
        terminal.screenText.ActivateInputField();
        terminal.screenText.Select();
    }
}
