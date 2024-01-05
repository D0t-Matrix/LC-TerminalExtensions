using TerminalAPI.Attributes;
using TerminalAPI.Models;
using MatrixTermExtensions.Extensions;
using UnityEngine;

namespace MatrixTermExtensions.Commands;

public class DoorCommands
{

    [TerminalCommand("Open")]
    [CommandInfo("Opens the door.")]
    public string OpenDoorCommand()
    {
        var trigger = GameObject.Find("StartButton").GetComponentInChildren<InteractTrigger>();
        trigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);

        return "Opened door";
    }

    [TerminalCommand("Close")]
    [CommandInfo("Closes the door.")]
    public string CloseDoorCommand()
    {
        var trigger = GameObject.Find("StopButton").GetComponentInChildren<InteractTrigger>();
        trigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);

        return "Closed door";
    }

    public string ToggleDoor()
    {
        var buttonName = StartOfRound.Instance.hangarDoorsClosed
            ? "StartButton"
            : "StopButton";
        var trigger = GameObject.Find(buttonName).GetComponentInChildren<InteractTrigger>();
        trigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);

        return "Toggled door.";
    }
    
    [TerminalCommand("Door")]
    [CommandInfo("Opens or closes the door.", "[Command]")]
    public string DoorCommand([RemainingText] string? subcommand)
    {
        if (subcommand.IsNullOrEmpty()) return "Invalid Command.";

        return subcommand.ToLowerInvariant() switch
        {
            "open" => OpenDoorCommand(),
            "close" => CloseDoorCommand(),
            "toggle" => ToggleDoor(),
            _ => "Invalid Command.",
        };
    }
}
