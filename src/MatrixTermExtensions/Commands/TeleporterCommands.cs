using LethalAPI.LibTerminal.Attributes;
using Matrix.TerminalExtensions.Extensions;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Matrix.TerminalExtensions.Commands;

public class TeleporterCommands
{
    #region strings

    const string TeleporterCooldownFieldName = "cooldownTime";
    const string TeleporterObjectName = "Teleporter(Clone)";
    const string InverseTeleporterObjectName = "InverseTeleporter(Clone)";
    const string TeleporterCooldown = "Teleporter is on cooldown, {0:D} seconds left!";
    const string InverseTeleporterCooldown = "Inverse Teleporter is on cooldown, {0:D} seconds left!";
    const string TeleportingPlayer = "Teleporting Player..";
    const string InverseTeleporting = "Tally ho, lads!";
    const string NoTeleporterInShip = "No Teleporter in ship!";
    const string ShipTeleporterComponentNotFound = "<!!!> Cannot locate ShipTeleporter Component! <!!!>";
    const string UnableToResetInverted = "Unable to reset inverse teleporter.";
    const string InSpaceError = "Can't use the Inverse Teleporter in space!";

    #endregion

    #region Commands
    [TerminalCommand("Teleport", false)]
    [CommandInfo("Activates teleporter.")]
    public string TeleportCommand()
    {
        if (!TryGetTeleporter(out var teleporter, out var errorMessage, isInverted: false))
            return errorMessage;

        if (!teleporter.buttonTrigger.interactable)
        {
            var cooldown = (int)Utils.GetInstancedStructField<float>(teleporter, nameof(ShipTeleporter.cooldownTime));

            return string.Format(TeleporterCooldown, cooldown);
        }

        teleporter.buttonTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
        return TeleportingPlayer;
    }

    [TerminalCommand("iTeleport", false)]
    [CommandInfo("Activates Inverse Teleporter.")]
    public string InverseTeleportCommand()
    {
        if (!StartOfRound.Instance.shipDoorsEnabled)
            return InSpaceError;

        if (!TryGetTeleporter(out var inverseTeleporter, out var errorMessage, isInverted: true))
            return errorMessage;

        if (!inverseTeleporter.buttonTrigger.interactable)
        {
            var cooldown = (int)Utils.GetInstancedStructField<float>(inverseTeleporter, nameof(ShipTeleporter.cooldownTime));

            return string.Format(InverseTeleporterCooldown, cooldown);
        }

        inverseTeleporter.buttonTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
        return InverseTeleporting;
    }

    [TerminalCommand("tp", false)]
    [CommandInfo("Activates teleporter.")]
    public string TpCommand() => TeleportCommand();

    [TerminalCommand("itp", false)]
    [CommandInfo("Activates the Inverse Teleporter.")]
    public string ItpCommand() => InverseTeleportCommand();

    #endregion

    #region Helper Methods
    internal static bool TryGetTeleporter([NotNullWhen(true)] out ShipTeleporter? teleporter, out string errorMessage, bool isInverted = false)
    {
        errorMessage = string.Empty;
        var teleporterObjName = isInverted
            ? InverseTeleporterObjectName
            : TeleporterObjectName;

        GameObject teleporterObject = GameObject.Find(teleporterObjName);

        if (teleporterObject is null)
        {
            teleporter = null;
            errorMessage = NoTeleporterInShip;
            return false;
        }

        teleporter = teleporterObject!.GetComponent<ShipTeleporter>();

        if (teleporter is null)
        {
            teleporter = null;
            errorMessage = ShipTeleporterComponentNotFound;
            return false;
        }

        return errorMessage.IsNullOrWhitespace();
    }

    internal static bool TryResetInverseTeleporter()
    {
        if (!TryGetTeleporter(out var teleporter, out string errorMsg, isInverted: true))
        {
            Plugin.Logger.LogDebug(UnableToResetInverted);
            Plugin.Logger.LogDebug(errorMsg);

            return false;
        }
        

        Plugin.Logger.LogInfo("Inverse Teleporter loaded. Resetting cooldown");

        Utils.SetInstancedStructField(teleporter, nameof(ShipTeleporter.cooldownTime), 0f);
        teleporter.enabled = true;

        teleporter.buttonTrigger.cooldownTime = 0;
        teleporter.buttonTrigger.interactable = true;

        var cooldown = (int)Utils.GetInstancedStructField<float>(teleporter, nameof(ShipTeleporter.cooldownTime));

        Plugin.Logger.LogInfo(string.Format(InverseTeleporterCooldown, cooldown));

        return true;
    }

    #endregion
}
