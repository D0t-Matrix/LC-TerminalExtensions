using LethalAPI.LibTerminal.Attributes;
using UnityEngine;

namespace Matrix.TerminalExtensions.Commands;

public class DoorCommands
{
    [TerminalCommand("Door", false)]
    [CommandInfo("Opens or closes the door", "Door [open/close]")]
    public string DoorCommand([RemainingText] string subcommand)
        => subcommand.ToLowerInvariant() switch
        {
            "open" => OpenCommand(),
            "close" => CloseCommand(),
            _ => "Invalid Parameter!",
        };

    [TerminalCommand("Open", false)]
    [CommandInfo("Opens doors, if not already open.")]
    public string OpenCommand()
    {
        if (TryOpenDoors())
            return "Doors Opened.";
        return "Doors Already Open!";
    }

    [TerminalCommand("Close", false)]
    [CommandInfo("Closes doors, if not already closed.")]
    public string CloseCommand()
    {
        if (TryCloseDoors())
            return "Doors Closed.";
        return "Doors Already Closed!";
    }

    private static bool TryOpenDoors()
    {
        if (StartOfRound.Instance.hangarDoorsClosed)
        {
            InteractTrigger openTrigger = GameObject.Find("StartButton").GetComponentInChildren<InteractTrigger>();

            openTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);

            return true;
        }

        return false;
    }

    private static bool TryCloseDoors()
    {
        if (!StartOfRound.Instance.hangarDoorsClosed)
        {
            InteractTrigger closeTrigger = GameObject.Find("StopButton").GetComponentInChildren<InteractTrigger>();

            closeTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);

            return true;
        }

        return false;
    }
}
