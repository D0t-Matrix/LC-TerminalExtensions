using LethalAPI.LibTerminal.Attributes;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Matrix.TerminalExtensions.Commands;

public class LaunchCommands
{
    #region Strings

    const string StartGameLever = "StartGameLever";
    const string alreadyInTransitMsg = "Unable to comply. Ship is alredy in tranist.";
    const string CantFindStartComponent = "<!!!> Can't find StartMatchLever Component <!!!>";
    const string InitiatingLaunch = "Initiating launch sequence.";
    const string InitiatingLanding = "Initiating landing sequence.";
    const string AlreadyInSpace = "Already in space!";
    const string AlreadyLanded = "Already landed on moon!";

    #endregion

    #region Commands

    [TerminalCommand("Launch", false)]
    public string LaunchCommand()
    {
        if (!TryGetShipLever(out var lever))
            return CantFindStartComponent;

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

        return PullLever(lever);
    }

    [TerminalCommand("Go", false)]
    [CommandInfo("Pull the lever, Kronk!")]
    public string GoCommand() => LaunchCommand();

    [TerminalCommand("TakeOff", false)]
    [CommandInfo("Takes off from moon.")]
    public string TakeOffCommand()
    {
        if (!TryGetShipLever(out var lever))
            return CantFindStartComponent;

        if (!StartOfRound.Instance.shipDoorsEnabled)
            return AlreadyInSpace;

        //! Doors are enabled (on a moon), and ship is either not landed or already leaving
        if (StartOfRound.Instance.shipDoorsEnabled
            && !(StartOfRound.Instance.shipHasLanded
                || StartOfRound.Instance.shipIsLeaving))
        {
            return alreadyInTransitMsg;
        }


        return PullLever(lever);
    }

    [TerminalCommand("Land", false)]
    [CommandInfo("Lands ship on routed moon.")]
    public string LandCommand()
    {
        if (!TryGetShipLever(out var lever))
            return CantFindStartComponent;

        if (StartOfRound.Instance.shipDoorsEnabled)
            return AlreadyLanded;

        //! Doors are disabled (in space), and ship is in transit to another moon.
        if (!StartOfRound.Instance.shipDoorsEnabled
            && StartOfRound.Instance.travellingToNewLevel)
        {
            return alreadyInTransitMsg;
        }


        return PullLever(lever);
    }

    #endregion

    private static bool TryGetShipLever([NotNullWhen(true)] out StartMatchLever? startMatchLever)
    {
        startMatchLever = null;

        GameObject leverObject = GameObject.Find(StartGameLever);
        if (leverObject is null) return false;

        startMatchLever = leverObject.GetComponent<StartMatchLever>();
        
        return startMatchLever is not null;
    }

    private static string PullLever(StartMatchLever lever)
    {
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
}
