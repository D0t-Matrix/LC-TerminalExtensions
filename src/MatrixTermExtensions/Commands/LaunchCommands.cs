using LethalAPI.LibTerminal.Attributes;
using UnityEngine;

namespace Matrix.TerminalExtensions.Commands;

public class LaunchCommands
{
    const string alreadyInTransitMsg = "Unable to comply. Ship is alredy in tranist.";

    [TerminalCommand("Launch", false)]
    [CommandInfo("Pull the lever, Kronk!")]
    public string LaunchCommand()
    {
        GameObject leverObject = GameObject.Find("StartGameLever");
        if (leverObject is null) return "<!!!> Can't find StartGameLever <!!!>";

        StartMatchLever lever = leverObject.GetComponent<StartMatchLever>();
        if (lever is null) return "<!!!> Can't find StartMatchLever Component <!!!>";

        //! Doors are enabled (on a moon), and ship is either not landed or already leaving
        if (StartOfRound.Instance.shipDoorsEnabled
            && !(StartOfRound.Instance.shipHasLanded
                || StartOfRound.Instance.shipIsLeaving))
        {
            return alreadyInTransitMsg;
        }

        //! Doors are disabled (in space), and ship is in transit to another moon.
        if (!StartOfRound.Instance.shipDoorsEnabled
            && StartOfRound.Instance.travellingToNewLevel)
        {
            return alreadyInTransitMsg;
        }

        bool newState = !lever.leverHasBeenPulled;
        lever.PullLever();
        lever.LeverAnimation();

        if (newState)
            lever.StartGame();
        else
            lever.EndGame();

        return "Initiating " + (lever.leverHasBeenPulled ? "landing" : "launch") + " sequence.";
    }

    [TerminalCommand("Go", false)]
    [CommandInfo("Pull the lever, Kronk!")]
    public string GoCommand() => LaunchCommand();
}
