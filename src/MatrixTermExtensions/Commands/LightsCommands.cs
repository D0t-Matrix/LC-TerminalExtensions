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
    [CommandInfo("Turns the lights on or off.", "Lights [on/off]")]
    public string SwitchLightsCommand([RemainingText] string subcommand)
        => subcommand.ToLowerInvariant() switch
        {
            "on" or "up" => LightsOnCommand(),
            "off" or "out" => LightsOffCommand(),
            _ => ParameterError,
        };

    [TerminalCommand("Lightup", false)]
    [CommandInfo("Turns the lights on, if not already on")]
    public string LightsOnCommand()
    {
        return TrySwitchLights(true);
    }

    [TerminalCommand("lightout", false)]
    [CommandInfo("Turns the lights off, if not already off")]
    public string LightsOffCommand()
    {
        return TrySwitchLights(false);
    }

    [TerminalCommand("Lightsout", false)]
    [CommandInfo("Turns the lights off, if not already off")]
    public string LightsOutCommand() => LightsOffCommand();

    private static string TrySwitchLights(bool switchOn)
    {
        if (!StartOfRound.Instance.shipRoomLights.enabled)
        {
            return LightsDisabled;
        }

        InteractTrigger lightSwitchTrigger = GameObject.Find("LightSwitch").GetComponent<InteractTrigger>();

        if (switchOn)
        {
            if (!StartOfRound.Instance.shipRoomLights.areLightsOn)
            {
                lightSwitchTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
                return LightsTurnedOn;
            }
            else
            {
                return LightsAlreadyOn;
            }
        }
        else
        {
            if (StartOfRound.Instance.shipRoomLights.areLightsOn)
            {
                lightSwitchTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);
                return LightsTurnedOff;
            }
            else
            {
                return LightsAlreadyOff;
            }
        }
    }
}
