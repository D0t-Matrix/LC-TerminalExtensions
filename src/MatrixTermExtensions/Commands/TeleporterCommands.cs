using LethalAPI.LibTerminal.Attributes;
using Matrix.TerminalExtensions.Extensions;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Matrix.TerminalExtensions.Commands;

public class TeleporterCommands
{
    [TerminalCommand("teleport", false)]
    [CommandInfo("Activates teleporter.")]
    public string TeleportCommand()
    {
        if (!TryGetTeleporter(out var teleporter, out var errorMessage, isInverted: false))
            return errorMessage;

        if (!teleporter.buttonTrigger.interactable)
        {
            return $"Teleporter is on cooldown, {teleporter.buttonTrigger.interactCooldown} seconds left!";
        }

        teleporter.buttonTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
        return "Teleporting Player..";
    }

    [TerminalCommand("inverse", false)]
    [CommandInfo("Activates Inverse Teleporter.")]
    public string InverseTeleportCommand()
    {
        if (!TryGetTeleporter(out var inverseTeleporter, out var errorMessage, isInverted: true))
            return errorMessage;

        if (!inverseTeleporter.buttonTrigger.interactable)
        {
            return $"Teleporter is on cooldown, {inverseTeleporter.buttonTrigger.interactCooldown} seconds left!";
        }

        inverseTeleporter.buttonTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
        return "Teleporting Player..";
    }

    [TerminalCommand("tp", false)]
    [CommandInfo("Activates teleporter.")]
    public string TpCommand() => TeleportCommand();

    [TerminalCommand("itp", false)]
    [CommandInfo("Activates the Inverse Teleporter.")]
    public string ItpCommand() => InverseTeleportCommand();

    internal static bool TryGetTeleporter([NotNullWhen(true)] out ShipTeleporter? teleporter, out string errorMessage, bool isInverted = false)
    {
        errorMessage = string.Empty;
        var teleporterObjName = isInverted
            ? "InverseTeleporter(Clone)"
            : "TeleporterClone";

        GameObject teleporterObject = GameObject.Find(teleporterObjName);

        if (teleporterObject is null)
        {
            teleporter = null;
            errorMessage = "No teleporter in ship!";
            return false;
        }

        teleporter = teleporterObject!.GetComponent<ShipTeleporter>();

        if (teleporter is null)
        {
            teleporter = null;
            errorMessage = "<!!!> Cannot locate ShipTeleporter Component! <!!!>";
            return false;
        }

        return errorMessage.IsNullOrWhitespace();
    }

    internal static bool TryResetInverseTeleporter()
    {
        if (!TryGetTeleporter(out var teleporter, out string errorMsg, isInverted: true))
        {
            Plugin.Logger.LogDebug("Unable to reset inverse teleporter.");
            Plugin.Logger.LogDebug(errorMsg);

            return false;
        }

        teleporter.buttonTrigger.currentCooldownValue = 0;
        teleporter.buttonTrigger.interactable = true;

        return true;
    }
}
