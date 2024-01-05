using TerminalAPI.Attributes;
using TerminalAPI.Models;
using UnityEngine;

namespace MatrixTermExtensions.Commands;

public class TeleporterCommands
{

    [TerminalCommand("Teleport")]
    [CommandInfo("Activate the teleporter.")]
    public string TeleportCommand()
    {
        var teleporterObject = GameObject.Find("Teleporter(clone)");
        if (teleporterObject is null) return "You don't have a teleporter!";

        var teleporter = teleporterObject.GetComponent<ShipTeleporter>();
        if (teleporter is null) return "!! Can't find ShipTeleporter component !!";

        if (!teleporter.buttonTrigger.interactable) return "Teleporter is on cooldown!";

        teleporter.buttonTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
        return "Teleporting player.";
    }

    [TerminalCommand("Tp")]
    [CommandInfo("Activate the teleporter.")]
    public string TeleportShortCommand() => TeleportCommand();

    [TerminalCommand("InverseTeleport")]
    [CommandInfo("Activate the Inverse Teleporter.")]
    public string InverseTeleportCommand()
    {
        var inverseTeleporter = FindTeleporter(true);
        if (inverseTeleporter is null)
        {
            return "You don't have an Inverse Teleporter!";
        }

        if (!inverseTeleporter.buttonTrigger.interactable) return "Inverse Teleporter is on cooldown!";

        inverseTeleporter.buttonTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
        return "Teleporting player(s) out!";
    }

    internal static ShipTeleporter? FindTeleporter(bool isInverse = false)
    {
        var teleporterName = "Teleporter(clone)";
        if (isInverse) 
            teleporterName = "Inverse" + teleporterName;

        var teleporterObject = GameObject.Find(teleporterName);
        return teleporterObject?.GetComponent<ShipTeleporter>();
    }

    [TerminalCommand("Inverse")]
    [CommandInfo("Activate the Inverse Teleporter.")]
    public string InverseTeleportShortCommand() => InverseTeleportCommand();

    [TerminalCommand("itp")]
    [CommandInfo("Activate the Inverse Teleporter.")]
    public string InverseTeleportShorterCommand() => InverseTeleportCommand();

}
