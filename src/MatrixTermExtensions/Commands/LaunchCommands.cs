using TerminalAPI.Attributes;
using TerminalAPI.Models;
using UnityEngine;

namespace MatrixTermExtensions.Commands;

public class LaunchCommands
{

    const string AlreadyInTransitMessage = "Unable to comply as the ship is already in transit.";

    [TerminalCommand("Launch")]
    [CommandInfo("ooh, what does this lever do?")]
    public string LaunchCommand()
    {
        var leverObject = GameObject.Find("StartGameLever");
        if (leverObject is null) return "!! Can't find StartGameLever component !!";

        var lever = leverObject.GetComponent<StartMatchLever>();
        if (lever is null) return "!! Can't find StartMatchLever component !!";

        //! Doors are enabled (on a moon), and the ship is either leaving or not landed
        if (StartOfRound.Instance.shipDoorsEnabled &&
            !(StartOfRound.Instance.shipHasLanded
                || StartOfRound.Instance.shipIsLeaving))
        {
            return AlreadyInTransitMessage;
        }

        //! Doors are disabled (in space), and the ship is in transit to another moon
        if (!StartOfRound.Instance.shipDoorsEnabled
            && StartOfRound.Instance.travellingToNewLevel)
        {
            return AlreadyInTransitMessage;
        }

        var newState = !lever.leverHasBeenPulled;
        lever.PullLever();
        lever.LeverAnimation();
        if (newState)
        {
            lever.StartGame();
        }
        else
        {
            lever.EndGame();
        }

        var sequenceName = lever.leverHasBeenPulled ? "landing" : "launch";
        return $"Initiating {sequenceName} sequence.";
    }

    [TerminalCommand("Go")]
    [CommandInfo("ooh, what does this lever do?")]
    public string GoCommand() => LaunchCommand();

}
