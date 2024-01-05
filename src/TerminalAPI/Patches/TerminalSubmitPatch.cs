using BepInEx.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace TerminalAPI.Patches;

[HarmonyPatch(typeof(Terminal), "OnSubmit")]
public class TerminalSubmitPatch
{
    private static readonly ManualLogSource _logSource = new("Matrix.TerminalAPI");
    public static TerminalNode? LastNode { get; set; }

    [HarmonyPrefix]
    public static void Prefix() => LastNode = null;

    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var instructionsArr = instructions.ToArray();
        for (int i = instructionsArr.Length - 1; i >= 0; --i)
        {
            if (!(instructionsArr[i].opcode != OpCodes.Callvirt))
            {
                if (instructionsArr[i + 1].opcode != OpCodes.Ldarg_0)
                {
                    ReportTranspileError("Ldarg_0 expected after final callVirt, not found");
                    return instructionsArr;
                }
                instructionsArr[i + 1] = new CodeInstruction(OpCodes.Ret, null);
                return instructionsArr;
            }
        }
        ReportTranspileError("Failed to find Callvirt in backward scan");
        return instructionsArr;
    }

    private static void ReportTranspileError(string message)
    {
        _logSource.LogError("Failed to transpile OnSubmit to remove ScrollToBottom. Did the method get modified in an update? (" + message + ")");
        _logSource.LogWarning("This won't break the mod, but it will cause some odd terminal scrolling behavior.");
    }

    //[HarmonyPostfix]
    //public static void Postfix(Terminal __instance, ref Coroutine __forceScrollbarCoroutine)
    //{
    //    if (Equals(LastNode, null) || LastNode.clearPreviousText)
    //        ExecuteScrollCoroutine(__instance, ref __forceScrollbarCoroutine);
    //    else
    //        __instance.StartCoroutine("forceScrollbarDown");
    //}

    //private static void ExecuteScrollCoroutine(Terminal terminal, ref Coroutine forceScrollbarCoroutine)
    //{
    //    if (forceScrollbarCoroutine != null)
    //        terminal.StopCoroutine(forceScrollbarCoroutine);

    //    forceScrollbarCoroutine = terminal.StartCoroutine("forceScrollbarUp");
    //}
}
