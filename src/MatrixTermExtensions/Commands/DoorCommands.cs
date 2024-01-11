using LethalAPI.LibTerminal.Attributes;
using UnityEngine;

namespace Matrix.TerminalExtensions.Commands;

public class DoorCommands
{
    #region Strings

    const string StartButton = "StartButton";
    const string StopButton = "StopButton";
    const string OpenParameter = "open";
    const string CloseParameter = "close";
    const string ToggleParameter = "toggle";
    const string InvalidParameter = "Invalid Parameter!";
    const string DoorsOpened = "Doors Opened.";
    const string DoorsClosed = "Doors Closed.";
    const string DoorsAlreadyOpen = "Doors already open!";
    const string DoorsAlreadyClosed = "Doors already closed!";
    const string InSpaceError = "Can't open doors in space!";

    #endregion

    #region Commands

    [TerminalCommand("Door", false)]
    [CommandInfo("Opens or closes the door", "Door [open/close]")]
    public string DoorCommand([RemainingText] string? subcommand)
        => subcommand?.ToLowerInvariant() switch
        {
            OpenParameter => OpenCommand(),
            CloseParameter => CloseCommand(),
            ToggleParameter => ToggleDoors(),
            _ => InvalidParameter,
        };

    [TerminalCommand("Doors", false)]
    [CommandInfo("Opens or closes the door", "Doors [open/close]")]
    public string DoorsCommand([RemainingText] string? subcommand)
        => subcommand?.ToLowerInvariant() switch
        {
            OpenParameter => OpenCommand(),
            CloseParameter => CloseCommand(),
            _ => InvalidParameter,
        };

    [TerminalCommand("Open", false)]
    [CommandInfo("Opens doors, if not already open.")]
    public string OpenCommand()
    {
        if (!StartOfRound.Instance.shipDoorsEnabled)
            return InSpaceError;

        if (TryOpenDoors())
            return DoorsOpened;
        return DoorsAlreadyOpen;
    }

    [TerminalCommand("Close", false)]
    [CommandInfo("Closes doors, if not already closed.")]
    public string CloseCommand()
    {
        if (!StartOfRound.Instance.shipDoorsEnabled)
            return InSpaceError;

        if (TryCloseDoors())
            return DoorsClosed;
        return DoorsAlreadyClosed;
    }

    #endregion

    #region Helper Methods

    private static bool TryOpenDoors()
    {
        if (StartOfRound.Instance.hangarDoorsClosed)
        {
            InteractTrigger openTrigger = GameObject.Find(StartButton).GetComponentInChildren<InteractTrigger>();

            openTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);

            return true;
        }

        return false;
    }

    private static bool TryCloseDoors()
    {
        if (!StartOfRound.Instance.hangarDoorsClosed)
        {
            InteractTrigger closeTrigger = GameObject.Find(StopButton).GetComponentInChildren<InteractTrigger>();

            closeTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);

            return true;
        }

        return false;
    }

    private string ToggleDoors()
        => StartOfRound.Instance.hangarDoorsClosed switch
        {
            true => OpenCommand(),
            false => CloseCommand(),
        };

    #endregion
}
