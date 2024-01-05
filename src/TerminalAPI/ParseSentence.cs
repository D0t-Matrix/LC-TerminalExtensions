using TerminalAPI.Models;
using TerminalAPI.Patches;
using UnityEngine.Video;

namespace TerminalAPI
{
    [HarmonyPatch(typeof(Terminal), "ParsePlayerSentence")]
    public static class ParseSentence
    {
        [HarmonyPrefix]
        public static bool ParsePrefix(
            Terminal __instance, 
            ref TerminalNode __state)
        {
            var command = __instance.screenText.text[^__instance.textAdded..];
            __state = CommandHandler.TryExecute(command, __instance)!;
            return Equals(__state, null);
        }

        [HarmonyPostfix]
        public static TerminalNode ParsePostFix(
            TerminalNode __result, 
            TerminalNode __state, 
            Terminal __instance)
        {
            if (!Equals(__state, null))
            {
                TerminalSubmitPatch.LastNode = __state;
                return __state;
            }
            if (__instance.videoPlayer.source == (VideoSource)1)
                __instance.videoPlayer.source = 0;
            TerminalSubmitPatch.LastNode = __result;
            return __result;
        }
    }
}
