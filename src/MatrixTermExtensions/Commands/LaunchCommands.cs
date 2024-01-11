using LethalAPI.LibTerminal.Attributes;
using UnityEngine;

namespace Matrix.TerminalExtensions.Commands;

public class LaunchCommands
{
    #region Strings

    const string StartGameLever = "StartGameLever";
    const string alreadyInTransitMsg = "Unable to comply. Ship is alredy in tranist.";
    const string CantFindStartLever = "<!!!> Can't find StartGameLever <!!!>";
    const string CantFindStartComponent = "<!!!> Can't find StartMatchLever Component <!!!>";
    const string InitiatingLaunch = "Initiating launch sequence.";
    const string InitiatingLanding = "Initiating landing sequence.";

    #endregion

    #region Commands

    [TerminalCommand("Launch", false)]
    [CommandInfo("Pull the lever, Kronk!")]
    public string LaunchCommand()
    {
        GameObject leverObject = GameObject.Find(StartGameLever);
        if (leverObject is null) return CantFindStartLever;

        StartMatchLever lever = leverObject.GetComponent<StartMatchLever>();
        if (lever is null) return CantFindStartComponent;

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

        return lever.leverHasBeenPulled 
            ? InitiatingLanding
            : InitiatingLaunch;
    }

    [TerminalCommand("Go", false)]
    [CommandInfo("Pull the lever, Kronk!")]
    public string GoCommand() => LaunchCommand();

    #endregion
}
