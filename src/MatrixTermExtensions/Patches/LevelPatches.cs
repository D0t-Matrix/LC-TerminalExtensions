using MatrixTermExtensions.Commands;

namespace MatrixTermExtensions.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal sealed class LevelPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(RoundManager.LoadNewLevel))]
    static void LoadNewLevel()
    {
        ResetInverseTeleporter();
    }

    private static bool ResetInverseTeleporter()
    {
        var inverseTeleporter = TeleporterCommands.FindTeleporter(true);

        if (inverseTeleporter is null) return false;

        inverseTeleporter.buttonTrigger.currentCooldownValue = 5;
        inverseTeleporter.buttonTrigger.interactable = true;

        return true;
    }
}
