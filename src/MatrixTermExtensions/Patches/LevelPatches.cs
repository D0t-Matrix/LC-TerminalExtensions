using Matrix.TerminalExtensions.Commands;

namespace Matrix.TerminalExtensions.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal sealed class LevelPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(RoundManager.LoadNewLevel))]
    static void LoadNewLevel()
    {
        Plugin.Logger.LogInfo("Resetting the Inverse Teleporter Cooldown");
        TeleporterCommands.TryResetInverseTeleporter();
    }
}
