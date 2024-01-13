using UnityEngine;
using LethalAPI.LibTerminal.Attributes;

namespace Matrix.TerminalExtensions.Commands;

public class LightsCommands
{
    const string LightsTurnedOn = "Turning lights on.";
    const string LightsTurnedOff = "Turning lights off.";
    const string LightsDisabled = "Unable to switch lights, as they are disabled.";
    const string LightsAlreadyOn = "Lights are already on!";
    const string LightsAlreadyOff = "Lights are already off!";
    const string ParameterError = "Invalid Parameter.";

    [TerminalCommand("Lights", false)]
    [CommandInfo("Toggles the lights on or off")]
    public string SwitchLightsCommand() => TrySwitchLights(LightState.Toggle);

    [TerminalCommand("Lightup", false)]
    public string LightsOnCommand() => TrySwitchLights(LightState.On);

    [TerminalCommand("lightout", false)]
    [CommandInfo("Turns the lights off, if not already off")]
    public string LightsOffCommand() => TrySwitchLights(LightState.Off);

    [TerminalCommand("nox", false)]
    public string NoxCommand() => LightsOffCommand();

    [TerminalCommand("lumos", false)]
    [CommandInfo("Turns the lights off, if not already off")]
    public string LumosCommand() => LightsOnCommand();

    private static string TrySwitchLights(LightState switchState)
    {
        if (!StartOfRound.Instance.shipRoomLights.enabled)
        {
            return LightsDisabled;
        }

        InteractTrigger lightSwitchTrigger = GameObject.Find("LightSwitch").GetComponent<InteractTrigger>();


        switch (switchState)
        {
            case LightState.Off:
                if (StartOfRound.Instance.shipRoomLights.areLightsOn)
                {
                    lightSwitchTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
                    return LightsTurnedOff;
                }
                else
                {
                    return LightsAlreadyOff;
                }
            case LightState.On:
                if (!StartOfRound.Instance.shipRoomLights.areLightsOn)
                {
                    lightSwitchTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
                    return LightsTurnedOn;
                }
                else
                {
                    return LightsAlreadyOn;
                }
            case LightState.Toggle:
                lightSwitchTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);

                return StartOfRound.Instance.shipRoomLights.areLightsOn
                   ? LightsTurnedOn
                   : LightsTurnedOff;
            default:
                return ParameterError;

        }
    }
    private enum LightState
    {
        On,
        Off,
        Toggle
    }
}


